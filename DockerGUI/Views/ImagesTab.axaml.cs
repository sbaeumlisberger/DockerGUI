using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DockerGUI.Views
{
    public class ImagesTab : UserControl
    {
        public ImagesTab()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
