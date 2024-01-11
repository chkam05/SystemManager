using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemController.ProcessesManagement.Data
{
    [Flags]
    public enum ProcessAccessFlags : uint
    {
        VirtualMemoryRead = 0x0010,
        VirtualMemoryWrite = 0x0020,
        DuplicateHandle = 0x0040,
        QueryInformation = 0x0400,
        QueryLimitedInformation = 0x00001000,
        All = 0x1F0FFF,
    }
}
