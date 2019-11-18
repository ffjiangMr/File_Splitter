namespace FileSplitter
{
    #region using directive

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    #endregion
    internal sealed class ExecuteCommands : ICommand
    {
        private static ExecuteCommands execute = new ExecuteCommands();

        public static ExecuteCommands ExeCommand { get { return execute; } }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {            

            return true;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("");
           // throw new NotImplementedException();
        }
    }
}
