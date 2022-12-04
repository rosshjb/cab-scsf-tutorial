using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.WinForms;
using System;

namespace command_handler_parameters
{
    internal class Program : FormShellApplication<WorkItem, Form1>
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            new Program().Run();
        }

        protected override void AfterShellCreated()
        {
            base.AfterShellCreated();

            RootWorkItem.Commands["ShowMessageCommand"]
                .AddInvoker(Shell.commandButton, "Click");
        }
    }
}
