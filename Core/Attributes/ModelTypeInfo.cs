using System;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for ModelType (enum).
    /// FullName - user-friendly name for Model.
    /// Description - extended information about a Model.
    /// Implementation - the name of type, which implements a Model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ModelTypeInfo : Attribute
    {
        public ModelTypeInfo(String fullName, String description, String implementation)
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
