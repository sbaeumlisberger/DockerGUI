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

        public string GetVersion()
        {
            return dockerExecutableService.Execute("-v").Trim();
        }

        public IList<DockerImageInfo> GetImages()
        {
            string result = dockerExecutableService.Execute("images");
            return ParseTable(result, new[] { "REPOSITORY", "TAG", "IMAGE ID", "CREATED", "SIZE" })
                 .Select(row => ParseDockerImageInfo(row))
                 .ToList();
        }

        public async Task PullImageAsync(string name)
        {
            await Task.Run(() =>
            {
                dockerExecutableService.Execute($"pull {name}");
            });
        }

        public void RemoveImage(string imageID)
        {
            dockerExecutableService.Execute($"rmi {imageID}");
        }

        public void RunImage(string imageID)
        {            
            dockerExecutableService.Execute($"run {imageID}");
        }

        public IList<DockerContainerInfo> GetContainers()
        {
            string result = dockerExecutableService.Execute("ps -a");
            return ParseTable(result, new[] { "CONTAINER ID", "IMAGE", "COMMAND", "CREATED", "STATUS", "PORTS", "NAMES" })
                 .Select(row => ParseDockerContainerInfo(row))
                 .ToList();
        }

        public void StartContainer(string containerID)
        {
            dockerExecutableService.Execute($"start -a {containerID}");
        }

        public void StopContainer(string containerID)
        {
            dockerExecutableService.Execute($"stop {containerID}");
        }

        internal void RestartContainer(string containerID)
        {
            dockerExecutableService.Execute($"restart {containerID}");
        }

        public void RemoveContainer(string containerID)
        {
            dockerExecutableService.Execute($"rm {containerID}");
        }

        public void RenameContainer(string containerID, string newName)
        {
            dockerExecutableService.Execute($"rename {containerID} \"{newName}\"");
        }

        public async Task CommitContainerAsync(string containerID, string repository = null, string tag = null)
        {
            if (tag != null && repository is null)
            {
                throw new ArgumentException($"When the parameter '{nameof(tag)}' is not null the parameter '{nameof(repository)}' must not be null.");
            }
            await Task.Run(() =>
            {
                string command = $"commit {containerID}";
                if (repository != null)
                {
                    command += $" {repository}";
                }
                if (tag != null)
                {
                    command += $":{tag}";
                }
                dockerExecutableService.Execute(command);
            });
        }

        public IList<ImageSearchResult> SearchDockerHub(string query)
        {
            string result = dockerExecutableService.Execute($"search --no-trunc {query}");
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
