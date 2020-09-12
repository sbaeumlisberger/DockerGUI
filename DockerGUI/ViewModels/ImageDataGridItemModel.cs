using DockerGUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerGUI.ViewModels
{
    public class ImageDataGridItemModel : ViewModelBase
    {
        public string ID { get; }
        public string Repository { get; }
        public string Tag { get; }
        public string Created { get; }
        public string Size { get; }

        private readonly DockerCommandService dockerCommandService;

        private readonly ImagesTabModel imagesTabModel;

        public ImageDataGridItemModel(ImagesTabModel imagesTabModel, DockerCommandService dockerCommandService, DockerImageInfo imageInfo)
        {
            this.imagesTabModel = imagesTabModel;
            this.dockerCommandService = dockerCommandService;
            ID = imageInfo.ID;
            Repository = imageInfo.Repository;
            Tag = imageInfo.Tag;
            Created = imageInfo.Created;
            Size = imageInfo.Size;
        }

        public async Task RemoveAsync() 
        {
            try
            {
                await dockerCommandService.RemoveImageAsync(ID);
                await imagesTabModel.RefreshImagesAsync();
            }
            catch (Exception exception)
            {
                await ShowMessageDialogAsync("Could not remove image", exception.Message);
            }
        }

        public async Task RunAsync()
        {
            try
            {
                var dialogModel = new CreateContainerDialogModel();
                await ShowDialogAsync(dialogModel);
                if (!dialogModel.IsCanceld)
                {
                    var portBindings = dialogModel.PortBindings
                        .Where(vm => vm.IsValid())
                        .Select(vm => vm.ToPortBinding());

                    var environmentVariables = dialogModel.EnvironmentVariables
                      .Where(vm => vm.IsValid())
                      .ToDictionary(vm => vm.Name, vm => vm.Value);

                    await dockerCommandService.RunImageAsync(ID, new DockerRunOptions() 
                    {
                        ContainerName = dialogModel.ContainerName,
                        PortBindings = portBindings,
                        EnvironmentVariables = environmentVariables,
                        AdditionalOptions = dialogModel.AdditionalOptions,
                        RestartAutomatically = dialogModel.RestartAutomatically
                    });
                }
            }
            catch (Exception exception)
            {
               await ShowMessageDialogAsync("Could not run image", exception.Message);
            }
        }

    }
}
