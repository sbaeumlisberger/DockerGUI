using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Threading;
using DockerGUI.ViewModels;
using DockerGUI.Views;
using System;

namespace DockerGUI
{
    public class App : Application
    {
        private readonly OSThemeService osThemeService = new OSThemeService();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            if (osThemeService.IsDarkThemeEnabled)
            {
                Styles.Add(new StyleInclude(new Uri("resm:Styles?assembly=ControlCatalog"))
                {
                    Source = new Uri("resm:Avalonia.Themes.Default.DefaultTheme.xaml?assembly=Avalonia.Themes.Default")
                });
                Styles.Add(new StyleInclude(new Uri("resm:Styles?assembly=ControlCatalog"))
                {
                    Source = new Uri("resm:Avalonia.Themes.Default.Accents.BaseDark.xaml?assembly=Avalonia.Themes.Default")
                });
            }
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
