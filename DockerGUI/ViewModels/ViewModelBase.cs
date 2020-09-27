using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using MessageBox.Avalonia.DTO;
using ReactiveUI;

namespace DockerGUI.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public static event EventHandler<DialogRequestedEventArgs> DialogRequested;

        protected Task ShowMessageDialogAsync(string title, string message)
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var bitmap = new Bitmap(assets.Open(new Uri("avares://DoсkerGUI/Assets/icon.png")));
            var ms = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                
                WindowIcon = new WindowIcon(bitmap),
            });
            return ms.Show();
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
