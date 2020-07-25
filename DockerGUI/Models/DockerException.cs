using System;
using System.Collections.Generic;
using System.Text;

namespace DockerGUI.Models
{
    public class DockerException : Exception
    {
        public int ExitCode;

        public DockerException(string message, int exitCode) : base(message)
        {
            ExitCode = exitCode;
        }
    }
}
