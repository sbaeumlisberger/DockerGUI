using DockerGUI.Utils;
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
            var processStartInfo = CreateProcessStartInfo(command);
            using var process = Process.Start(processStartInfo);
            Executed?.Invoke(this, $"docker {command}");

            string output = await process.StandardOutput.ReadToEndAsync();
            if (!string.IsNullOrEmpty(output))
            {
                Output?.Invoke(this, output);
            }

            string error = await process.StandardError.ReadToEndAsync();
            if (!string.IsNullOrEmpty(error))
            {
                Error?.Invoke(this, error);
            }

            int exitCode = await process.WaitForExitAsync().ConfigureAwait(false);
            if (exitCode != 0)
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
