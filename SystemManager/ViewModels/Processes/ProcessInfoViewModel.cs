using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.ProcessesManagement.Data;
using SystemManager.ViewModels.Base;

namespace SystemManager.ViewModels.Processes
{
    public class ProcessInfoViewModel : BaseViewModel
    {

        //  VARIABLES

        private ProcessInfo _processInfo;


        //  GETTERS & SETTERS
        
        public int Id
        {
            get => _processInfo.Id;
            set
            {
                _processInfo.Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string? Name
        {
            get => _processInfo.Name;
            set
            {
                _processInfo.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string? Description
        {
            get => _processInfo.Description;
            set
            {
                _processInfo.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string? Type
        {
            get => _processInfo.Type;
            set
            {
                _processInfo.Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public string? CommandLocation
        {
            get => _processInfo.CommandLocation;
            set
            {
                _processInfo.CommandLocation = value;
                OnPropertyChanged(nameof(CommandLocation));
            }
        }

        public double CPUUsage
        {
            get => _processInfo.CPUUsage;
            set
            {
                _processInfo.CPUUsage = value;
                OnPropertyChanged(nameof(CPUUsage));
            }
        }

        public long MemoryUsage
        {
            get => _processInfo.MemoryUsage;
            set
            {
                _processInfo.MemoryUsage = value;
                OnPropertyChanged(nameof(MemoryUsage));
            }
        }

        public bool HasWindows
        {
            get => _processInfo.HasWindows;
            set
            {
                _processInfo.HasWindows = value;
                OnPropertyChanged(nameof(HasWindows));
            }
        }

        public bool IsSystemService
        {
            get => _processInfo.IsSystemService;
            set
            {
                _processInfo.IsSystemService = value;
                OnPropertyChanged(nameof(IsSystemService));
            }
        }

        public ProcessMode Mode
        {
            get => _processInfo.Mode;
            set
            {
                _processInfo.Mode = value;
                OnPropertyChanged(nameof(Mode));
            }
        }

        public ProcessPriorityClass? Priority
        {
            get => _processInfo.Priority;
            set
            {
                _processInfo.Priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }

        public int ThreadCount
        {
            get => _processInfo.ThreadCount;
            set
            {
                _processInfo.ThreadCount = value;
                OnPropertyChanged(nameof(ThreadCount));
            }
        }

        public TimeSpan Uptime
        {
            get => _processInfo.Uptime;
            set
            {
                _processInfo.Uptime = value;
                OnPropertyChanged(nameof(Uptime));
            }
        }

        public string? UserName
        {
            get => _processInfo.UserName;
            set
            {
                _processInfo.UserName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ProcessInfoViewModel class constructor. </summary>
        /// <param name="processInfo"> Process information. </param>
        public ProcessInfoViewModel(ProcessInfo processInfo)
        {
            _processInfo = processInfo;

            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(Type));
            OnPropertyChanged(nameof(CommandLocation));
            OnPropertyChanged(nameof(CPUUsage));
            OnPropertyChanged(nameof(MemoryUsage));
            OnPropertyChanged(nameof(HasWindows));
            OnPropertyChanged(nameof(IsSystemService));
            OnPropertyChanged(nameof(Mode));
            OnPropertyChanged(nameof(Priority));
            OnPropertyChanged(nameof(ThreadCount));
            OnPropertyChanged(nameof(Uptime));
            OnPropertyChanged(nameof(UserName));
        }

        #endregion CLASS METHODS

    }
}
