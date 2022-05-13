
namespace WATO
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// This class represents a command, which is a i command, so it can be executed.
    /// It have an action, which will invoked, by the execution of the command. 
    /// </summary>
    [Serializable]
    public class Command : ICommand
    {
        /// <summary>
        /// The action, which will be invoked, when the
        /// command executes.
        /// </summary>
        private readonly Action<object> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="action">The action, which the command should invoke.</param>
        public Command(Action<object> action)
        {
            this.action = action ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Occurs when changes occur that affect the execution of the command.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can be executed in its current state.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        /// <returns>
        /// Whether the command can be executed or not.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the command and due to this, the action.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        public void Execute(object parameter)
        {
            this.action(parameter);
        }
    }
}
