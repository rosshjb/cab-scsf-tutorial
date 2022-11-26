using components;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI.WinForms;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections;

namespace services
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

            RootWorkItem.Services.AddNew<components.Service>();
            RootWorkItem.Items.AddNew<components.Service>("svc2");

            try {
                RootWorkItem.Services.AddNew<components.Service>();
            } catch (ArgumentException e) {
                // 예외 발생: 'System.ArgumentException'(Microsoft.Practices.CompositeUI.dll)
                // A service of this type already exists: components.Service.
                System.Console.WriteLine(e.Message);
            }

            // notfound_svc1 is null ? True, notfound_svc2 is null ? True
            Client1 client = RootWorkItem.Items.AddNew<components.Client1>("client1");
            client.printFields();

            try {
                RootWorkItem.Items.AddNew<components.Client2>();
            } catch (ServiceMissingException e) {
                // 예외 발생: 'Microsoft.Practices.CompositeUI.Services.ServiceMissingException'(Microsoft.Practices.CompositeUI.dll)
                // Service components.NotFoundService is not available in the current context.
                System.Console.WriteLine(e.Message);
            }

            try {
                RootWorkItem.Items.AddNew<components.Client3>();
            }
            catch (DependencyMissingException e) {
                // 예외 발생: 'Microsoft.Practices.ObjectBuilder.DependencyMissingException'(Microsoft.Practices.ObjectBuilder.dll)
                // Could not locate dependency "components.NotFoundService".
                System.Console.WriteLine(e.Message);
            }

            // dependency lookup:
            WorkItem rootWorkItem = RootWorkItem.Services.Get<WorkItem>(ensureExists : true);
            Client1 client1 = RootWorkItem.Items.Get<Client1>("client1");

            // split interface from implementation
            RootWorkItem.Services.AddNew<components.ServiceImplementation, components.AbstractService>();
            components.AbstractService iService = RootWorkItem.Services.Get<components.AbstractService>();
            iService.foobar();

            // print workitem collections
            printRootWorkItemCollection("WorkItems", RootWorkItem.WorkItems);
            printRootWorkItemCollection("Services", RootWorkItem.Services);
            printRootWorkItemCollection("Items", RootWorkItem.Items);
        }

        /// <summary>
        /// WorkItems : <br/>
        /// Services : <br/>
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
        /// [components.Service, components.Service]<br/>
        /// [components.AbstractService, components.ServiceImplementation]<br/>
        /// Items : <br/>
        /// [091de335-a2b9-499d-8d36-edfad2750bc9, Microsoft.Practices.CompositeUI.State]<br/>
        /// [69149749-0bb3-4c59-95f6-b4db0002e9db, services.Form1, Text: Form1]<br/>
        /// [svc2, components.Service]<br/>
        /// [client1, components.Client1]<br/>
        /// [9f3cd029-02b4-486b-aa19-d13ad8b43ceb, components.Client2]<br/>
        /// [856d23a8-61d8-4c11-a67e-a558efd4c372, components.Client3]<br/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="collection"></param>
        void printRootWorkItemCollection(string name, ICollection collection)
        {
            System.Console.WriteLine($"{name} : ");
            foreach (var obj in collection)
                System.Console.WriteLine(obj);
        }
    }
}

namespace components
{
    public class Client1
    {
        [ServiceDependency(Required = false)]
        public NotFoundService notfound_svc1 { get; set; }

        [ComponentDependency(id: "notfound_svc2", Required = false)]
        public NotFoundService notfound_svc2 { get; set; }

    public void printFields() {
            System.Console.WriteLine($"notfound_svc1 is null ? {notfound_svc1 == null}, notfound_svc2 is null ? {notfound_svc2 == null}");
        }
    }

    public class Client2
    {
        public Client2([ServiceDependency] NotFoundService svc3) { }
    }

    public class Client3
    {
        public Client3([ComponentDependency("svc3")] NotFoundService svc4) { }
    }

    public class Service { }

    public class NotFoundService { }

    public interface AbstractService {
        void foobar();
    }

    public class ServiceImplementation : AbstractService {
        public void foobar() { System.Console.WriteLine("foobar"); }
    }
}