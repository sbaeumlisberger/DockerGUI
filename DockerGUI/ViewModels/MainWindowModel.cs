using DockerGUI.Models;
using DockerGUI.Utils;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerGUI.ViewModels
{
    public class MainWindowModel : ViewModelBase
    {
        public string GUIVersion => "DockerGUI version 0.1";

        public string DockerVersion { get; }

        public ContainerTabModel ContainerTabModel { get; }

        public ImagesTabModel ImagesTabModel { get; }

        public DockerHubTabModel DockerHubTabModel { get; }

        public ObservableCollection<string> LogEntries { get; } = new ObservableCollection<string>();

        private readonly DockerExecutableService dockerExecutableService;

        private readonly DockerCommandService dockerCommandService;

        public MainWindowModel()
        {
            dockerExecutableService = new DockerExecutableService();
            dockerCommandService = new DockerCommandService(dockerExecutableService);

            dockerExecutableService.Executed += DockerExecutableService_Executed;
            dockerExecutableService.Output += DockerExecutableService_Out;
            dockerExecutableService.Error += DockerExecutableService_Error;

            ContainerTabModel = new ContainerTabModel(dockerCommandService);
            ImagesTabModel = new ImagesTabModel(dockerCommandService);
            DockerHubTabModel = new DockerHubTabModel(dockerCommandService);

            try
            {
                DockerVersion = dockerCommandService.GetVersion();
            }
            catch (Exception exception)
            {
                this.Log().Error(exception, "Could not retrieve docker version.");
            }

            ContainerTabModel.RefreshContainers();
        }

        private void DockerExecutableService_Executed(object sender, string command)
        {
            RunOnUIThread(() => LogEntries.Add(command));
        }

        private void DockerExecutableService_Out(object sender, string output)
        {
            RunOnUIThread(() => LogEntries.Add(output.RemovePostfix("\n")));
        }

        private void DockerExecutableService_Error(object sender, string error)
        {
            RunOnUIThread(() => LogEntries.Add(error.RemovePostfix("\n")));
        }

        public void ExecuteCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                return;
            }
            if (command.StartsWith("docker"))
            {
                command = command.Substring(6);
            }
            dockerExecutableService.Execute(command);
        }

        public async Task Copy(string text)
        {
            await App.Current.Clipboard.SetTextAsync(text);
        }

    }
}
