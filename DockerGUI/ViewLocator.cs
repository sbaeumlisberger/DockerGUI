using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using DockerGUI.ViewModels;

namespace DockerGUI
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object viewModel)
        {
            var name = viewModel.GetType().FullName.Replace("Model", "");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }
            else
            {
                throw new Exception($"No view found for view model of type '{viewModel.GetType().FullName}'");
            }
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}