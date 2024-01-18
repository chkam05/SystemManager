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

        public ProcessInfo ProcessInfo { get; private set; }


        //  GETTERS & SETTERS
        
        public int Id
        {
            get => ProcessInfo.Id;
            set
            {
                ProcessInfo.Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string? Name
        {
            get => ProcessInfo.Name;
            set
            {
                ProcessInfo.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string? Description
        {
            get => ProcessInfo.Description;
            set
            {
                ProcessInfo.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string? Type
        {
            get => ProcessInfo.Type;
            set
            {
                ProcessInfo.Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public string? CommandLocation
        {
            get => ProcessInfo.CommandLocation;
            set
            {
                ProcessInfo.CommandLocation = value;
                OnPropertyChanged(nameof(CommandLocation));
            }
        }

        public double? CPUUsage
        {
            get => ProcessInfo.CPUUsage;
            set
            {
                ProcessInfo.CPUUsage = value;
                OnPropertyChanged(nameof(CPUUsage));
            }
        }

        public long? MemoryUsage
        {
            get => ProcessInfo.MemoryUsage;
            set
            {
                ProcessInfo.MemoryUsage = value;
                OnPropertyChanged(nameof(MemoryUsage));
            }
        }

        public bool HasWindows
        {
            get => ProcessInfo.HasWindows;
            set
            {
                ProcessInfo.HasWindows = value;
                OnPropertyChanged(nameof(HasWindows));
            }
        }

        public bool IsSystemService
        {
            get => ProcessInfo.IsSystemService;
            set
            {
                ProcessInfo.IsSystemService = value;
                OnPropertyChanged(nameof(IsSystemService));
            }
        }

        public ProcessMode? Mode
        {
            get => ProcessInfo.Mode;
            set
            {
                ProcessInfo.Mode = value;
                OnPropertyChanged(nameof(Mode));
            }
        }

        public ProcessPriorityClass? Priority
        {
            get => ProcessInfo.Priority;
            set
            {
                ProcessInfo.Priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }

        public int ThreadCount
        {
            get => ProcessInfo.ThreadCount;
            set
            {
                ProcessInfo.ThreadCount = value;
                OnPropertyChanged(nameof(ThreadCount));
            }
        }

        public TimeSpan? Uptime
        {
            get => ProcessInfo.Uptime;
            set
            {
                ProcessInfo.Uptime = value;
                OnPropertyChanged(nameof(Uptime));
            }
        }

        public string? UserName
        {
            get => ProcessInfo.UserName;
            set
            {
                ProcessInfo.UserName = value;
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
            ProcessInfo = processInfo;

            OnProcessInfoPropertyUpdate();
        }

        #endregion CLASS METHODS

        #region COMPARATION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Determines whether the specified object is equal to the current object. </summary>
        /// <param name="obj"> Object to compare. </param>
        /// <returns> True - object is equal to the current object; False - otherwise. </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj is ProcessInfoViewModel processInfoViewModel)
                return processInfoViewModel.GetHashCode() == GetHashCode();

            return false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Serves as the default hash function. </summary>
        /// <returns> A hash code for the current object. </returns>
        public override int GetHashCode()
        {
            return ProcessInfo.GetHashCode();
        }

        #endregion COMPARATION METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Triggers properies, related with ProcessInfo to be updated in the UI. </summary>
        private void OnProcessInfoPropertyUpdate()
        {
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

        #endregion PROPERITES CHANGED METHODS

        #region UPDATE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Update process information. </summary>
        /// <param name="processInfo"> Process information. </param>
        public void Update(ProcessInfo processInfo)
        {
            ProcessInfo = processInfo;

            OnProcessInfoPropertyUpdate();
        }

        #endregion UPDATE METHODS

    }
}
