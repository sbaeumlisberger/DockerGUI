using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DockerGUI.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace DockerGUI.Views
{
    public class CreateContainerDialog : Window
    {
        private CreateContainerDialogModel ViewModel => (CreateContainerDialogModel)DataContext;

        public CreateContainerDialog()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void DockerDocumentationLink_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            Process.Start(new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = "https://docs.docker.com/engine/reference/commandline/run/"
            });
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.IsCanceld = false;
            Close();
        }

    }
}
