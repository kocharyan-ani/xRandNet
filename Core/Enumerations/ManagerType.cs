using System;

using Core.Attributes;

namespace Core.Enumerations
{
    /// <summary>
    /// Enumaration, used for indicating the manager type for Research.
    /// Uses Attribute ManagerTypeInfo for storing metadata about every model.
    /// </summary>
    public enum ManagerType
    {
        [ManagerTypeInfo("Local manager",
            "Local manager distributes realizations in threads.",
            "Manager.LocalEnsembleManager, Manager")]
        Local,

        [ManagerTypeInfo("WCF distributed manager",
            "Local manager distributes realizations in computers in local network.",
            "Manager.WCFDistributedEnsembleManager, Manager")]
        WCFDistributed
    }
}
