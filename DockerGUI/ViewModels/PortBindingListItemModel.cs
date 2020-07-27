using DockerGUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DockerGUI.ViewModels
{
    public class PortBindingListItemModel
    {
        public string ContainerPort { get; set; }
        public string HostPort { get; set; }

        public bool IsValid()
        {
            return int.TryParse(ContainerPort, out _) && int.TryParse(HostPort, out _);
        }

        public PortBinding ToPortBinding()
        {
            if (!IsValid()) 
            {
                throw new InvalidOperationException();
            }
            return new PortBinding()
            {
                ContainerPort = int.Parse(ContainerPort),
                HostPort = int.Parse(HostPort)
            };
        }
    }
}
