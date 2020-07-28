using DockerGUI.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerGUI.ViewModels
{
    public class ImagesTabModel : ViewModelBase
    {
        public IList<ImageDataGridItemModel> Images { get; private set; } = new List<ImageDataGridItemModel>();

        private readonly DockerCommandService dockerCommandService;

        public ImagesTabModel(DockerCommandService dockerCommandService)
        {
            this.dockerCommandService = dockerCommandService;
        }

        public async Task RefreshImagesAsync()
        {
            try
            {
                Images = (await dockerCommandService.GetImagesAsync())
                    .Select(imageInfo => new ImageDataGridItemModel(this, dockerCommandService, imageInfo))
                    .ToList();
                this.RaisePropertyChanged(nameof(Images));
            }
            catch (Exception exception)
            {
                await ShowMessageDialogAsync("Could not retrieve images", exception.Message);
            }
        }

    }
}
