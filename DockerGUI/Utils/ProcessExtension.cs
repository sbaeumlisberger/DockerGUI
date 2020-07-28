using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DockerGUI.Utils
{
    public static class ProcessExtensions
    {
        public static async Task<int> WaitForExitAsync(this Process process)
        {
            if (process is null)
            {
                throw new ArgumentNullException(nameof(process));
            }                      

            var completionSource = new TaskCompletionSource<int>();

            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) =>
            {
                completionSource.TrySetResult(process.ExitCode);
            };

            if (process.HasExited)
            {
                return process.ExitCode;
            }

            return await completionSource.Task.ConfigureAwait(false);
        }
    }
}
