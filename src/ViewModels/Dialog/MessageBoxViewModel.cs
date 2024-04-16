using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace TVPlayer.ViewModels.Dialog
{
    public class MessageBoxViewModel : BindableBase, IDialogAware
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(); }
        }

        private string _msg;

        public string Msg
        {
            get { return _msg; }
            set { _msg = value; RaisePropertyChanged(); }
        }


        public event Action<IDialogResult> RequestClose;
        public DelegateCommand CloseWindowCommand { get; set; }
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("Title");
            Msg = parameters.GetValue<string>("Msg");
            CloseWindowCommand = new DelegateCommand(OnDialogClosed);

        }
    }
}
