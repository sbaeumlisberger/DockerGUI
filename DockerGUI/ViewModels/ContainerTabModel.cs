using DockerGUI.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockerGUI.ViewModels
{
    public class ContainerTabModel : ViewModelBase
    {
        public IList<ContainerDataGridItemModel> Containers { get; private set; } = new List<ContainerDataGridItemModel>();
        
        private readonly DockerCommandService dockerCommandService;

        public ContainerTabModel(DockerCommandService dockerCommandService)
        {
            this.dockerCommandService = dockerCommandService;
        }

        public void RefreshContainers()
        {
            try
            {
                Containers = dockerCommandService.GetContainers()
                    .Select(containerInfo => new ContainerDataGridItemModel(this, dockerCommandService, containerInfo))
                    .ToList();
                this.RaisePropertyChanged(nameof(Containers));
            }
            catch (Exception exception)
            {
                ShowMessageDialog("Could not retrieve containers", exception.Message);
            }
        }

    }
}
