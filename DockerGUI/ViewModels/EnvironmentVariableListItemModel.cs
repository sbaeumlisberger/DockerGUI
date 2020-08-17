using System;
using System.Collections.Generic;
using System.Text;

namespace DockerGUI.ViewModels
{
    public class EnvironmentVariableListItemModel
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}
