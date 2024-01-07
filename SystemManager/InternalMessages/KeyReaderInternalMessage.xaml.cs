using chkam05.Tools.ControlsEx;
using chkam05.Tools.ControlsEx.Data;
using chkam05.Tools.ControlsEx.Events;
using chkam05.Tools.ControlsEx.InternalMessages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SystemController.MouseKeyboard;
using SystemController.MouseKeyboard.Data;
using SystemController.MouseKeyboard.Events;
using SystemManager.Utilities;

namespace SystemManager.InternalMessages
{
    public partial class KeyReaderInternalMessage : StandardInternalMessageEx
    {

        //  VARIABLES

        private bool _canceled = false;
        private bool _isListening = false;
        private bool _multiKeys = false;

        private ButtonEx? _okButton;
        private KeyboardReader _keyboardReader;
        private ObservableCollection<byte> _pressedKeys;


        //  GETTERS & SETTERS

        public ObservableCollection<byte> PressedKeys
        {
            get => _pressedKeys;
            set
            {
                _pressedKeys = value;
                _pressedKeys.CollectionChanged += OnPressedKeysCollectionChanged;
                OnPropertyChanged(nameof(PressedKeys));
            }
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> KeyReaderInternalMessage class constructor. </summary>
        /// <param name="parentContainer"> InternalMessagesExContainer. </param>
        public KeyReaderInternalMessage(InternalMessagesExContainer parentContainer, KeyboardReader keyboardReader,
            bool multiKeys = false) : base(parentContainer)
        {
            //  Setup data.
            _multiKeys = multiKeys;

            _keyboardReader = keyboardReader;
            _pressedKeys = new ObservableCollection<byte>();

            _keyboardReader.OnKeyPress += OnKeyPressed;

            //  Initialize user interface.
            InitializeComponent();

            //  Interface configuration.
            Buttons = new InternalMessageButtons[]
            {
                InternalMessageButtons.OkButton
            };
        }

        #endregion CLASS METHODS

        #region INTERNAL MESSAGE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Create Internal Message Close Event Arguments. </summary>
        /// <returns> Internal Message Close Event Arguments. </returns>
        protected override InternalMessageCloseEventArgs CreateCloseEventArgs()
        {
            return new InternalMessageCloseEventArgs(
                _canceled ? InternalMessageResult.Cancel : Result);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after internal message loaded. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void KeyReaderInternalMessageLoaded(object sender, RoutedEventArgs e)
        {
            if (_okButton != null)
                _okButton.IsEnabled = false;

            _isListening = true;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after internal message loaded. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void KeyReaderInternalMessageUnloaded(object sender, RoutedEventArgs e)
        {
            _keyboardReader.OnKeyPress -= OnKeyPressed;
        }

        #endregion INTERNAL MESSAGE METHODS

        #region KEY LISTENING METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after pressing key in KeyboardReader. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Key Press Event Arguments. </param>
        private void OnKeyPressed(object sender, KeyPressEventArgs e)
        {
            if (!_isListening)
                return;

            if (e.KeyState == KeyState.Press)
            {
                PressedKeys.Add(e.KeyCode);
                OnPropertyChanged(nameof(PressedKeys));
            }

            if (!_multiKeys || e.HeldKeys == 0)
            {
                _isListening = false;

                _subTitle.Text = "Pressed keys:";

                if (_okButton != null)
                    _okButton.IsEnabled = true;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after exception thrown in KeyboardReader. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Key Press Exception Event Arguments. </param>
        private void OnKeyPressThrownException(object sender, ThrownExceptionEventArgs e)
        {
            var title = "Keyboard Reader Error";
            var message = e.Exception.Message;

            var imError = InternalMessageEx.CreateErrorMessage(_parentContainer, title, message);
            InternalMessageExHelper.SetInternalMessageAppearance(imError);

            _canceled = true;

            Close();

            _parentContainer.ShowMessage(imError);
        }

        #endregion KEY LISTENING METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Triggers propertyName to be updated in the UI by PressedKeys collection changed. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Notify Collection Changed Event Arguments. </param>
        public virtual void OnPressedKeysCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PressedKeys));
        }

        #endregion PROPERITES CHANGED METHODS

        #region TEMPLATE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> When overridden in a derived class,cis invoked whenever 
        /// application code or internal processes call ApplyTemplate. </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _okButton = GetButtonEx("okButton");

            if (_okButton != null)
                _okButton.Content = "Close";
        }

        #endregion TEMPLATE METHODS

    }
}
