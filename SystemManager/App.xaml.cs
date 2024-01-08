using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SystemManager.Data.Configuration;

namespace SystemManager
{
    public partial class App : Application
    {

        //  VARIABLES

        private AppearanceConfig? _appearanceConfig;
        private ConfigManager? _configManager;


        //  GETTERS & SETTERS

        public AppearanceConfig AppearanceConfig
        {
            get
            {
                if (_appearanceConfig == null)
                    _appearanceConfig = AppearanceConfig.Instance;

                return _appearanceConfig;
            }
        }

        public ConfigManager ConfigManager
        {
            get
            {
                if (_configManager == null)
                    _configManager = ConfigManager.Instance;

                return _configManager;
            }
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after application startup. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Startup Event Arguments. </param>
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            _appearanceConfig = AppearanceConfig.Instance;
            _configManager = ConfigManager.Instance;
        }

        #endregion CLASS METHODS

    }
}
