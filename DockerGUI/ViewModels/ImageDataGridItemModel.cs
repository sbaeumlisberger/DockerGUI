using DockerGUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

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

        public void Remove() 
        {
            try
            {
                dockerCommandService.RemoveImage(ID);
                imagesTabModel.RefreshImages();
            }
            catch (Exception exception)
            {
                ShowMessageDialog("Could not remove image", exception.Message);
            }
        }

        public void Run()
        {
            try
            {
                dockerCommandService.RunImage(ID);
            }
            catch (Exception exception)
            {
                ShowMessageDialog("Could not run image", exception.Message);
            }
        }

    }
}
