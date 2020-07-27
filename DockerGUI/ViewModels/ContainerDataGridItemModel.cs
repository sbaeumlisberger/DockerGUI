using DockerGUI.Models;
using DynamicData.Kernel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DockerGUI.ViewModels
{
    public class ContainerDataGridItemModel : ViewModelBase
    {
        public string ID { get; }
        public string ImageID { get; }
        public string Command { get; }
        public string Created { get; }
        public string Status { get; }
        public string Ports { get; }
        public string Name { get => name; set => Rename(value); }

        public bool IsRunning => !Status.Contains("Exited");

        private string name;

        private readonly ContainerTabModel containerTabModel;

        private readonly DockerCommandService dockerCommandService;

        public ContainerDataGridItemModel(ContainerTabModel containerTabModel, DockerCommandService dockerCommandService, DockerContainerInfo containerinfo)
        {
            this.containerTabModel = containerTabModel;
            this.dockerCommandService = dockerCommandService;
            ID = containerinfo.ID;
            ImageID = containerinfo.ImageID;
            Command = containerinfo.Command;
            Created = containerinfo.Created;
            Status = containerinfo.Status;
            Ports = containerinfo.Ports;
            name = containerinfo.Names;
        }

        public async Task Rename(string newName)
        {
            try
            {
                await dockerCommandService.RenameContainerAsync(ID, newName);
                name = newName;
            }
            catch (Exception exception)
            {
                ShowMessageDialog("Could not rename container", exception.Message);
            }
        }

        public async Task Start()
        {
            try
            {
                await dockerCommandService.StartContainerAsync(ID);
                containerTabModel.RefreshContainers();
            }
            catch (Exception exception)
            {
                ShowMessageDialog("Could not start container", exception.Message);
            }
        }

        public async Task Stop()
        {
            try
            {
                await dockerCommandService.StopContainerAsync(ID);
                containerTabModel.RefreshContainers();
            }
            catch (Exception exception)
            {
                ShowMessageDialog("Could not stop container", exception.Message);
            }
        }

        public async Task Restart()
        {
            try
            {
                await dockerCommandService.RestartContainerAsync(ID);
                containerTabModel.RefreshContainers();
            }
            catch (Exception exception)
            {
                ShowMessageDialog("Could not restart container", exception.Message);
            }
        }

        public async Task Remove()
        {
            try
            {
                await dockerCommandService.RemoveContainerAsync(ID);
                containerTabModel.RefreshContainers();
            }
            catch (Exception exception)
            {
                ShowMessageDialog("Could not remove container", exception.Message);
            }
        }

        public async Task Commit()
        {
            try
            {
                await dockerCommandService.CommitContainerAsync(ID);
            }
            catch (Exception exception)
            {
                ShowMessageDialog("Could not commit container", exception.Message);
            }
        }

        public void Run()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var processStartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C docker exec -it {ID} bash"
                };
                Process.Start(processStartInfo);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var processStartInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash ",
                    Arguments = $"-c \"docker exec -it {ID} bash\""
                };
                Process.Start(processStartInfo);
            }
            else
            {
                ShowMessageDialog("Operating system not supported", "");
            }
        }

    }
}
