using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.WinForms;
using System;
using System.Windows.Forms;

namespace command_handler_usage
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

            Command command = RootWorkItem.Commands["ShowMessageCommand"];

            ToolStripItem toolStripItemForCommand = this.Shell.toolStrip1.Items["button1"];

            command.AddInvoker(toolStripItemForCommand, "Click");

            printRootWorkItemCollections();
        }

        /// <summary>
        /// Services:<br/>
        /// [Microsoft.Practices.CompositeUI.WorkItem, Microsoft.Practices.CompositeUI.WorkItem]<br/>
        /// [Microsoft.Practices.CompositeUI.Services.ICryptographyService, Microsoft.Practices.CompositeUI.Collections.ServiceCollection+DemandAddPlaceholder]<br/>
        /// [Microsoft.Practices.CompositeUI.ITraceSourceCatalogService, Microsoft.Practices.CompositeUI.TraceSourceCatalogService]<br/>
        /// [Microsoft.Practices.CompositeUI.Services.IWorkItemExtensionService, Microsoft.Practices.CompositeUI.Services.WorkItemExtensionService]<br/>
        /// [Microsoft.Practices.CompositeUI.Services.IWorkItemTypeCatalogService, Microsoft.Practices.CompositeUI.Services.WorkItemTypeCatalogService]<br/>
        /// [Microsoft.Practices.CompositeUI.IWorkItemActivationService, Microsoft.Practices.CompositeUI.SimpleWorkItemActivationService]<br/>
        /// [Microsoft.Practices.CompositeUI.Services.IAuthenticationService, Microsoft.Practices.CompositeUI.Services.WindowsPrincipalAuthenticationService]<br/>
        /// [Microsoft.Practices.CompositeUI.Services.IModuleLoaderService, Microsoft.Practices.CompositeUI.Services.ModuleLoaderService]<br/>
        /// [Microsoft.Practices.CompositeUI.Services.IModuleEnumerator, Microsoft.Practices.CompositeUI.Services.FileCatalogModuleEnumerator]<br/>
        /// [Microsoft.Practices.CompositeUI.Commands.ICommandAdapterMapService, Microsoft.Practices.CompositeUI.Commands.CommandAdapterMapService]<br/>
        /// [Microsoft.Practices.CompositeUI.UIElements.IUIElementAdapterFactoryCatalog, Microsoft.Practices.CompositeUI.UIElements.UIElementAdapterFactoryCatalog]<br/>
        /// [Microsoft.Practices.CompositeUI.WinForms.IControlActivationService, Microsoft.Practices.CompositeUI.WinForms.ControlActivationService]<br/>
        /// Items:<br/>
        /// [6f104598-8444-4ae7-b586-ecb69eb8fd99, Microsoft.Practices.CompositeUI.State]<br/>
        /// [d8e1d611-28b4-400e-be28-2dca7f3ce27f, command_handler_usage.Form1, Text: Form1]<br/>
        /// [ShowMessageCommand, Microsoft.Practices.CompositeUI.Commands.Command]<br/>
        /// Commands:<br/>
        /// [ShowMessageCommand, Microsoft.Practices.CompositeUI.Commands.Command]<br/>
        /// </summary>
        private void printRootWorkItemCollections()
        {
            Console.WriteLine("Services:");
            foreach (var item in RootWorkItem.Services)
                Console.WriteLine(item);

            Console.WriteLine("Items:");
            foreach (var item in RootWorkItem.Items)
                Console.WriteLine(item);

            Console.WriteLine("Commands:");
            foreach (var item in RootWorkItem.Commands)
                Console.WriteLine(item);
        }
    }
}
