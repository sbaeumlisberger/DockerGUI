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

        public static WindowIcon icon;

        static ViewModelBase()
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var stream = assets.Open(new Uri("avares://DockerGUI/Assets/icon.png"));
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Position = 0;
            var bitmap = new Bitmap(memoryStream);
            icon = new WindowIcon(bitmap);
        }

        protected Task ShowMessageDialogAsync(string title, string message)
        {
            var ms = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ContentTitle = title,
                ContentHeader = title,
                ContentMessage = message,
                WindowIcon = icon,
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