using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DockerGUI.ViewModels;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using System;
using System.Collections.Specialized;
using System.Linq;
using IconEnum = MessageBox.Avalonia.Enums.Icon;

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

            ViewModelBase.MessageDialogRequested += (object sender, MessageDialogModel dialogModel) =>
            {
                if (Dispatcher.UIThread.CheckAccess())
                {
                    ShowMessageDialog(dialogModel.Title, dialogModel.Message);
                }
                else
                {
                    Dispatcher.UIThread.Post(() => ShowMessageDialog(dialogModel.Title, dialogModel.Message));
                }
            };

            DataContextChanged += MainWindow_DataContextChanged;

            var tabControl = this.FindControl<TabControl>("tabControl");
            tabControl.SelectionChanged += TabControl_SelectionChanged;

            var commandTextBox = this.FindControl<TextBox>("commandTextBox");
            commandTextBox.KeyUp += CommandTextBox_KeyUp;

            var searchTextBox = this.FindControl<TextBox>("searchTextBox");
            searchTextBox.KeyUp += SearchTextBox_KeyUp;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (((TabControl)sender).SelectedIndex == 0)
            {
                ViewModel.ContainerTabModel.RefreshContainers();
            }
            else
            {
                ViewModel.ImagesTabModel.RefreshImages();
            }
        }

        private void ShowMessageDialog(string title, string message)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams()
            {
                ContentTitle = title,
                ContentHeader = title,
                ContentMessage = message,
                ButtonDefinitions = ButtonEnum.Ok,
                Icon = IconEnum.None,
                // MaxWidth = 600
            });
            messageBox.Show();
        }

        private void MainWindow_DataContextChanged(object sender, EventArgs e)
        {
            ViewModel.LogEntries.CollectionChanged += LogEntries_CollectionChanged;
        }

        private void CommandTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var commandTextBox = (TextBox)sender;
                ViewModel.ExecuteCommand(commandTextBox.Text);
            }
        }


        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var searchTextBox = (TextBox)sender;
                ViewModel.DockerHubTabModel.Search(searchTextBox.Text);
            }
        }

        private void LogEntries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var logListBox = this.FindControl<ListBox>("logListBox");
            if (logListBox.Scroll != null)
            {
                logListBox.Scroll.Offset = new Vector(0, logListBox.Scroll.Extent.Height - logListBox.Scroll.Viewport.Height);
            }
        }
    }
}
