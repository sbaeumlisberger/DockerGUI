using System;
using System.Collections.Generic;
using System.Text;

namespace DockerGUI.Models
{
    public class DockerImageInfo
    {
        public string ID { get; set; }
        public string Repository { get; set; }
        public string Tag { get; set; }
        public string Created { get; set; }
        public string Size { get; set; }
    }
}
