using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SystemManager.ViewModels.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        //  EVENTS

        public event PropertyChangedEventHandler? PropertyChanged;


        //  METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Triggers the given propertyName to be updated in the UI. </summary>
        /// <param name="propertyName"> Field property name. </param>
        public virtual void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Triggers the given propertyName to be updated in the UI, and set ref field to new value. </summary>
        /// <typeparam name="T"> Field and value type. </typeparam>
        /// <param name="field"> Field. </param>
        /// <param name="newValue"> New value to set in field. </param>
        /// <param name="propertyName"> Field property name. </param>
        public virtual void UpdateProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }

        #endregion PROPERITES CHANGED METHODS

    }
}
