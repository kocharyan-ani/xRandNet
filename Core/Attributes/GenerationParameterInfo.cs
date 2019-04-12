using System;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for GenerationParameter (enum).
    /// FullName - user-friendly name for a Generation Parameter.
    /// Description - extended information about a Generation Parameter.
    /// Type - type of a Generation Parameter.
    /// DefaultValue - String-representation of default value for a Generation Parameter (for GUI).
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GenerationParameterInfo : Attribute
    {
        public GenerationParameterInfo(String fullName, String description, Type type, String defaultValue)
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
