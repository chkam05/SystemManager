using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SystemManager.Data.Macros.DataModels
{
    public class MacroKeyCombination : MacroBase
    {

        //  VARIABLES

        private ObservableCollection<byte> _keyCodes;


        //  GETTERS & SETTERS

        public ObservableCollection<byte> KeyCodes
        {
            get => _keyCodes;
            set
            {
                UpdateProperty(ref _keyCodes, value);
                KeyCodes.CollectionChanged += OnKeyCodesCollectionChanged;
            }
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MacroKeyCombination class constructor. </summary>
        /// <param name="keyCode"> Key code. </param>
        public MacroKeyCombination()
        {
            MacroType = MacroType.KeyCombination;
            _keyCodes = new ObservableCollection<byte>();
        }

        #endregion CLASS METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after changing KeyCodes collection. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Notify Collection Changed Event Arguments. </param>
        protected void OnKeyCodesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(KeyCodes));
        }

        #endregion PROPERITES CHANGED METHODS

    }
}
