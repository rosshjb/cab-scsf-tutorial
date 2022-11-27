using components;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI.WinForms;
using System;

namespace service_methods
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

            // Service1 is instantiated.
            RootWorkItem.Services.AddNew<Service1>();
            Service1 svc1 = RootWorkItem.Services.Get<Service1>();

            // Service2 is instantiated.
            Service2 svc2 = new Service2();
            RootWorkItem.Services.Add<Service2>(svc2);
            svc2 = RootWorkItem.Services.Get<Service2>();

            RootWorkItem.Services.AddOnDemand<Service3>();

            // Services(1):
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
            // [Microsoft.Practices.CompositeUI.WinForms.IControlActivationService, Microsoft.Practices.CompositeUI.WinForms.ControlActivationService]
            // [components.Service1, components.Service1]
            // [components.Service2, components.Service2]
            // [components.Service3, Microsoft.Practices.CompositeUI.Collections.ServiceCollection+DemandAddPlaceholder]
            System.Console.WriteLine("Services(1):");
            foreach (var item in RootWorkItem.Services)
                System.Console.WriteLine(item);

            // Service3 is instantiated.
            Service3 svc3 = RootWorkItem.Services.Get<Service3>();

            // Services(2):
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
            // [Microsoft.Practices.CompositeUI.WinForms.IControlActivationService, Microsoft.Practices.CompositeUI.WinForms.ControlActivationService]
            // [components.Service1, components.Service1]
            // [components.Service2, components.Service2]
            // [components.Service3, components.Service3]
            System.Console.WriteLine("Services(2):");
            foreach (var item in RootWorkItem.Services)
                System.Console.WriteLine(item);

            RootWorkItem.Services.AddNew<Service4>();
            if (RootWorkItem.Services.Contains<Service4>())
                RootWorkItem.Services.Remove<Service4>();
            // Service4 is null ? True
            System.Console.WriteLine($"Service4 is null ? {RootWorkItem.Services.Get<Service4>(ensureExists: false) == null}");
        }
    }
}

namespace components
{
    public class Service1 {
        public Service1() { System.Console.WriteLine("Service1 is instantiated.");  }
    }

    public class Service2 {
        public Service2() { System.Console.WriteLine("Service2 is instantiated."); }
    }

    public class Service3 {
        public Service3() { System.Console.WriteLine("Service3 is instantiated."); }
    }

    public class Service4 { }
}