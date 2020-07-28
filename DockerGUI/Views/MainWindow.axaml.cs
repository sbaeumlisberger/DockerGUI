using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DockerGUI.ViewModels;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DockerGUI.Views
{
    public class MainWindow : Window
    {
        private MainWindowModel ViewModel => (MainWindowModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            ViewModelBase.DialogRequested += ViewModelBase_DialogRequested;

            DataContextChanged += MainWindow_DataContextChanged;
        }

        private void ViewModelBase_DialogRequested(object sender, DialogRequestedEventArgs args)
        {
            var dialog = (Window)new ViewLocator().Build(args.ViewModel);
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.DataContext = args.ViewModel;

            if (Dispatcher.UIThread.CheckAccess())
            {
                args.CompletionTask = dialog.ShowDialog(this);
            }
            else
            {
                Dispatcher.UIThread.Post(() =>
                {
                    args.CompletionTask = dialog.ShowDialog(this);
                });
            }
        }

        private void MainWindow_DataContextChanged(object sender, EventArgs e)
        {
            ViewModel.LogEntries.CollectionChanged += LogEntries_CollectionChanged;
        }

        private async void LogEntries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var logListBox = this.FindControl<ListBox>("logListBox");
            if (logListBox.Scroll != null)
            {
                try
                {
                    await Task.Delay(10);
                    logListBox.Scroll.Offset = new Vector(0, logListBox.Scroll.Extent.Height - logListBox.Scroll.Viewport.Height);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                }
            }
        }

        private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (DataContext is null)
            {
                return;
            }
            int tabIndex = ((TabControl)sender).SelectedIndex;
            if (tabIndex == 0)
            {
                await ViewModel.ContainerTabModel.RefreshContainersAsync();
            }
            else if (tabIndex == 1)
            {
                await ViewModel.ImagesTabModel.RefreshImagesAsync();
            }
        }

        private async void CommandTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var commandTextBox = (TextBox)sender;
                await ViewModel.ExecuteCommandAsync(commandTextBox.Text);
                commandTextBox.Text = string.Empty;
            }
        }
    }
}
