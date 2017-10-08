using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using TreeWPF.Annotations;
using TreeWPF.Commands;
using TreeWPF.Interfaces;

namespace TreeWPF.ViewModels
{
    public class MessageDialogViewModel : ViewModelBase, IClosableViewModel
    {
        #region Fields

        private string _message;

        public event EventHandler Close;

        #endregion

        #region Properties

        public string Message
        {
            get { return _message; }
            set
            {

                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public ActionCommand CloseWindowCommand { get; set; }

        public ActionCommand DoSomeActionCommand { get; set; }

        #endregion

        #region Ctor

        public MessageDialogViewModel() : this(null)
        {}

        public MessageDialogViewModel(string message)
        {
            CloseWindowCommand = new ActionCommand(CloseView);
            Message = message;
        }

        #endregion

        protected virtual void CloseView(object parameter)
        {
            var handler = Close;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}