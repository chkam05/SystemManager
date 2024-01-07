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

        public string? ClassName { get; set; }
        public IntPtr Handle { get; set; }
        public List<WindowInfo> ChildWindows { get; set; }
        public WindowInfo? ParentWindow { get; set; }
        public POINT Position { get; set; }
        public SIZE Size { get; set; }
        public WindowState State { get; set; }
        public string? Title { get; set; }
        public bool Visible { get; set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> WindowInfo class constructor. </summary>
        /// <param name="childWindows"> List of child windows. </param>
        public WindowInfo(List<WindowInfo>? childWindows = null)
        {
            ChildWindows = childWindows ?? new List<WindowInfo>();
        }

        #endregion CLASS METHODS

    }
}
