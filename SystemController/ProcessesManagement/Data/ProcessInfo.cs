﻿using System;
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
        public double? CPUUsage { get; set; }               //  Not working at all
        public long? MemoryUsage { get; set; }
        public bool HasWindows { get; set; }
        public bool IsSystemService { get; set; }           //  Propably not working - Unable to proper debug
        public ProcessMode Mode { get; set; }               //  Propably not working - Unable to proper debug
        public ProcessPriorityClass? Priority { get; set; }
        public int ThreadCount { get; set; }
        public TimeSpan? Uptime { get; set; }
        public string? UserName { get; set; }               //  Not working as expected

    }
}
