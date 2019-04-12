using System;
using System.Collections.Generic;

namespace Core.Attributes
{
    /// <summary>
    /// Enumeration used for inticating the logical type of statistical property.
    /// </summary>
    public enum OptionType
    {
        Global,
        ValueList,
        Distribution,
        Trajectory,
        Centrality
    }

    /// <summary>
    /// Mapping which shows the type of data for each OptionType.
    /// </summary>
    public static class OptionTypeToTypeMapping
    {
        public static Dictionary<OptionType, Type> Mapping { get; private set; }

        static OptionTypeToTypeMapping()
        {
            Mapping = new Dictionary<OptionType, Type>
            {
                { OptionType.Global, typeof(Double) },
                { OptionType.ValueList, typeof(List<Double>) },
                { OptionType.Centrality, typeof(List<Double>) },
                { OptionType.Distribution, typeof(SortedDictionary<Double, Double>) },
                { OptionType.Trajectory, typeof(SortedDictionary<Double, Double>) }
            };
        }
    };

    /// <summary>
    /// Attribute for AnalyzeOption (enum).
    /// FullName - user-friendly name for an Analyze Option.
    /// Description - extended information about an Analyze Option.
    /// OptionType - the logical type of an Analyze Option.
    /// XAxisName - name of xAxis for graphic (xRandNetStat).
    /// YAxisName - name of yAxis for graphic (xRandNetStat).
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AnalyzeOptionInfo : Attribute
    {
        public AnalyzeOptionInfo(String fullName, 
            String description,
            OptionType optionType)
        {
            FullName = fullName;
            Description = description;
            OptionType = optionType;
        }

        public AnalyzeOptionInfo(String fullName,
            String description,
            OptionType optionType,
            String xAxisName,
            String yAxisName)
            : this(fullName, description, optionType)
        {
            XAxisName = xAxisName;
            YAxisName = yAxisName;
        }

        public String FullName { get; private set; }
        public String Description { get; private set; }
        public OptionType OptionType { get; private set; }
        public String XAxisName { get; private set; }
        public String YAxisName { get; private set; }
    }
}
