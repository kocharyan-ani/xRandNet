using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using Core.Enumerations;
using Core.Attributes;

namespace Core.Result
{
    /// <summary>
    /// Represents the result of analyze for single ensemble.
    /// </summary>
    public class EnsembleResult
    {
        public Int32 NetworkSize { get; set; }
        public Double EdgesCount { get; set; }
        public Dictionary<AnalyzeOption, Object> Result { get; set; }

        private int realizationCount = 0;

        public EnsembleResult(Int32 networkSize)
        {
            NetworkSize = networkSize;
            EdgesCount = 0;
            Result = new Dictionary<AnalyzeOption, Object>();
        }

        /// <summary>
        /// Clears huge data in Result.
        /// </summary>
        public void Clear()
        {
            foreach (AnalyzeOption o in Result.Keys)
            {
                AnalyzeOptionInfo[] info = (AnalyzeOptionInfo[])o.GetType().GetField(o.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false);
                OptionType ot = info[0].OptionType;

                switch (ot)
                {
                    case OptionType.ValueList:
                    case OptionType.Centrality:
                        Debug.Assert(Result[o] is List<Double>);
                        (Result[o] as List<Double>).Clear();
                        break;
                    case OptionType.Distribution:
                    case OptionType.Trajectory:
                        Debug.Assert(Result[o] is SortedDictionary<Double, Double>);
                        (Result[o] as SortedDictionary<Double, Double>).Clear();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Averages specified results by realization count.
        /// </summary>
        /// <param name="results">List of results for realizations.</param>
        /// <returns>Ensemble result, which represents avereged values of realizations results.</returns>
        public static EnsembleResult AverageResults(List<RealizationResult> results)
        {
            Debug.Assert(results.Count != 0);

            EnsembleResult r = new EnsembleResult(results[0].NetworkSize);

            double rCount = results.Count;
            foreach (RealizationResult res in results)
            {
                r.EdgesCount += res.EdgesCount;
            }
            r.EdgesCount /= rCount;
            Math.Round(r.EdgesCount, 4);

            foreach (AnalyzeOption option in results[0].Result.Keys)
            {
                AnalyzeOptionInfo[] info = (AnalyzeOptionInfo[])option.GetType().GetField(option.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false);
                OptionType ot = info[0].OptionType;

                #region EIGENVALUES
                if (option == AnalyzeOption.EigenValues || option == AnalyzeOption.LaplacianEigenValues)
                {
                    FillEigenValuesResults(option, results, r);
                    continue;
                }
                else if (option == AnalyzeOption.EigenDistanceDistribution)
                {
                    FillEigenDistanceDistributionResults(option, results, r);
                    continue;
                }
                #endregion

                Debug.Assert(option != AnalyzeOption.EigenValues && option != AnalyzeOption.EigenDistanceDistribution);
                switch (ot)
                {
                    case OptionType.Global:
                        FillDoubleResults(option, results, r);
                        break;
                    case OptionType.ValueList:
                    case OptionType.Centrality:
                        FillListResults(option, results, r);
                        break;
                    case OptionType.Distribution:
                        FillDictionaryResults(option, results, r);
                        break;
                    case OptionType.Trajectory:
                        FillTrajectoryResults(option, results, r);
                        break;
                    default: { break; }
                }
            }

            foreach (RealizationResult res in results)
            {
                res.Clear();
            }

            return r;
        }

        #region Utilities

        private static void FillEigenValuesResults(AnalyzeOption option, List<RealizationResult> results, EnsembleResult r)
        {
            Debug.Assert(option == AnalyzeOption.EigenValues || option == AnalyzeOption.LaplacianEigenValues);
            List<Double> temp = new List<Double>();
            foreach (RealizationResult res in results)
            {
                Debug.Assert(res.Result[option] is List<Double>);
                List<Double> e = res.Result[option] as List<Double>;
                foreach (Double k in e)
                    temp.Add(k);
            }
            r.Result.Add(option, temp);
        }

        private static void FillEigenDistanceDistributionResults(AnalyzeOption option, List<RealizationResult> results, EnsembleResult r)
        {
            Debug.Assert(option == AnalyzeOption.EigenDistanceDistribution);
            SortedDictionary<Double, Double> temp = new SortedDictionary<Double, Double>();
            foreach (RealizationResult res in results)
            {
                Debug.Assert(res.Result[option] is SortedDictionary<Double, Double>);
                SortedDictionary<Double, Double> d = res.Result[option] as SortedDictionary<Double, Double>;
                foreach (KeyValuePair<double, double> k in d)
                {
                    if (temp.ContainsKey(k.Key))
                        temp[k.Key] += k.Value;
                    else
                        temp.Add(k.Key, k.Value);
                }
            }
            r.Result.Add(option, temp);
        }

        private static void FillDoubleResults(AnalyzeOption option, List<RealizationResult> results, EnsembleResult r)
        {
            double temp = 0;
            foreach (RealizationResult res in results)
            {
                temp += (double)(res.Result[option]) / results.Count;
            }
            r.Result.Add(option, double.IsNaN(temp) ? 0 : Math.Round(temp, 4));
        }

        private static void FillListResults(AnalyzeOption option, List<RealizationResult> results, EnsembleResult r)
        {
            List<Double> temp = new List<double>();
            foreach (RealizationResult res in results)
            {
                Debug.Assert(res.Result[option] is List<Double>);
                List<Double> l = res.Result[option] as List<Double>;
                for (int j = 0; j < l.Count; ++j)
                {
                    if (j < temp.Count())
                        temp[j] += Math.Round(l[j] / results.Count, 4);
                    else
                        temp.Add(Math.Round(l[j] / results.Count, 4));
                }
            }
            r.Result.Add(option, temp);
        }

        private static void FillDictionaryResults(AnalyzeOption option, List<RealizationResult> results, EnsembleResult r)
        {
            SortedDictionary<Double, Double> temp = new SortedDictionary<Double, Double>();
            foreach (RealizationResult res in results)
            {
                Debug.Assert(res.Result[option] is SortedDictionary<Double, Double>);
                SortedDictionary<Double, Double> d = res.Result[option] as SortedDictionary<Double, Double>;
                foreach (KeyValuePair<double, double> k in d)
                {
                    if (temp.ContainsKey(k.Key))
                        temp[k.Key] += Math.Round(k.Value / results.Count, 4);
                    else
                        temp.Add(k.Key, Math.Round(k.Value / results.Count, 4));
                }
            }
            r.Result.Add(option, temp);
        }

        private static void FillTrajectoryResults(AnalyzeOption option, List<RealizationResult> results, EnsembleResult r)
        {
            SortedDictionary<Double, Double> temp = new SortedDictionary<Double, Double>();
            foreach (RealizationResult res in results)
            {
                Debug.Assert(res.Result[option] is SortedDictionary<Double, Double>);
                SortedDictionary<Double, Double> d = res.Result[option] as SortedDictionary<Double, Double>;
                foreach (KeyValuePair<double, double> k in d)
                {
                    if (temp.ContainsKey(k.Key))
                        temp[k.Key] += k.Value / results.Count;
                    else
                        temp.Add(k.Key, k.Value / results.Count);
                }
            }
            r.Result.Add(option, temp);
        }

        private Double CalculateNextAverageValue(Double current, Double next)
        {
            return (current * realizationCount + next) / (realizationCount + 1);
        }

        #endregion
    }
}
