using System;
using System.Configuration;
using System.IO;
using System.Web;
using Core.Enumerations;
using Core.Exceptions;

namespace Core.Settings
{
    /// <summary>
    /// RandNetStat application settings organization and manipulation interface.
    /// </summary>
    public static class RandNetStatSettings
    {
        static private String defaultDirectory =
            Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\xRandNet";

        static private Configuration config;

        static private StorageType storageType = StorageType.XMLStorage;
        static private String xmlStorageDirectory;
        static private String txtStorageDirectory;
        static private String excelStorageDirectory;

        static RandNetStatSettings()
        {
            if (HttpContext.Current != null)
            {
                config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            }
            else
            {
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }

            try
            {
                storageType = (StorageType)Enum.Parse(typeof(StorageType),
                    config.AppSettings.Settings["StorageType"].Value);
                xmlStorageDirectory = config.AppSettings.Settings["XMLStorageDirectory"].Value;
                txtStorageDirectory = config.AppSettings.Settings["TXTStorageDirectory"].Value;
                excelStorageDirectory = config.AppSettings.Settings["ExcelStorageDirectory"].Value;
            }
            catch
            {
                throw new CoreException("The structure of Configuration file is not correct.");
            }
        }

        static public StorageType StorageType
        {
            get
            {
                if (config.AppSettings.Settings["StorageType"].Value != storageType.ToString())
                    storageType = (StorageType)Enum.Parse(typeof(StorageType),
                        config.AppSettings.Settings["StorageType"].Value);
                return storageType;
            }
            set
            {
                storageType = value;
                config.AppSettings.Settings["StorageType"].Value = storageType.ToString();
            }
        }

        static public String XMLStorageDirectory
        {
            get
            {
                return (xmlStorageDirectory == "") ? defaultDirectory + "\\Results" : xmlStorageDirectory;
            }
            set
            {
                if (value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    xmlStorageDirectory = value;
                }
                else
                {
                    xmlStorageDirectory = value + Path.DirectorySeparatorChar;
                }

                if (Directory.Exists(xmlStorageDirectory) == false)
                {
                    Directory.CreateDirectory(xmlStorageDirectory);
                }

                config.AppSettings.Settings["XMLStorageDirectory"].Value = xmlStorageDirectory;
            }
        }

        static public String TXTStorageDirectory
        {
            get
            {
                return (txtStorageDirectory == "") ? defaultDirectory + "\\Results" : txtStorageDirectory;
            }
            set
            {
                if (value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    txtStorageDirectory = value;
                }
                else
                {
                    txtStorageDirectory = value + Path.DirectorySeparatorChar;
                }

                if (Directory.Exists(txtStorageDirectory) == false)
                {
                    Directory.CreateDirectory(txtStorageDirectory);
                }

                config.AppSettings.Settings["TXTStorageDirectory"].Value = txtStorageDirectory;
            }
        }

        static public String ExcelStorageDirectory
        {
            get
            {
                return (excelStorageDirectory == "") ? defaultDirectory + "\\Results" : excelStorageDirectory;
            }
            set
            {
                if (value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    excelStorageDirectory = value;
                }
                else
                {
                    excelStorageDirectory = value + Path.DirectorySeparatorChar;
                }

                if (Directory.Exists(excelStorageDirectory) == false)
                {
                    Directory.CreateDirectory(excelStorageDirectory);
                }

                config.AppSettings.Settings["ExcelStorageDirectory"].Value = excelStorageDirectory;
            }
        }

        /// <summary>
        /// Refreshes app.config file content.
        /// </summary>
        static public void Refresh()
        {
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
