using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemManager.Data.Processes.Data
{
    public enum ProcessCompareResult
    {
        Equal = 0,
        NotEqual = 1,
        New = 2,
        Removed = 3
    }
}
