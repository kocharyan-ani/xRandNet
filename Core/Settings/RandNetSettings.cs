using System;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Core.Exceptions;
using Core.Enumerations;
using Core.Utility;

namespace Core.Settings
{
    /// <summary>
    /// RandNet application settings organization and manipulation interface.
    /// </summary>
    public static class RandNetSettings
    {
        static private String defaultDirectory = 
            Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\xRandNet";

        static private Configuration config;

        static private String loggingDirectory = "";        
        static private String storageDirectory = "";
        static private String tracingDirectory = "";
        static private TracingType tracingType;
        static private ManagerType workingMode;
        static private String staticGenerationDirectory = "";
        static private String matrixConvertionToolDirectory = "";
        static private String modelCheckingToolDirectory = "";
        static private String dataConvertionToolDirectory = "";

        static RandNetSettings()
        {
            if (!CustomLogger.WebMode)
            {
                SetConfiguration();
            }
        }

        static public String LoggingDirectory
        {
            get
            {
                return (loggingDirectory == "") ? defaultDirectory + "\\Logging" : loggingDirectory;
            }
            set
            {
                if (value == loggingDirectory)
                    return;
                if (value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    loggingDirectory = value;
                else
                    loggingDirectory = value + Path.DirectorySeparatorChar;

                if (Directory.Exists(loggingDirectory) == false)
                    Directory.CreateDirectory(loggingDirectory);
                
                config.AppSettings.Settings["LoggingDirectory"].Value = loggingDirectory;
                InitializeLogging("");
            }
        }

        static public String StorageDirectory
        {
            get
            {
                return (storageDirectory == "") ? defaultDirectory + "\\Results" : storageDirectory;
            }
            set
            {
                if (value == storageDirectory)
                    return;
                if (value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    storageDirectory = value;
                else
                    storageDirectory = value + Path.DirectorySeparatorChar;

                if (Directory.Exists(storageDirectory) == false)
                    Directory.CreateDirectory(storageDirectory);

                config.AppSettings.Settings["StorageDirectory"].Value = storageDirectory;
            }
        }

        static public String TracingDirectory
        {
            get
            {
                return (tracingDirectory == "") ? defaultDirectory + "\\Tracing" : tracingDirectory;
            }
            set
            {
                if (value == tracingDirectory)
                    return;
                if (value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    tracingDirectory = value;
                else
                    tracingDirectory = value + Path.DirectorySeparatorChar;

                if (Directory.Exists(tracingDirectory) == false)
                    Directory.CreateDirectory(tracingDirectory);

                config.AppSettings.Settings["TracingDirectory"].Value = tracingDirectory;
            }
        }

        static public TracingType TracingType
        {
            get
            {
                return tracingType;
            }
            set
            {
                if (value == tracingType)
                    return;
                tracingType = value;
                config.AppSettings.Settings["TracingType"].Value = tracingType.ToString();
            }
        }

        static public ManagerType WorkingMode 
        {
            get
            {
                return workingMode;
            }
            set
            {
                if (value == workingMode)
                    return;
                workingMode = value;
                config.AppSettings.Settings["WorkingMode"].Value = workingMode.ToString();
            }
        }

        static public String StaticGenerationDirectory
        {
            get
            {
                return (staticGenerationDirectory == "") ? defaultDirectory + "\\Tracing" : staticGenerationDirectory;
            }
            set
            {
                if (value == staticGenerationDirectory)
                    return;
                if (value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    staticGenerationDirectory = value;
                else
                    staticGenerationDirectory = value + Path.DirectorySeparatorChar;

                if (Directory.Exists(staticGenerationDirectory) == false)
                    Directory.CreateDirectory(staticGenerationDirectory);

                config.AppSettings.Settings["StaticGenerationDirectory"].Value = staticGenerationDirectory;
            }
        }

        static public String MatrixConvertionToolDirectory
        {
            get
            {
                return (matrixConvertionToolDirectory == "") ? defaultDirectory + "\\Tracing" : matrixConvertionToolDirectory;
            }
            set
            {
                if (value == matrixConvertionToolDirectory)
                    return;
                if (value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    matrixConvertionToolDirectory = value;
                else
                    matrixConvertionToolDirectory = value + Path.DirectorySeparatorChar;

                if (Directory.Exists(matrixConvertionToolDirectory) == false)
                    Directory.CreateDirectory(matrixConvertionToolDirectory);

                config.AppSettings.Settings["MatrixConvertionToolDirectory"].Value = matrixConvertionToolDirectory;
            }
        }

        static public String ModelCheckingToolDirectory
        {
            get
            {
                return (modelCheckingToolDirectory == "") ? defaultDirectory + "\\Tracing" : modelCheckingToolDirectory;
            }
            set
            {
                if (value == modelCheckingToolDirectory)
                    return;
                if (value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    modelCheckingToolDirectory = value;
                else
                    modelCheckingToolDirectory = value + Path.DirectorySeparatorChar;

                if (Directory.Exists(modelCheckingToolDirectory) == false)
                    Directory.CreateDirectory(modelCheckingToolDirectory);

                config.AppSettings.Settings["ModelCheckingToolDirectory"].Value = modelCheckingToolDirectory;
            }
        }

        static public String DataConvertionToolDirectory
        {
            get
            {
                return (dataConvertionToolDirectory == "") ? defaultDirectory + "\\Results" : dataConvertionToolDirectory;
            }
            set
            {
                if (value == dataConvertionToolDirectory)
                    return;
                if (value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    dataConvertionToolDirectory = value;
                else
                    dataConvertionToolDirectory = value + Path.DirectorySeparatorChar;

                if (Directory.Exists(dataConvertionToolDirectory) == false)
                    Directory.CreateDirectory(dataConvertionToolDirectory);

                config.AppSettings.Settings["DataConvertionToolDirectory"].Value = dataConvertionToolDirectory;
            }
        }

        /// <summary>
        /// Refreshes app.config file content.
        /// </summary>
        static public void Refresh()
        {
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.RefreshSection("loggingConfiguration");
        }

        static public void InitializeLogging(String fileName)
        {
            LoggingSettings loggingSettings = (LoggingSettings)config.GetSection("loggingConfiguration");
            Debug.Assert(loggingSettings != null);
            TraceListenerData traceListenerData = loggingSettings.TraceListeners.Get("Flat File Trace Listener");
            Debug.Assert(traceListenerData != null);
            FlatFileTraceListenerData file = traceListenerData as FlatFileTraceListenerData;
            Debug.Assert(file != null);

            if (fileName == "")
            {
                File.Copy(file.FileName, LoggingDirectory + Path.GetFileName(file.FileName));
                file.FileName = LoggingDirectory + Path.GetFileName(file.FileName);
            }
            else
            {
                file.FileName = LoggingDirectory + "\\" + fileName + ".log";
                loggingDirectory = Path.GetDirectoryName(file.FileName);
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("loggingConfiguration");
        }

        static private void RefreshLoggingPath()
        {
            InitializeLogging("");
        }

        public static void ChangeDefaultDirectory(string newDirectory)
        {
            defaultDirectory = newDirectory;
        }

        private static void SetConfiguration()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                
            try
            {
                loggingDirectory = config.AppSettings.Settings["LoggingDirectory"].Value;
                storageDirectory = config.AppSettings.Settings["StorageDirectory"].Value;
                tracingDirectory = config.AppSettings.Settings["TracingDirectory"].Value;
                tracingType = (TracingType)Enum.Parse(typeof(TracingType),
                    config.AppSettings.Settings["Tracingtype"].Value);
                workingMode = (ManagerType)Enum.Parse(typeof(ManagerType), 
                    config.AppSettings.Settings["WorkingMode"].Value);
                staticGenerationDirectory = config.AppSettings.Settings["StaticGenerationDirectory"].Value;
                matrixConvertionToolDirectory = config.AppSettings.Settings["MatrixConvertionToolDirectory"].Value;
                modelCheckingToolDirectory = config.AppSettings.Settings["ModelCheckingToolDirectory"].Value;
                dataConvertionToolDirectory = config.AppSettings.Settings["DataConvertionToolDirectory"].Value;
            }
            catch
            {
                throw new CoreException("The structure of Configuration file is not correct.");
            }
        }
    }
}
