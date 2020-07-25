using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using DockerGUI.ViewModels;

namespace DockerGUI.Views
{
    public class DockerHubTab : UserControl
    {
        private DockerHubTabModel ViewModel => (DockerHubTabModel)DataContext;

        public DockerHubTab()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            var searchTextBox = this.FindControl<TextBox>("searchTextBox");
            searchTextBox.KeyUp += SearchTextBox_KeyUp;
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var searchTextBox = (TextBox)sender;
                ViewModel.Search(searchTextBox.Text);
            }
        }
    }
}
