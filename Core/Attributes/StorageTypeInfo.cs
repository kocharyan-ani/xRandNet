using System;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for StorageType (enum).
    /// Description - extended information about a Storage.
    /// Implementation - the name of type, which implements a Storage.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class StorageTypeInfo : Attribute
    {
        public StorageTypeInfo(String description, String implementation)
        {
            Description = description;
            Implementation = implementation;
        }

        public String Description { get; private set; }
        public String Implementation { get; private set; }
    }
}
