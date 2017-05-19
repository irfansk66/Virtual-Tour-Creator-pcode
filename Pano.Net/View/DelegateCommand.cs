using System;
using System.Windows.Input;

namespace Pano.Net.View
{
    internal class DelegateCommand<T> : ICommand
    {
        private Action<object> funcToCall;
        private Func<object, bool> funcToEvaluate;

        public DelegateCommand(Action<object> funcToCall, Func<object, bool> funcToEvaluate)
        {
            this.funcToCall = funcToCall;
            this.funcToEvaluate = funcToEvaluate;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}