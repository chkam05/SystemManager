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
        QueryInformation = 0x0400,
        VirtualMemoryRead = 0x0010,
        VirtualMemoryWrite = 0x0020,
        DuplicateHandle = 0x0040,
        All = 0x1F0FFF,
    }
}
