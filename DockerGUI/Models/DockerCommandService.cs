using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DockerGUI.Models
{
    public class DockerCommandService
    {

        private readonly DockerExecutableService dockerExecutableService;

        public DockerCommandService(DockerExecutableService dockerExecutableService)
        {
            this.dockerExecutableService = dockerExecutableService;
        }

        public async Task<string> GetVersionAsync()
        {
            return (await dockerExecutableService.ExecuteAsync("-v").ConfigureAwait(false)).Trim();
        }

        public async Task<IList<DockerImageInfo>> GetImagesAsync()
        {
            string result = await dockerExecutableService.ExecuteAsync("images").ConfigureAwait(false);
            return ParseTable(result, new[] { "REPOSITORY", "TAG", "IMAGE ID", "CREATED", "SIZE" })
                 .Select(row => ParseDockerImageInfo(row))
                 .ToList();
        }

        public async Task PullImageAsync(string name)
        {
            await dockerExecutableService.ExecuteAsync($"pull {name}").ConfigureAwait(false);
        }

        public async Task RemoveImageAsync(string imageID)
        {
            await dockerExecutableService.ExecuteAsync($"rmi {imageID}").ConfigureAwait(false);
        }

        public async Task RunImageAsync(string imageID, string containerName, IEnumerable<PortBinding> portBindings, string additionalOptions)
        {
            List<string> options = new List<string>();
            if (!string.IsNullOrEmpty(containerName))
            {
                options.Add($"--name \"{containerName}\"");
            }
            if (portBindings.Any())
            {
                options.Add("-p " + string.Join(" ", portBindings.Select(portBinding => $"{portBinding.HostPort}:{portBinding.ContainerPort}")));
            }
            if (!string.IsNullOrEmpty(additionalOptions))
            {
                options.Add(additionalOptions);
            }
            if (options.Any())
            {
                await dockerExecutableService.ExecuteAsync($"run {string.Join(" ", options)} {imageID}").ConfigureAwait(false);
            }
            else
            {
                await dockerExecutableService.ExecuteAsync($"run {imageID}").ConfigureAwait(false);
            }
        }

        public async Task<IList<DockerContainerInfo>> GetContainersAsync()
        {
            string result = await dockerExecutableService.ExecuteAsync("ps -a").ConfigureAwait(false);
            return ParseTable(result, new[] { "CONTAINER ID", "IMAGE", "COMMAND", "CREATED", "STATUS", "PORTS", "NAMES" })
                 .Select(row => ParseDockerContainerInfo(row))
                 .ToList();
        }

        public async Task StartContainerAsync(string containerID)
        {
            await dockerExecutableService.ExecuteAsync($"start -a {containerID}").ConfigureAwait(false);
        }

        public async Task StopContainerAsync(string containerID)
        {
            await dockerExecutableService.ExecuteAsync($"stop {containerID}").ConfigureAwait(false);
        }

        internal async Task RestartContainerAsync(string containerID)
        {
            await dockerExecutableService.ExecuteAsync($"restart {containerID}").ConfigureAwait(false);
        }

        public async Task RemoveContainerAsync(string containerID)
        {
            await dockerExecutableService.ExecuteAsync($"rm {containerID}").ConfigureAwait(false);
        }

        public async Task RenameContainerAsync(string containerID, string newName)
        {
            await dockerExecutableService.ExecuteAsync($"rename {containerID} \"{newName}\"").ConfigureAwait(false);
        }

        public async Task CommitContainerAsync(string containerID, string repository = null, string tag = null)
        {
            if (tag != null && repository is null)
            {
                throw new ArgumentException($"When the parameter '{nameof(tag)}' is not null the parameter '{nameof(repository)}' must not be null.");
            }
            string command = $"commit {containerID}";
            if (repository != null)
            {
                command += $" {repository}";
            }
            if (tag != null)
            {
                command += $":{tag}";
            }
            await dockerExecutableService.ExecuteAsync(command).ConfigureAwait(false);
        }

        public async Task<IList<ImageSearchResult>> SearchDockerHubAsync(string query)
        {
            string result = await dockerExecutableService.ExecuteAsync($"search --no-trunc {query}").ConfigureAwait(false);
            return ParseTable(result, new[] { "NAME", "DESCRIPTION", "STARS", "OFFICIAL", "AUTOMATED" })
                 .Select(row => ParseImageSearchResult(row))
                 .ToList();
        }

        private IList<string[]> ParseTable(string table, IEnumerable<string> headers)
        {
            var rows = table.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var headerRow = rows.First();
            var columnIndexes = headers.Select(header => headerRow.IndexOf(header)).ToArray();
            return rows.Skip(1).Select(row =>
            {
                string[] entries = new string[columnIndexes.Length];
                for (int i = 0; i < columnIndexes.Length; i++)
                {
                    if (i < columnIndexes.Length - 1)
                    {
                        int columnLength = columnIndexes[i + 1] - columnIndexes[i];
                        entries[i] = row.Substring(columnIndexes[i], columnLength).Trim();
                    }
                    else
                    {
                        entries[i] = row.Substring(columnIndexes[i]).Trim();
                    }
                }
                return entries;
            }).ToList();
        }

        private DockerImageInfo ParseDockerImageInfo(string[] row)
        {
            return new DockerImageInfo()
            {
                ID = row[2],
                Repository = row[0],
                Tag = row[1],
                Created = row[3],
                Size = row[4]
            };
        }

        private DockerContainerInfo ParseDockerContainerInfo(string[] row)
        {
            return new DockerContainerInfo()
            {
                ID = row[0],
                ImageID = row[1],
                Command = row[2],
                Created = row[3],
                Status = row[4],
                Ports = row[5],
                Names = row[6]
            };
        }

        private ImageSearchResult ParseImageSearchResult(string[] row)
        {
            return new ImageSearchResult()
            {
                Name = row[0],
                Description = row[1],
                Stars = int.Parse(row[2]),
                IsOffical = row[3].Contains("OK"),
                IsAutomated = row[4].Contains("OK")
            };
        }

    }
}
