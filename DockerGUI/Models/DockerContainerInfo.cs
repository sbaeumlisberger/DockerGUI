using System;
using System.Collections.Generic;
using System.Text;

namespace DockerGUI.Models
{
    public class DockerContainerInfo
    {
        public string ID { get; set; }
        public string ImageID { get; set; }
        public string Command { get; set; }
        public string Created { get; set; }
        public string Status { get; set; }
        public string Ports { get; set; }
        public string Names { get; set; }
    }
}
