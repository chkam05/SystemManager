using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemController.Processes.Data
{
    public class ProcessInfo
    {

        //  VARIABLES

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? CommandLocation { get; set; }
        public double? CPUUsage { get; set; }               //  Not working at all
        public long? MemoryUsage { get; set; }
        public bool HasWindows { get; set; }
        public bool IsSystemService { get; set; }           //  Propably not working - Unable to proper debug
        public ProcessMode? Mode { get; set; }               //  Propably not working - Unable to proper debug
        public ProcessPriorityClass? Priority { get; set; }
        public int ThreadCount { get; set; }
        public TimeSpan? Uptime { get; set; }
        public string? UserName { get; set; }               //  Not working as expected


        //  METHODS

        #region COMPARATION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Determines whether the specified object is equal to the current object. </summary>
        /// <param name="obj"> Object to compare. </param>
        /// <returns> True - object is equal to the current object; False - otherwise. </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj is ProcessInfo processInfo)
                return processInfo.GetHashCode() == GetHashCode();

            return false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Serves as the default hash function. </summary>
        /// <returns> A hash code for the current object. </returns>
        public override int GetHashCode()
        {
            return Id.ToString().GetHashCode()
                + GetInternalHash(Name)
                + GetInternalHash(Description)
                + GetInternalHash(Type)
                + GetInternalHash(CommandLocation)
                + (CPUUsage.HasValue ? CPUUsage.Value.GetHashCode() : 0)
                + (MemoryUsage.HasValue ? MemoryUsage.Value.GetHashCode() : 0)
                + HasWindows.GetHashCode()
                + IsSystemService.GetHashCode()
                + (Mode.HasValue ? Mode.Value.GetHashCode() : 0)
                + (Priority.HasValue ? Priority.Value.GetHashCode() : 0)
                + ThreadCount.GetHashCode()
                + (Uptime.HasValue ? Uptime.GetHashCode() : 0)
                + GetInternalHash(UserName);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get internal value hash. </summary>
        /// <param name="value"> Internal value. </param>
        /// <returns> A hash code for internal value. </returns>
        private int GetInternalHash(string? value)
        {
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                return value.GetHashCode();

            return 0;
        }

        #endregion COMPARATION METHODS

    }
}
