using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemController.Processes.Data
{
    public enum WindowActionResult
    {
        Success = 0,
        WindowNotExist = 1,
        ProcessNotExist = 2,
        UnknownError = 3,
    }
}
