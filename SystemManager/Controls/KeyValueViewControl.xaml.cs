using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SystemManager.Controls
{
    public partial class KeyValueViewControl : UserControl
    {

        //  DEPENDENCY PROPERTIES

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(KeyValueViewControl),
            new PropertyMetadata(""));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(string),
            typeof(KeyValueViewControl),
            new PropertyMetadata(""));


        //  GETTERS & SETTERS

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public string Value
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> KeyValueViewControl class constructor. </summary>
        public KeyValueViewControl()
        {
            //  Initialize user interface.
            InitializeComponent();
        }

        #endregion CLASS METHODS

    }
}
