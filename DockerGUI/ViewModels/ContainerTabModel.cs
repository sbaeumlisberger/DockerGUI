using DockerGUI.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task RefreshContainersAsync()
        {
            try
            {
                Containers = (await dockerCommandService.GetContainersAsync())
                    .Select(containerInfo => new ContainerDataGridItemModel(this, dockerCommandService, containerInfo))
                    .ToList();
                this.RaisePropertyChanged(nameof(Containers));
            }
            catch (Exception exception)
            {
                await ShowMessageDialogAsync("Could not retrieve containers", exception.Message);
            }
        }

    }
}
