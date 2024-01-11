using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemController.ProcessesManagement.Data
{
    public enum ProcessActionResult
    {
        Success = 0,
        ProcessNotExist = 1,
        UnknownError = 2,
    }
}
