using System;
using System.Collections.Generic;
using System.Text;

namespace DockerGUI.Models
{
    public class ImageSearchResult
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stars { get; set; }
        public bool IsOffical { get; set; }
        public bool IsAutomated { get; set; }
    }
}
