using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using Avalonia.Threading;
using ReactiveUI;

namespace DockerGUI.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public static event EventHandler<MessageDialogModel> MessageDialogRequested;

        protected void ShowMessageDialog(string title, string message)
        {
            MessageDialogRequested.Invoke(this, new MessageDialogModel(title, message));
        }

        protected void RunOnUIThread(Action action)
        {
            if (Dispatcher.UIThread.CheckAccess())
            {
                action();                
            }
            else
            {
                Dispatcher.UIThread.Post(action);
            }
        }
    }
}
