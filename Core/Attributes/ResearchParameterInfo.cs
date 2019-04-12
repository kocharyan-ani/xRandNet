using System;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for ResearchParameter (enum).
    /// FullName - user-friendly name for a Research Parameter.
    /// Description - extended information about a Research Parameter.
    /// Type - type of a Research Parameter.
    /// DefaultValue - String-representation of default value for a Research Parameter (for GUI).
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ResearchParameterInfo : Attribute
    {
        public ResearchParameterInfo(String fullName, String description, Type type, String defaultValue)
        {
            FullName = fullName;
            Description = description;
            Type = type;
            DefaultValue = defaultValue;
        }

        public String FullName { get; private set; }
        public String Description { get; private set; }
        public Type Type { get; private set; }
        public String DefaultValue { get; private set; }
    }
}
