using chkam05.Tools.ControlsEx;
using chkam05.Tools.ControlsEx.InternalMessages;
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
using SystemManager.Data.Processes;
using SystemManager.Data.Processes.Events;
using SystemManager.Utilities;

namespace SystemManager.Pages
{
    public partial class ProcessesPage : Page
    {

        //  VARIABLES

        private InternalMessagesExContainer _imContainer;
        private ProcessesDataContext _processesDataContext;


        //  METHDOS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ProcessesPage class constructor. </summary>
        /// <param name="imContainer"> InternalMessageExContainer. </param>
        public ProcessesPage(InternalMessagesExContainer imContainer)
        {
            //  Setup components.
            _imContainer = imContainer;
            _processesDataContext = new ProcessesDataContext();

            _processesDataContext.ProcessesLoaded += OnProcessesLoaded;

            //  Initialize user interface.
            InitializeComponent();

            //  Setup context.
            DataContext = _processesDataContext;
        }

        #endregion CLASS METHODS

        #region DATA CONTEXT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after loading processes. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Processes Loaded Finished Event Arguments. </param>
        public void OnProcessesLoaded(object? sender, ProcessesLoaderFinishedEventArgs e)
        {
            if (e.Exception != null)
            {
                var title = "Processes loading error";
                var message = $"An error occurred while loading processes:{Environment.NewLine}{e.Exception.Message}";
                var imError = InternalMessageEx.CreateErrorMessage(_imContainer, title, message);

                InternalMessageExHelper.SetInternalMessageAppearance(imError);

                _imContainer.ShowMessage(imError);
            }
        }

        #endregion DATA CONTEXT METHODS

        #region HEADER BUTTONS INTERACTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AutoRefresh ButtonEx. </summary>
        /// <param name="sender"> Object taht invoked the method. </param>
        /// <param name="e"> Routed Event Args. </param>
        private void AutoRefreshButtonExClick(object sender, RoutedEventArgs e)
        {
            //
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking Refresh ButtonEx. </summary>
        /// <param name="sender"> Object taht invoked the method. </param>
        /// <param name="e"> Routed Event Args. </param>
        private void RefreshButtonExClick(object sender, RoutedEventArgs e)
        {
            _processesDataContext.LoadProcesses();
        }

        #endregion HEADER BUTTONS INTERACTION METHODS

        #region PAGE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked during loading page. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _processesDataContext.LoadProcesses();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked during unloading page. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _processesDataContext.StopLoadingProcesses();
        }

        #endregion PAGE METHODS

        #region PROCESSES ITEMS MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking ProcessClose ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void ProcessCloseButtonExClick(object sender, RoutedEventArgs e)
        {
            //
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking ProcessKill ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void ProcessKillButtonExClick(object sender, RoutedEventArgs e)
        {
            //
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking ProcessWindows ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void ProcessWindowsButtonExClick(object sender, RoutedEventArgs e)
        {
            //
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after changing selection of ProcessItem. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Selection Changed Event Arguments. </param>
        private void ProcessesListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListViewEx listViewEx && listViewEx.SelectedItem != null)
            {
                listViewEx.SelectedItem = null;
            }
        }

        #endregion PROCESSES ITEMS MANAGEMENT METHODS

    }
}
