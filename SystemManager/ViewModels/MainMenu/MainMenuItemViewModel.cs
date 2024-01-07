using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManager.ViewModels.Base;

namespace SystemManager.ViewModels.MainMenu
{
    public class MainMenuItemViewModel : BaseViewModel
    {

        //  VARIABLES

        private string? _title;
        private string? _description;
        private PackIconKind _icon = PackIconKind.None;
        private Action? _action;


        //  GETTERS

        public string? Title
        {
            get => this._title;
            set => UpdateProperty(ref this._title, value);
        }

        public string? Description
        {
            get => this._description;
            set => UpdateProperty(ref this._description, value);
        }

        public PackIconKind Icon
        {
            get => _icon;
            set => UpdateProperty(ref this._icon, value);
        }

        public Action? Action
        {
            get => this._action;
            set => UpdateProperty(ref this._action, value);
        }


        //  METHDOS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MainMenuItemViewModel class constructor. </summary>
        public MainMenuItemViewModel()
        {
            //
        }

        //  --------------------------------------------------------------------------------
        /// <summary> MainMenuItemViewModel class constructor. </summary>
        /// <param name="title"> Main item title. </param>
        /// <param name="description"> Menu item description. </param>
        /// <param name="action"> Menu item action to execute after select. </param>
        public MainMenuItemViewModel(string title, string? description = null, PackIconKind? icon = null, Action? action = null)
        {
            this.Title = title;
            this.Description = description;
            this.Icon = icon ?? PackIconKind.None;
            this.Action = action;
        }

        #endregion CLASS METHODS

    }
}
