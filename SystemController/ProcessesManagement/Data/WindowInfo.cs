using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.Data;

namespace SystemController.ProcessesManagement.Data
{
    public class WindowInfo
    {

        //  VARIABLES

        public IntPtr Handle { get; set; }
        public POINT Position { get; set; }
        public SIZE Size { get; set; }
        public WindowState State { get; set; }
        public string? Title { get; set; }
        public bool Visible { get; set; }
        
    }
}
