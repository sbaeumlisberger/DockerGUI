using Microsoft.Win32;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace DockerGUI.Views
{
    public class OSThemeService : IEnableLogger
    {
        public bool IsDarkThemeEnabled
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return IsDarkThemeEnabled_Windows();
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return IsDarkThemeEnabled_Linux();
                }
                return false;
            }
        }

        private bool IsDarkThemeEnabled_Windows()
        {
            try
            {
                var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                return ((int)key.GetValue("AppsUseLightTheme")) == 0;
            }
            catch (Exception exception)
            {
                this.Log().Error(exception, "Could not retrieve theme.");
                return false;
            }
        }

        private bool IsDarkThemeEnabled_Linux()
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = "/bin/bash ",
                Arguments = $"-c \"gsettings set org.gnome.desktop.interface gtk-theme\"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.Default,
            };
            var process = Process.Start(processStartInfo);
            string output = process.StandardOutput.ReadToEnd();
            return output.Contains("dark");
        }

    }
}
