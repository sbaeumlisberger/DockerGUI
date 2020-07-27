using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DockerGUI.Models
{
    public class DockerExecutableService
    {

        public event EventHandler<string> Executed;

        public event EventHandler<string> Output;

        public event EventHandler<string> Error;

        /// <exception cref="DockerException"/>
        public async Task<string> ExecuteAsync(string command)
        {
            return await Task.Run(() => Execute(command)).ConfigureAwait(false);
        }

        /// <exception cref="DockerException"/>
        public string Execute(string command)
        {
            var processStartInfo = CreateProcessStartInfo(command);
            var process = Process.Start(processStartInfo);
            Executed?.Invoke(this, $"docker {command}");
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(output))
            {
                Output?.Invoke(this, output);
            }
            if (!string.IsNullOrEmpty(error))
            {
                Error?.Invoke(this, error);
            }
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new DockerException(error, process.ExitCode);
            }
            return output;
        }

        private ProcessStartInfo CreateProcessStartInfo(string command)
        {
            string fileName;
            string arguments;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {

                fileName = "cmd.exe";
                arguments = $"/C docker {command}";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                fileName = "/bin/bash";
                arguments = $"-c \"docker {command.Replace("\"", "\\\"")}\"";
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
            return new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = arguments,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.Default,
                StandardErrorEncoding = Encoding.Default
            };
        }

    }
}
