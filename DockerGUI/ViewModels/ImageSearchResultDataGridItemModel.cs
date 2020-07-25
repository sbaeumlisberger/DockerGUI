using DockerGUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DockerGUI.ViewModels
{
    public class ImageSearchResultDataGridItemModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stars { get; set; }
        public bool IsOffical { get; set; }
        public bool IsAutomated { get; set; }

        private readonly DockerCommandService dockerCommandService;

        public ImageSearchResultDataGridItemModel(DockerCommandService dockerCommandService, ImageSearchResult searchResult)
        {
            this.dockerCommandService = dockerCommandService;
            Name = searchResult.Name;
            Description = searchResult.Description;
            Stars = searchResult.Stars;
            IsOffical = searchResult.IsOffical;
            IsAutomated = searchResult.IsAutomated;
        }

        public async System.Threading.Tasks.Task Pull()
        {
            try
            {
                await dockerCommandService.PullImageAsync(Name);
            }
            catch (Exception exception)
            {
                ShowMessageDialog("Could not pull image", exception.Message);
            }
        }
    }
}
