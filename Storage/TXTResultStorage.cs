using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Globalization;

using Core;
using Core.Enumerations;
using Core.Attributes;
using Core.Result;
using Core.Utility;

namespace Storage
{
    /// <summary>
    /// Implementation of txt result storage. 
    /// <remark>Making folder with .txt files for global properties and  for each option saparetely.</remark>
    /// </summary>
    class TXTResultStorage : AbstractResultStorage
    {
        private SortedDictionary<Guid, string> existingFolderNames;

        public TXTResultStorage(string str)
            : base(str)
        {
            if (!storageStr.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                storageStr += Path.DirectorySeparatorChar;
            }
            if (!Directory.Exists(storageStr))
            {
                Directory.CreateDirectory(storageStr);
            }
        }

        public override StorageType GetStorageType()
        {
            return StorageType.TXTStorage;
        }

        public override void Save(ResearchResult result)
        {
            string dirName = GetFileName(result.ResearchID, result.ResearchName);

            Directory.CreateDirectory(dirName);

            SaveGeneralInfo(dirName, result.ResearchID, result.ResearchName,
                    result.ResearchType, result.ModelType, result.RealizationCount,
                    result.Size, result.Edges, result.ResearchParameterValues,
                    result.GenerationParameterValues);

            for (int i = 0; i < result.EnsembleResults.Count; ++i)
            {
                SaveEnsembleResult(dirName, result.ResearchName, result.EnsembleResults[i], i);
            }
        }

        public override string GetFileName(Guid researchID, string researchName)
        {
            string dirName = storageStr + researchName;
            if (Directory.Exists(dirName))
                dirName += researchID;
            FileName = Path.GetFileName(dirName);
            return dirName;
        }

        public override void Delete(Guid researchID)
        {
            if (existingFolderNames != null && existingFolderNames.Keys.Contains(researchID))
            {
                Directory.Delete(existingFolderNames[researchID]);
                existingFolderNames.Remove(researchID);
            }
            else
            {
                string fileNameToDelete = FileNameByGuid(researchID);
                if (fileNameToDelete != null)
                    Directory.Delete(fileNameToDelete);
            }
        }

        public override List<ResearchResult> LoadAllResearchInfo()
        {
            existingFolderNames = new SortedDictionary<Guid, string>();
            List<ResearchResult> researchInfos = new List<ResearchResult>();

            ResearchResult researchInfo = null;
            foreach (string folderName in Directory.GetDirectories(storageStr, "*", SearchOption.TopDirectoryOnly))
            {
                researchInfo = new ResearchResult();
                LoadGeneralInfo(folderName, researchInfo);

                researchInfos.Add(researchInfo);
                existingFolderNames.Add(researchInfo.ResearchID, folderName);
            }

            return researchInfos;
        }

        public override ResearchResult Load(Guid researchID)
        {
            ResearchResult researchInfo = null;

            string folderNameToLoad = null;
            if (existingFolderNames != null && existingFolderNames.Keys.Contains(researchID))
                folderNameToLoad = existingFolderNames[researchID];
            else
                folderNameToLoad = FileNameByGuid(researchID);

            if (folderNameToLoad != null)
            {
                foreach (string folderName in Directory.GetDirectories(storageStr, "*", SearchOption.TopDirectoryOnly))
                {
                    researchInfo = new ResearchResult();
                    LoadGeneralInfo(folderName, researchInfo);
                    LoadEnsembleResults(folderName, researchInfo);

                    existingFolderNames.Add(researchInfo.ResearchID, folderName);
                }
            }

            return researchInfo;
        }

        public override ResearchResult Load(string name)
        {
            ResearchResult researchInfo = null;

            if (name != null)
            {
                foreach (string folderName in Directory.GetDirectories(storageStr, "*", SearchOption.TopDirectoryOnly))
                {
                    researchInfo = new ResearchResult();
                    LoadGeneralInfo(folderName, researchInfo);
                    LoadEnsembleResults(folderName, researchInfo);

                    existingFolderNames.Add(researchInfo.ResearchID, folderName);
                }
            }

            return researchInfo;
        }

        #region Utilities

        #region Save

        private void SaveGeneralInfo(string dirName,
            Guid researchID,
            string researchName,
            ResearchType rType,
            ModelType mType,
            int realizationCount,
            Int32 size,
            Double edges,
            Dictionary<ResearchParameter, Object> rp,
            Dictionary<GenerationParameter, Object> gp)
        {
            string fileName = dirName + Path.DirectorySeparatorChar + "general";
            using (StreamWriter w = new StreamWriter(fileName + ".txt"))
            {
                w.WriteLine("Research Info");
                w.WriteLine("ResearchID - " + researchID);
                w.WriteLine("ResearchName - " + researchName);
                w.WriteLine("ResearchType - " + rType);
                w.WriteLine("ModelType - " + mType);
                w.WriteLine("RealizationCount - " + realizationCount);
                w.WriteLine("Date - " + DateTime.Now);
                w.WriteLine("Size - " + size);
                w.WriteLine("Edges - " + edges);
            }

            SaveResearchParameters(fileName, rp);
            SaveGenerationParameters(fileName, gp);
        }

