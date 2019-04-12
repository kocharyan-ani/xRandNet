﻿using System;

using Core.Attributes;

namespace Core.Enumerations
{
    /// <summary>
    /// Enumaration, used for indicating the result storage type for Research.
    /// Uses Attribute StorageTypeInfo for storing metadata about every storage.
    /// </summary>
    public enum StorageType
    {
        [StorageTypeInfo("XML file with defined LINK structure.", 
            "Storage.XMLResultStorage, Storage")]
        XMLStorage,

        // depracate sql storage
        /*[StorageTypeInfo("SQL DB with defined LINK schema.", 
            "Storage.SQLResultStorage, Storage")]
        SQLStorage,*/

        [StorageTypeInfo("Excel file with defined LINK structure.",
            "Storage.ExcelResultStorage, Storage")]
        ExcelStorage,

        [StorageTypeInfo("Folder with txt files with defined LINK structure.",
            "Storage.TXTResultStorage, Storage")]
        TXTStorage
    }
}
