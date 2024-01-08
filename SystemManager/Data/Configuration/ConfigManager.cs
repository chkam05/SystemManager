using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemManager.ViewModels.Base;

namespace SystemManager.Data.Configuration
{
    public class ConfigManager : BaseViewModel
    {

        //  CONST

        private static readonly string FILE_NAME = "config.json";


        //  VARIABLES

        private static ConfigManager _instance;
        private static object _instanceLock = new object();

        private Config? _config;


        //  GETTERS & SETTERS

        public static ConfigManager Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                        _instance = new ConfigManager();

                    return _instance;
                }
            }
        }

        public Config Config
        {
            get
            {
                if (_config == null)
                    _config = new Config();

                return _config;
            }
            set => UpdateProperty(ref _config, value);
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ConfigManager class constructor. </summary>
        private ConfigManager()
        {
            LoadConfig();
        }

        #endregion CLASS METHODS

        #region LOAD & SAVE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Load configuration. </summary>
        public void LoadConfig()
        {
            Config = ReadConfigFromFile();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Read configuration from file. </summary>
        /// <returns> Configuration object. </returns>
        private Config ReadConfigFromFile()
        {
            var configFilePath = GetConfigFilePath();

            if (!File.Exists(configFilePath))
                return new Config();

            try
            {
                var fileContent = File.ReadAllText(configFilePath);
                var config = JsonConvert.DeserializeObject<Config>(fileContent);

                return config ?? new Config();
            }
            catch (Exception)
            {
                return new Config();
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Save configuration. </summary>
        public void SaveConfig()
        {
            var configFilePath = GetConfigFilePath();

            try
            {
                var fileContent = JsonConvert.SerializeObject(Config);
                File.WriteAllText(configFilePath, fileContent);
            }
            catch (Exception)
            {
                //
            }
        }

        #endregion LOAD & SAVE METHODS

        #region UTILITY METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Get configuration file path. </summary>
        /// <returns> Configuration file path. </returns>
        public string GetConfigFilePath()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName()?.Name ?? "SystemManager";

            var appData = Environment.GetEnvironmentVariable("APPDATA");
            var basePath = Path.Combine(appData, assemblyName);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            return Path.Combine(basePath, FILE_NAME);
        }

        #endregion UTILITY METHODS

    }
}