        private void SaveResearchParameters(string fileName, Dictionary<ResearchParameter, Object> p)
        {
            if (p.Count != 0)
            {
                using (StreamWriter w = new StreamWriter(fileName + ".txt", true))
                {
                    w.WriteLine();
                    w.WriteLine("Research Parameters");
                    foreach (ResearchParameter rp in p.Keys)
                    {
                        if (p[rp] != null)
                        {
                            w.WriteLine(rp.ToString() + " " + p[rp].ToString());
                        }
                    }
                }
            }
        }

        private void SaveGenerationParameters(string fileName, Dictionary<GenerationParameter, Object> p)
        {
            using (StreamWriter w = new StreamWriter(fileName + ".txt", true))
            {
                w.WriteLine();
                w.WriteLine("Generation Parameters");
                foreach (GenerationParameter gp in p.Keys)
                {
                    if (p[gp] == null)
                        continue;
                    if (gp == GenerationParameter.AdjacencyMatrix)
                    {
                        w.WriteLine("FileName - " + ((MatrixPath)p[gp]).Path);
                    }
                    else
                    {
                        w.WriteLine(gp.ToString() + " " + p[gp].ToString());
                    }
                }
            }
        }

        private void SaveEnsembleResult(string dirName, string researchName, EnsembleResult e, int id)
        {
            foreach (AnalyzeOption opt in e.Result.Keys)
            {
                AnalyzeOptionInfo info = ((AnalyzeOptionInfo[])opt.GetType().GetField(opt.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false))[0];
                OptionType optionType = info.OptionType;

                switch (optionType)
                {
                    case OptionType.Global:
                        SaveToGlobalFile(dirName, id, info, e.Result[opt]);
                        break;
                    case OptionType.ValueList:
                    case OptionType.Centrality:
                        SaveValueListFile(dirName, id, info, e.Result[opt]);
                        break;
                    case OptionType.Distribution:
                        SaveDistributionFile(dirName, id, info, e.Result[opt]);
                        break;
                    case OptionType.Trajectory:
                        SaveTrajectoryFile(dirName, researchName, id, info, e.Result[opt]);
                        break;
                    default:
                        break;
                }
            }
        }

        private void SaveToGlobalFile(string dirName, int id, AnalyzeOptionInfo info, Object value)
        {
            string fileName = dirName + Path.DirectorySeparatorChar + id.ToString() + "_globals";
            using (StreamWriter w = new StreamWriter(fileName + ".txt", true))
            {
                w.WriteLine(info.FullName + " " + value.ToString());
            }
        }

        private void SaveValueListFile(string dirName, int id, AnalyzeOptionInfo info, Object value)
        {
            string fileName = dirName + Path.DirectorySeparatorChar + id.ToString() + "_" + info.FullName;
            using (StreamWriter w = new StreamWriter(fileName + ".txt"))
            {
                Debug.Assert(value is List<Double>);
                List<Double> l = value as List<Double>;
                foreach (Double d in l)
                {
                    w.WriteLine(d);
                }
            }
        }

        private void SaveCentralityFile(string dirName, int id, AnalyzeOptionInfo info, Object value)
        {
            throw new NotImplementedException();
        }

        private void SaveDistributionFile(string dirName, int id, AnalyzeOptionInfo info, Object value)
        {
            string fileName = dirName + Path.DirectorySeparatorChar + id.ToString() + "_" + info.FullName;
            using (StreamWriter w = new StreamWriter(fileName + ".txt"))
            {
                Debug.Assert(value is SortedDictionary<Double, Double>);
                SortedDictionary<Double, Double> l = value as SortedDictionary<Double, Double>;
                foreach (KeyValuePair<Double, Double> d in l)
                {
                    w.WriteLine(d.Key + " " + d.Value);
                }
            }
        }

        private void SaveTrajectoryFile(string dirName, string researchName, int id, AnalyzeOptionInfo info, Object value)
        {
            string fileName = dirName + Path.DirectorySeparatorChar + id.ToString() + "_" + info.FullName;
            using (StreamWriter w = new StreamWriter(fileName + ".txt"))
            {
                Debug.Assert(value is SortedDictionary<Double, Double>);
                SortedDictionary<Double, Double> l = value as SortedDictionary<Double, Double>;
                foreach (KeyValuePair<Double, Double> d in l)
                    w.WriteLine(d.Key + " " + d.Value);
            }                
        }

        #endregion

        #region Load

