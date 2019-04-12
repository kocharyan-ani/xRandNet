using System;
using System.Collections.Generic;
using System.Diagnostics;

using Core.Enumerations;
using Core.Attributes;

namespace Core.Result
{
    /// <summary>
    /// Represents the result of analyze for single realization.
    /// </summary>
    public class RealizationResult
    {
        public Int32 NetworkSize { get; set; }
        public Double EdgesCount { get; set; }
        public Dictionary<AnalyzeOption, Object> Result { get; set; }

        public RealizationResult()
        {
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
    }
}
