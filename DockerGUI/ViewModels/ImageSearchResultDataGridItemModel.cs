using DockerGUI.Models;
using DockerGUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task Pull()
        {
            try
            {
                await dockerCommandService.PullImageAsync(Name);
            }
            catch (Exception exception)
            {
                await ShowMessageDialogAsync("Could not pull image", exception.Message);
            }
        }

        public void ShowInBrowser()
        {
            BrowserUtil.OpenUrl("https://hub.docker.com/_/" + Name);
        }
    }
}
