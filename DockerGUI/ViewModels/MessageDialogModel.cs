using System;
using System.Collections.Generic;
using System.Text;

namespace DockerGUI.ViewModels
{
    public class MessageDialogModel
    {
        public string Title { get; }
        public string Message { get; }

        public MessageDialogModel(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
