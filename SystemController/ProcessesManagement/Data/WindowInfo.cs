using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        public WindowAttributes Attributes { get; set; }
        public POINT Position { get; set; }
        public WindowRole Role { get; set; }
        public SIZE Size { get; set; }
        public WindowState State { get; set; }
        public string? Title { get; set; }
        public int Transparency { get; set; }
        public bool Visible { get; set; }


        //  GETTERS & SETTERS

        public int ChildWindowsCount
        {
            get => ChildWindows?.Count ?? 0;
        }


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

        #region EQUAL METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Determines whether the specified object is equal to the current object. </summary>
        /// <param name="obj"> Object to compare. </param>
        /// <returns> True - object is equal to the current object; False - otherwise. </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj is WindowInfo windowInfo)
            {
                //  Compare parent windows.
                if (windowInfo.ParentWindow != null && ParentWindow != null)
                {
                    if (windowInfo.ParentWindow.Equals(ParentWindow))
                        return false;
                }

                if (windowInfo.ParentWindow != null || ParentWindow != null)
                    return false;

                //  Compare child windows.
                if (windowInfo.ChildWindowsCount != ChildWindowsCount)
                    return false;

                if (windowInfo.ChildWindows?.Any() ?? false)
                {
                    var newWindows = windowInfo.ChildWindows.Where(w => !ChildWindows.Contains(w));
                    var oldWindows = ChildWindows.Where(w => !windowInfo.ChildWindows.Contains(w));

                    if (newWindows.Any())
                        return false;

                    if (oldWindows.Any())
                        return false;

                    foreach (var newWindow in windowInfo.ChildWindows)
                    {
                        var oldWindow = ChildWindows.FirstOrDefault(w => w.Handle == newWindow.Handle);

                        if (!newWindow.Equals(oldWindow))
                            return false;
                    }
                }

                //  Compare parameters.
                return windowInfo.GetHashCode() == GetHashCode();
            }

            return false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Serves as the default hash function. </summary>
        /// <returns> A hash code for the current object. </returns>
        public override int GetHashCode()
        {
            long childHashCode = 0;

            if (ChildWindows.Any())
            {
                foreach (var childWindow in ChildWindows)
                    childHashCode += childWindow.GetHashCode();

                childHashCode = childHashCode / ChildWindows.Count;
            }

            long hashCode = ((long) Handle.GetHashCode())
                + GetInternalHash(ClassName)
                + Attributes.GetHashCode()
                + Position.X.GetHashCode()
                + Position.Y.GetHashCode()
                + Role.GetHashCode()
                + Size.Width.GetHashCode()
                + Size.Height.GetHashCode()
                + State.GetHashCode()
                + GetInternalHash(Title)
                + Transparency.GetHashCode()
                + Visible.GetHashCode()
                + (ParentWindow != null ? ParentWindow.GetHashCode() : 0)
                + childHashCode;

            return Convert.ToInt32(hashCode / 14);
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

        #endregion EQUAL METHODS

    }
}
