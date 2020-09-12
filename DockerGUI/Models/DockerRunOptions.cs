using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DockerGUI.Models
{
    public class DockerRunOptions
    {
        public string ContainerName { get; set; }

        public IEnumerable<PortBinding> PortBindings { get; set; }

        public IDictionary<string, string> EnvironmentVariables { get; set; }

        public bool RestartAutomatically { get; set; }

        public string AdditionalOptions { get; set; }

        public override string ToString()
        {
            List<string> options = new List<string>();

            if (!string.IsNullOrEmpty(ContainerName))
            {
                options.Add($"--name \"{ContainerName}\"");
            }

            if (PortBindings.Any())
            {
                options.AddRange(PortBindings.Select(portBinding => $"-p {portBinding.HostPort}:{portBinding.ContainerPort}"));
            }

            if (EnvironmentVariables.Any())
            {
                options.AddRange(EnvironmentVariables.Select(environmentVariable =>
                {
                    if (string.IsNullOrEmpty(environmentVariable.Value))
                    {
                        return $"-e {environmentVariable.Key}";
                    }
                    return $"-e {environmentVariable.Key}={environmentVariable.Value}";
                }));
            }

            if (RestartAutomatically)
            {
                options.Add("--restart=unless-stopped");
            }

            if (!string.IsNullOrEmpty(AdditionalOptions))
            {
                options.Add(AdditionalOptions);
            }

            return string.Join(" ", options);
        }
    }
}
