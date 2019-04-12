using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

using Core.Exceptions;
using Core.Settings;

namespace Core.Utility
{
    /// <summary>
    /// Specialized functions for file system operations.
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// Reads matrix, branches (if exists) and actives (if exists) from specified file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>Matrix/neighbourship, branches (if exists) and actives (if exists).</returns>
        /// <throws>CoreException, MatrixFormatException, ActiveStatesFormatException.</throws>
        public static NetworkInfoToRead Read(String fileName, int size)
        {
            if ((File.GetAttributes(fileName) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                throw new CoreException("File should be specified.");
            }
            else
            {
                Debug.Assert(Path.GetExtension(fileName) == ".txt");
                NetworkInfoToRead r;
                if (IsMatrixFile(fileName))
                {
                    r = new MatrixInfoToRead();
                    (r as MatrixInfoToRead).Matrix = ReadMatrix(fileName);
                }
                else
                {
                    r = new NeighbourshipInfoToRead();
                    NeighbourshipInfoToRead nr = r as NeighbourshipInfoToRead;
                    nr.Neighbours = ReadNeighbourship(fileName);
                    nr.Size = size == 0 ? nr.Neighbours.Max() + 1 : size;                    
                }

                r.FileName = fileName;
                r.Branches = ReadBranches(fileName.Substring(0, fileName.Length - 4) + ".branches");
                r.ActiveStates = ReadActiveStates(fileName.Substring(0, fileName.Length - 4) + ".actives", size);

                return r;
            }
        }

        /// <summary>
        /// Writes matrix, branches (if exists) and active states (if exists) to specified file.
        /// </summary>
        /// <param name="matrixInfo">Matrix, branches (if exists) and active states (if exists).</param>
        /// <param name="filePath">File path.</param>
        public static void Write(String directoryPath, MatrixInfoToWrite matrixInfo, String filePath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            WriteMatrix(matrixInfo.Matrix, filePath);
            if (matrixInfo.Branches != null)
                WriteBranches(matrixInfo.Branches, filePath);
            if (matrixInfo.ActiveStates != null)
                WriteActiveStates(matrixInfo.ActiveStates, filePath);
        }

        /// <summary>
        /// Writes neighbourship info, branches (if exists) and active states (if exists) to specified file.
        /// </summary>
        /// <param name="neighbourshipInfo">Neighbourship, branches (if exists) and active states (if exists).</param>
        /// <param name="filePath">File path.</param>
        public static void Write(String directoryPath, NeighbourshipInfoToWrite neighbourshipInfo, String filePath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            WriteNeighbourship(neighbourshipInfo.Neighbourship, filePath);
            if (neighbourshipInfo.Branches != null)
                WriteBranches(neighbourshipInfo.Branches, filePath);
            if (neighbourshipInfo.ActiveStates != null)
                WriteActiveStates(neighbourshipInfo.ActiveStates, filePath);
        }

        /// <summary>
        /// Reads subnetwork matrixes (in *.sm files).
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="vertices">Array of vertex numbers.</param>
        /// <returns>True, if succeed.</returns>
        public static bool ReadSubnetworkMatrix(String fileName, out int[] vertices)
        {
            vertices = new int[0];
            if (Path.GetExtension(fileName) != ".sm")
                return false;

            String content = File.ReadAllText(fileName);
            String[] saparators = { " ", "\t", "\r", "\n" };
            String[] split = content.Split(saparators, StringSplitOptions.RemoveEmptyEntries);
            vertices = new int[split.Count()];
            for (int i = 0; i < split.Count(); ++i)
            {
                int v;
                if (!int.TryParse(split[i], out v))
                    return false;
                vertices[i] = v;
            }

            return true;
        }

        private static bool IsMatrixFile(String fileName)
        {
            using (StreamReader streamreader = new StreamReader(fileName, System.Text.Encoding.Default))
            {
                char[] buffer = new char[10];
                String[] saparators = { " " };
                streamreader.ReadBlock(buffer, 0, 10);
                String content = new String(buffer);
                String[] split = content.Split(saparators, StringSplitOptions.RemoveEmptyEntries);
                if (split.Count() <= 2)
                    return false;
                else
                {
                    foreach (String str in split)
                    {
                        if (str == "0" || str == "1")
                            continue;
                        else
                            return false;
                    }
                }
            }
            return true;
        }

        private static BitArray[] ReadMatrix(String filePath)
        {
            String[] saparators = { " " };

            // retrieving size of matrix
            int size = 0;
            using (StreamReader streamreader = new StreamReader(filePath, System.Text.Encoding.Default))
            {
                Debug.Assert(streamreader.ReadLine() != null);
                size = (streamreader.ReadLine().Split(saparators, StringSplitOptions.None)).Length - 1;
            }

            // reading matrix
            BitArray[] matrix = new BitArray[size];
            using (StreamReader streamreader = new StreamReader(filePath, System.Text.Encoding.Default))
            {
                String contents;
                int i = 0;
                while ((contents = streamreader.ReadLine()) != null)
                {
                    matrix[i] = new BitArray(size);
                    String[] split = contents.Split(saparators, StringSplitOptions.None);
                    for (int j = 0; j < split.Length - 1; ++j)
                    {
                        if (split[j].Equals("0"))
                            matrix[i][j] = false;
                        else if (split[j].Equals("1"))
                            matrix[i][j] = true;
                        else throw new MatrixFormatException();
                    }
                    ++i;
                }
            }

            return matrix;
        }

        private static List<int> ReadNeighbourship(String filePath)
        {
            List<int> neighbours = new List<int>();
            using (StreamReader streamreader = new StreamReader(filePath, System.Text.Encoding.Default))
            {
                String[] saparators = { " ", ",", ";" };
                String contents;
                while ((contents = streamreader.ReadLine()) != null)
                {
                    String[] split = contents.Split(saparators, StringSplitOptions.None);

                    try
                    {
                        int i = Convert.ToInt32(split[0]), j = Convert.ToInt32(split[1]);
                        neighbours.Add(i);
                        neighbours.Add(j);
                    }
                    catch (SystemException)
                    {
                        throw new MatrixFormatException();
                    }
                }
            }
            if (neighbours.Count() % 2 != 0)
                throw new MatrixFormatException();

            return neighbours;
        }

        private static List<List<int>> ReadBranches(String filePath)
        {
            Debug.Assert(Path.GetExtension(filePath) == ".branches");
            if (File.Exists(filePath))
            {
                List<List<int>> branches = new List<List<int>>();
                try
                {
                    using (StreamReader streamreader =
                        new StreamReader(filePath, System.Text.Encoding.Default))
                    {
                        String contents;
                        while ((contents = streamreader.ReadLine()) != null)
                        {
                            String[] split = System.Text.RegularExpressions.Regex.Split(contents,
                                "\\s+", System.Text.RegularExpressions.RegexOptions.None);
                            List<int> tmp = new List<int>();
                            foreach (String s in split)
                            {
                                if (s != "")
                                    tmp.Add(int.Parse(s));
                            }
                            branches.Add(tmp);
                        }
                    }
                }
                catch (SystemException)
                {
                    throw new BranchesFormatException();
                }

                return branches;
            }
            else return null;
        }

        private static BitArray ReadActiveStates(String filePath, int size)
        {            
            Debug.Assert(Path.GetExtension(filePath) == ".actives");
            if (File.Exists(filePath))
            {
                if (size == 0)
                    throw new ActiveStatesFormatException();
                BitArray activeStates = new BitArray(size);
                try
                {
                    using (StreamReader streamreader =
                        new StreamReader(filePath, System.Text.Encoding.Default))
                    {
                        String contents;
                        while ((contents = streamreader.ReadLine()) != null)
                        {
                            String[] split = System.Text.RegularExpressions.Regex.Split(contents,
                                "\\s+", System.Text.RegularExpressions.RegexOptions.None);
                            if (split.Count() != 1 && split.Count() != 2)
                                throw new ActiveStatesFormatException();
                            if (split.Count() == 1)
                            {
                                int i = int.Parse(split[0]);
                                if(i >= size)
                                    throw new ActiveStatesFormatException();
                                activeStates[i] = true;
                            } else
                            {
                                int i = int.Parse(split[0]);
                                int j = int.Parse(split[1]);
                                if (i >= size || j >= size || i >= j)
                                    throw new ActiveStatesFormatException();
                                for (int l = i; l <= j; ++l)
                                    activeStates[l] = true;
                            }
                        }
                    }
                }
                catch (SystemException)
                {
                    throw new ActiveStatesFormatException();
                }

                return activeStates;
            }
            else return null;
        }

        private static void WriteMatrix(BitArray[] matrix, String filePath)
        {
            using (StreamWriter file = new StreamWriter(filePath + ".txt"))
            {
                int size = matrix.Length;
                for (int i = 0; i < size; ++i)
                {
                    for (int j = 0; j < size; ++j)
                    {
                        if (matrix[i][j])
                        {
                            file.Write(1 + " ");
                        }
                        else
                        {
                            file.Write(0 + " ");
                        }
                    }
                    file.WriteLine("");
                }
            }
        }

        private static void WriteNeighbourship(List<KeyValuePair<int, int>> neighbourship, String filePath)
        {
            using (StreamWriter file = new StreamWriter(filePath + ".txt"))
            {
                foreach (KeyValuePair<int, int> p in neighbourship)
                    file.WriteLine(p.Key.ToString() + " " + p.Value.ToString());
            }
        }

        private static void WriteBranches(List<List<int>> branches, String filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath + ".branches"))
            {
                for (int i = 0; i < branches.Count; i++)
                {
                    for (int k = 0; k < branches[i].Count; k++)
                    {
                        writer.Write(branches[i][k]);
                        writer.Write(" ");
                    }
                    writer.WriteLine();
                }
            }
        }

        private static void WriteActiveStates(BitArray activeStates, String filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath + ".actives"))
            {
                for(int i = 0; i < activeStates.Length - 1; ++i)
                {
                    if (!activeStates[i])
                    {
                        if (i == activeStates.Length - 2 && activeStates[activeStates.Length - 1])
                        {
                            writer.Write(i + 1);
                        }
                        continue;
                    }
                    if (activeStates[i] && !activeStates[i + 1])
                    {
                        writer.WriteLine(i);
                        ++i;
                        continue;
                    }
                    int j = i + 1;
                    while (j < activeStates.Length && activeStates[j])
                        ++j;
                    writer.Write(i);
                    writer.Write(" ");
                    writer.WriteLine(j - 1);
                    i = j;            
                }
            }
        }
    }
}