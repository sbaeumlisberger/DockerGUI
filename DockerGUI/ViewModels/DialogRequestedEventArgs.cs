using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DockerGUI.ViewModels
{
    public class DialogRequestedEventArgs
    {    
        public ViewModelBase ViewModel { get; }

        public Task CompletionTask { get; set; }

        public DialogRequestedEventArgs(ViewModelBase viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
