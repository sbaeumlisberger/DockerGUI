using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;

namespace DockerGUI.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public static event EventHandler<DialogRequestedEventArgs> DialogRequested;

        protected Task ShowMessageDialogAsync(string title, string message)
        {
            var messageDialogModel = new MessageDialogModel(title, message);
            var eventArgs = new DialogRequestedEventArgs(messageDialogModel);
            DialogRequested.Invoke(this, eventArgs);
            return eventArgs.CompletionTask;
        }

        protected Task ShowDialogAsync(ViewModelBase viewModel)
        {
            var eventArgs = new DialogRequestedEventArgs(viewModel);
            DialogRequested.Invoke(this, eventArgs);
            return eventArgs.CompletionTask;
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
