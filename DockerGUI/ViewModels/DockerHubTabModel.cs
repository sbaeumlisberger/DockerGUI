using DockerGUI.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerGUI.ViewModels
{
    public class DockerHubTabModel : ViewModelBase
    {
        public IList<ImageSearchResultDataGridItemModel> SearchResults { get; private set; } = new List<ImageSearchResultDataGridItemModel>();

        private readonly DockerCommandService dockerCommandService;

        public DockerHubTabModel(DockerCommandService dockerCommandService)
        {
            this.dockerCommandService = dockerCommandService;
        }

        public async Task Search(string query)
        {
            try
            {
                SearchResults = (await dockerCommandService.SearchDockerHubAsync(query))
                    .Select(searchResult => new ImageSearchResultDataGridItemModel(dockerCommandService, searchResult))
                    .ToList();
                this.RaisePropertyChanged(nameof(SearchResults));
            }
            catch (Exception exception)
            {
                await ShowMessageDialogAsync("Could not retrieve search results.", exception.Message);
            }
        }
    }
}
