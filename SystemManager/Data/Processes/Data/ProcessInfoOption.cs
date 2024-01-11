using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManager.ViewModels.Base;

namespace SystemManager.Data.Processes.Data
{
    public class ProcessInfoOption : BaseViewModel
    {

        //  VARIABLES

        private bool _id = true;
        private bool _name = true;
        private bool _description = false;
        private bool _type = true;
        private bool _commandLocation = true;
        private bool _cpuUsage = false;
        private bool _memoryUsage = false;
        private bool _isSystemService = true;
        private bool _mode = true;
        private bool _priority = true;
        private bool _threadCount = true;
        private bool _uptime = false;
        private bool _userName = false;


        //  GETTERS & SETTERS

        public bool Id
        {
            get => _id;
            set => UpdateProperty(ref _id, value);
        }

        public bool Name
        {
            get => _name;
            set => UpdateProperty(ref _name, value);
        }

        public bool Description
        {
            get => _description;
            set => UpdateProperty(ref _description, value);
        }

        public bool Type
        {
            get => _type;
            set => UpdateProperty(ref _type, value);
        }

        public bool CommandLocation
        {
            get => _commandLocation;
            set => UpdateProperty(ref _commandLocation, value);
        }

        public bool CPUUsage
        {
            get => _cpuUsage;
            set => UpdateProperty(ref _cpuUsage, value);
        }

        public bool MemoryUsage
        {
            get => _memoryUsage;
            set => UpdateProperty(ref _memoryUsage, value);
        }

        public bool IsSystemService
        {
            get => _isSystemService;
            set => UpdateProperty(ref _isSystemService, value);
        }

        public bool Mode
        {
            get => _mode;
            set => UpdateProperty(ref _mode, value);
        }

        public bool Priority
        {
            get => _priority;
            set => UpdateProperty(ref _priority, value);
        }

        public bool ThreadCount
        {
            get => _threadCount;
            set => UpdateProperty(ref _threadCount, value);
        }

        public bool Uptime
        {
            get => _uptime;
            set => UpdateProperty(ref _uptime, value);
        }

        public bool UserName
        {
            get => _userName;
            set => UpdateProperty(ref _userName, value);
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ProcessInfoOption class constructor. </summary>
        [JsonConstructor]
        public ProcessInfoOption(
            bool? id = null,
            bool? name = null,
            bool? description = null,
            bool? type = null,
            bool? commandLocation = null,
            bool? cpuUsage = null,
            bool? memoryUsage = null,
            bool? isSystemService = null,
            bool? mode = null,
            bool? priority = null,
            bool? threadCount = null,
            bool? uptime = null,
            bool? userName = null)
        {
            Id = id.HasValue ? id.Value : true;
            Name = name.HasValue ? name.Value : true;
            Description = description.HasValue ? description.Value : false;
            Type = type.HasValue ? type.Value : true;
            CommandLocation = commandLocation.HasValue ? commandLocation.Value : true;
            CPUUsage = cpuUsage.HasValue ? cpuUsage.Value : false;
            MemoryUsage = memoryUsage.HasValue ? memoryUsage.Value : false;
            IsSystemService = isSystemService.HasValue ? isSystemService.Value : true;
            Mode = mode.HasValue ? mode.Value : true;
            Priority = priority.HasValue ? priority.Value : true;
            ThreadCount = threadCount.HasValue ? threadCount.Value : true;
            Uptime = uptime.HasValue ? uptime.Value : false;
            UserName = userName.HasValue ? userName.Value : false;
        }

        #endregion CLASS METHODS

    }
}
