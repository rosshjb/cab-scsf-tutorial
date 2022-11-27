using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.WinForms;
using System;

namespace Shell
{
    public class Program : FormShellApplication<WorkItem, Form1>
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

            components.Service svc = RootWorkItem.Services.Get<components.Service>();
            svc.foobar();  // implementation

            // Services:
            // [Microsoft.Practices.CompositeUI.WorkItem, Microsoft.Practices.CompositeUI.WorkItem]
            // [Microsoft.Practices.CompositeUI.Services.ICryptographyService, Microsoft.Practices.CompositeUI.Collections.ServiceCollection+DemandAddPlaceholder]
            // [Microsoft.Practices.CompositeUI.ITraceSourceCatalogService, Microsoft.Practices.CompositeUI.TraceSourceCatalogService]
            // [Microsoft.Practices.CompositeUI.Services.IWorkItemExtensionService, Microsoft.Practices.CompositeUI.Services.WorkItemExtensionService]
            // [Microsoft.Practices.CompositeUI.Services.IWorkItemTypeCatalogService, Microsoft.Practices.CompositeUI.Services.WorkItemTypeCatalogService]
            // [Microsoft.Practices.CompositeUI.IWorkItemActivationService, Microsoft.Practices.CompositeUI.SimpleWorkItemActivationService]
            // [Microsoft.Practices.CompositeUI.Services.IAuthenticationService, Microsoft.Practices.CompositeUI.Services.WindowsPrincipalAuthenticationService]
            // [Microsoft.Practices.CompositeUI.Services.IModuleLoaderService, Microsoft.Practices.CompositeUI.Services.ModuleLoaderService]
            // [Microsoft.Practices.CompositeUI.Services.IModuleEnumerator, Microsoft.Practices.CompositeUI.Services.FileCatalogModuleEnumerator]
            // [Microsoft.Practices.CompositeUI.Commands.ICommandAdapterMapService, Microsoft.Practices.CompositeUI.Commands.CommandAdapterMapService]
            // [Microsoft.Practices.CompositeUI.UIElements.IUIElementAdapterFactoryCatalog, Microsoft.Practices.CompositeUI.UIElements.UIElementAdapterFactoryCatalog]
            // [components.Service, components.ServiceImpl]
            // [Microsoft.Practices.CompositeUI.WinForms.IControlActivationService, Microsoft.Practices.CompositeUI.WinForms.ControlActivationService]
            System.Console.WriteLine("Services:");
            foreach (var item in RootWorkItem.Services)
                System.Console.WriteLine(item);
        }
    }
}

namespace components
{
    public interface Service
    {
        void foobar();
    }

    public class ServiceImpl : Service
    {
        public void foobar()
        {
            System.Console.WriteLine("implementation");
        }
    }
}
