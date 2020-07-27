using System;
using System.Collections.Generic;
using System.Text;

namespace DockerGUI.Models
{
    public class PortBinding
    {
        public int ContainerPort { get; set; }
        public int HostPort { get; set; }
    }
}
