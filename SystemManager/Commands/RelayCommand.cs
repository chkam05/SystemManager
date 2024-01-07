using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SystemManager.Commands
{
    public class RelayCommand : ICommand
    {

        //  ACTIONS

        public Action Action { get; private set; }


        //  EVENTS

        public event EventHandler? CanExecuteChanged;


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> RelayCommand class constructor. </summary>
        /// <param name="action"> Action to assign. </param>
        /// <exception cref="ArgumentNullException"> Parameter cannot be null. </exception>
        public RelayCommand(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action), $"{nameof(action)} cannot be null");

            this.Action = action;
        }

        #endregion CLASS METHODS

        #region EXECUTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Check if action can be executed. </summary>
        /// <param name="parameter"> Data used by the command. </param>
        /// <returns> True - if this command can be executed; False - otherwise. </returns>
        public bool CanExecute(object? parameter)
        {
            //  Always executable.
            return true;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Execute action. </summary>
        /// <param name="parameter"> Data used by the command. </param>
        public void Execute(object? parameter)
        {
            this.Action();
        }

        #endregion EXECUTION METHODS

    }
}