        private void LoadGeneralInfo(String folderName, ResearchResult r)
        {
            using (StreamReader rd = new StreamReader(folderName + "\\general.txt"))
            {
                rd.ReadLine();
                r.ResearchID = new Guid(rd.ReadLine().Substring(13));
                r.ResearchName = rd.ReadLine().Substring(15);
                r.ResearchType = (ResearchType)Enum.Parse(typeof(ResearchType), rd.ReadLine().Substring(15));
                r.ModelType = (ModelType)Enum.Parse(typeof(ModelType), rd.ReadLine().Substring(12));
                r.RealizationCount = Int32.Parse(rd.ReadLine().Substring(19));
                r.Date = DateTime.Parse(rd.ReadLine().Substring(7));
                r.Size = Int32.Parse(rd.ReadLine().Substring(7));
                r.Edges = Double.Parse(rd.ReadLine().Substring(8), CultureInfo.InvariantCulture);

                rd.ReadLine();

                String str = null;
                while ((str = rd.ReadLine()) != "Generation Parameters")
                {
                    String[] split = str.Split(' ');
                    ResearchParameter rp = (ResearchParameter)Enum.Parse(typeof(ResearchParameter), split[0]);

                    ResearchParameterInfo rpInfo = (ResearchParameterInfo)(rp.GetType().GetField(rp.ToString()).GetCustomAttributes(typeof(ResearchParameterInfo), false)[0]);
                    if (rpInfo.Type.Equals(typeof(Int32)))
                        r.ResearchParameterValues.Add(rp, Int32.Parse(split[1]));
                    else if (rpInfo.Type.Equals(typeof(Double)))
                        r.ResearchParameterValues.Add(rp, Double.Parse(split[1], CultureInfo.InvariantCulture));
                    else if (rpInfo.Type.Equals(typeof(Boolean)))
                        r.ResearchParameterValues.Add(rp, Boolean.Parse(split[1]));
                    else if (rpInfo.Type.Equals(typeof(ResearchType)))
                        r.ResearchParameterValues.Add(rp, split[1]);
                    else if (rpInfo.Type.Equals(typeof(MatrixPath)))
                    {
                        MatrixPath mp = new MatrixPath();
                        mp.Path = split[1];
                        r.ResearchParameterValues.Add(rp, mp);
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                }
                while ((str = rd.ReadLine()) != null)
                {
                    String[] split = str.Split(' ');
                    GenerationParameter gp;
                    if (split[0] == "FileName")
                        gp = GenerationParameter.AdjacencyMatrix;
                    else
                        gp = (GenerationParameter)Enum.Parse(typeof(GenerationParameter), split[0]);

                    GenerationParameterInfo gpInfo = (GenerationParameterInfo)(gp.GetType().GetField(gp.ToString()).GetCustomAttributes(typeof(GenerationParameterInfo), false)[0]);
                    if (gpInfo.Type.Equals(typeof(Int32)))
                        r.GenerationParameterValues.Add(gp, Int32.Parse(split[1]));
                    else if (gpInfo.Type.Equals(typeof(Double)))
                        r.GenerationParameterValues.Add(gp, Double.Parse(split[1], CultureInfo.InvariantCulture));
                    else if (gpInfo.Type.Equals(typeof(Boolean)))
                        r.GenerationParameterValues.Add(gp, Boolean.Parse(split[1]));
                    else if (gpInfo.Type.Equals(typeof(MatrixPath)))
                    {
                        MatrixPath mp = new MatrixPath();
                        mp.Path = split[1];
                        r.GenerationParameterValues.Add(gp, mp);
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                }
            }
        }

        private void LoadEnsembleResults(String folderName, ResearchResult r)
        {
            EnsembleResult e = new EnsembleResult(r.Size);
            e.EdgesCount = r.Edges;
            e.Result = new Dictionary<AnalyzeOption, Object>();
            foreach (String fileName in Directory.GetFiles(folderName))
            {                
                /*reader.Read();
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    AnalyzeOption opt = (AnalyzeOption)Enum.Parse(typeof(AnalyzeOption), reader.Name);
                    AnalyzeOptionInfo optInfo = (AnalyzeOptionInfo)(opt.GetType().GetField(opt.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false)[0]);
                    switch (optInfo.OptionType)
                    {
                        case OptionType.Global:
                            e.Result.Add(opt, reader.ReadElementContentAsDouble());
                            break;
                        case OptionType.ValueList:
                        case OptionType.Centrality:
                            e.Result.Add(opt, LoadValueList());
                            reader.Read();
                            break;
                        case OptionType.Distribution:
                        case OptionType.Trajectory:
                            e.Result.Add(opt, LoadDistribution());
                            reader.Read();
                            break;
                        default:
                            break;
                    }
                }*/                
            }
            r.EnsembleResults.Add(e);
        }

        #endregion

        private string FileNameByGuid(Guid id)
        {
            foreach (string folderName in Directory.GetDirectories(storageStr, "*", SearchOption.TopDirectoryOnly))
            {
                String generalFileName = folderName + "\\" + "general.txt";
                if (!File.Exists(generalFileName))
                    continue;                
                using (StreamReader r = new StreamReader(generalFileName))
                {
                    try
                    {
                        r.ReadLine();
                        if (id == Guid.Parse(r.ReadLine().Substring(13)))
                        {
                            return folderName;
                        }
                    }
                    catch (SystemException)
                    {
                        continue;
                    }
                }
            }

            return null;
        }

        #endregion
    }
}