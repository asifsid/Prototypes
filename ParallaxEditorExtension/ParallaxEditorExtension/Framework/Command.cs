using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ParallaxEditorExtension
{
    public class Command<ParamT> : ICommand
    {
        protected Action<ParamT> _onExecute;
        protected Func<ParamT, bool> _onCanExecute;
        private EventHandler _canExecuteChangedHandler;

        public event EventHandler CanExecuteChanged
        {
            add { _canExecuteChangedHandler += value; }
            remove { _canExecuteChangedHandler -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            EventHandler handler = _canExecuteChangedHandler;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return _onCanExecute((ParamT)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            _onExecute((ParamT)parameter);
        }

        public static ICommand Create(Action<ParamT> execute, Func<ParamT, bool> canExecute)
        {
            return new Command<ParamT>()
            {
                _onExecute = execute,
                _onCanExecute = canExecute
            };
        }
        public static ICommand Create(Action<ParamT> execute)
        {
            return Create(execute, (ignore) => true);
        }
    }

    class Command : Command<object>
    {
        public static ICommand Create(Action execute, Func<bool> canExecute)
        {
            return Command<object>.Create((ignore) => execute(), (ignore) => canExecute());
        }

        public static ICommand Create(Action execute)
        {
            return Command<object>.Create((ignore) => execute());
        }
    }
}
