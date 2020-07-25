using DockerGUI.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void RefreshImages()
        {
            try
            {
                Images = dockerCommandService.GetImages()
                    .Select(imageInfo => new ImageDataGridItemModel(this, dockerCommandService, imageInfo))
                    .ToList();
                this.RaisePropertyChanged(nameof(Images));
            }
            catch (Exception exception)
            {
                ShowMessageDialog("Could not retrieve images", exception.Message);
            }
        }

    }
}
