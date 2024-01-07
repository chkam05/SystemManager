using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemController.ProcessesManagement.Data
{
    public class ProcessInfo
    {

        //  VARIABLES

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? CommandLocation { get; set; }
        public double CPUUsage { get; set; }
        public long MemoryUsage { get; set; }
        public bool IsSystemService { get; set; }
        public ProcessMode Mode { get; set; }
        public ProcessPriorityClass Priority { get; set; }
        public int ThreadCount { get; set; }
        public TimeSpan Uptime { get; set; }
        public string UserName { get; set; }
        public List<WindowInfo> Windows { get; set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ProcessInfo class constructor. </summary>
        /// <param name="windows"> List of process windows. </param>
        public ProcessInfo(List<WindowInfo>? windows = null)
        {
            Windows = windows ?? new List<WindowInfo>();
        }

        #endregion CLASS METHODS

    }
}
