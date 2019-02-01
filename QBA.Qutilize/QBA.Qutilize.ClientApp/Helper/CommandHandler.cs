using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QBA.Qutilize.ClientApp.Helper
{
    public class CommandHandler : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> _action;
        public CommandHandler(Action<object> action)
        {
            this._action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._action(parameter);
        }
    }
}
