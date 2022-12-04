using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Collections;
using Microsoft.Practices.CompositeUI.WinForms;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace command_status
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

            showCollectionsFor(RootWorkItem);

            RootWorkItem.Commands["ShowMessageCommand"]
                .AddInvoker(Shell.showMessageButton, "Click");
            RootWorkItem.Commands["ChangeStatusCommand"]
                .AddInvoker(Shell.changeStatusButton, "Click");

            showCollectionsFor(RootWorkItem);
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
        /// [93fae4b1-5efb-4ef3-a3ff-b7c718d2ab88, Microsoft.Practices.CompositeUI.State]<br/>
        /// [7ad93de8-024a-4daa-b895-a44254330de8, command_status.Form1, Text: Form1]<br/>
        /// [ShowMessageCommand, Microsoft.Practices.CompositeUI.Commands.Command]<br/>
        /// [ChangeStatusCommand, Microsoft.Practices.CompositeUI.Commands.Command]<br/>
        /// Commands:<br/>
        /// [ShowMessageCommand, Microsoft.Practices.CompositeUI.Commands.Command]<br/>
        /// [ChangeStatusCommand, Microsoft.Practices.CompositeUI.Commands.Command]<br/>
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
        /// [93fae4b1-5efb-4ef3-a3ff-b7c718d2ab88, Microsoft.Practices.CompositeUI.State]<br/>
        /// [7ad93de8-024a-4daa-b895-a44254330de8, command_status.Form1, Text: Form1]<br/>
        /// [ShowMessageCommand, Microsoft.Practices.CompositeUI.Commands.Command]<br/>
        /// [ChangeStatusCommand, Microsoft.Practices.CompositeUI.Commands.Command]<br/>
        /// Commands:<br/>
        /// [ShowMessageCommand, Microsoft.Practices.CompositeUI.Commands.Command]<br/>
        /// [ChangeStatusCommand, Microsoft.Practices.CompositeUI.Commands.Command]<br/>
        /// </summary>
        /// <param name="workItem"></param>
        private void showCollectionsFor(WorkItem workItem)
        {
            showCollection(name: "Services", collection: workItem.Services);
            showCollection(name: "Items", collection: workItem.Items);
            showCollection(name: "Commands", collection: workItem.Commands);
        }

        private void showCollection(string name, ICollection collection)
        {
            Console.WriteLine($"{name}:");
            foreach (var item in collection)
                Console.WriteLine(item);
        }
    }
}
