using System;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for ManagerType (enum).
    /// FullName - user-friendly name for Manager.
    /// Description - extended information about a Manager.
    /// Implementation - the name of type, which implements a Manager.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ManagerTypeInfo : Attribute
    {
        public ManagerTypeInfo(String fullName, String description, String implementation)
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
