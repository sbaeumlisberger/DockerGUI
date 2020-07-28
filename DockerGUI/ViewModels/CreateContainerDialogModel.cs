using DockerGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DockerGUI.ViewModels
{
    public class CreateContainerDialogModel : ViewModelBase
    {
        public string ContainerName { get; set; }

        public ObservableCollection<PortBindingListItemModel> PortBindings { get; } = new ObservableCollection<PortBindingListItemModel>();

        public string AdditionalOptions { get; set; }

        public bool IsCanceld { get; set; } = true;

        public void AddPortBinding()
        {
            PortBindings.Add(new PortBindingListItemModel());
        }

        public void RemovePortBinding(PortBindingListItemModel portBinding)
        {
            PortBindings.Remove(portBinding);
        }
    }
}
