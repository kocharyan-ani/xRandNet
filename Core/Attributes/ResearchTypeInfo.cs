using System;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for ResearchType (enum).
    /// FullName - user-friendly name for Research.
    /// Description - extended information about a Research.
    /// Implementation - the name of type, which implements a Research.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ResearchTypeInfo : Attribute
    {
        public ResearchTypeInfo(String fullName, String description, String implementation)
        {
            FullName = fullName;
            Description = description;
            Implementation = implementation;
        }

        public String FullName { get; private set; }
        public String Description { get; private set; }
        public String Implementation { get; private set; }
    }
}
