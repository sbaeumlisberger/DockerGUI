using Microsoft.Win32;
using Splat;
using System;
using System.Collections.Generic;
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
                // TODO: support other platforms
                return false;
            }
        }

    }
}
