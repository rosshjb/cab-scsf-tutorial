using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace retrieve_service
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

            // instantiate and register an service object.
            RootWorkItem.Services.AddNew<components.Service, components.IService>();

            // dependency lookup with Get method.
            components.IService svc1 = RootWorkItem.Services.Get<components.IService>(ensureExists : true);
            svc1.foobar();  // foobar.

            // dependency injection with ServiceDependency attribute.
            // : CAB는 Client를 생성하면서 ServiceDependency attribute를 확인하고 서비스를 찾아 주입한 뒤 Client 객체를 Items 컬렉션에 추가한다.
            components.Client client = RootWorkItem.Items.AddNew<components.Client>();
            // foobar.
            // foobar.
            client.doSomething();

            // CAB는 현재 WorkItem의 Services 컬렉션에서 서비스 객체를 찾지 못하면 root까지 부모를 거슬러 올라가서 찾아본다.
            WorkItem childWorkItem = RootWorkItem.WorkItems.AddNew<WorkItem>("child");
            components.IService svc2 = childWorkItem.Services.Get<components.IService>();
            svc2.foobar();  // foobar.

            printWorkItemCollections();
        }

        /// <summary>
        /// root WorkItem.WorkItems:<br/>
        /// [child, Microsoft.Practices.CompositeUI.WorkItem]<br/>
        /// root WorkItem.Services:<br/>
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
        /// [components.IService, components.Service]<br/>
        /// root WorkItem.Items:<br/>
        /// [6d8050f3-85e1-4240-8bfc-865f07441f75, Microsoft.Practices.CompositeUI.State]<br/>
        /// [b14c791f-4f1f-48e9-84a7-a132a0648716, retrieve_service.Form1, Text: Form1]<br/>
        /// [05dca30f-356a-411f-9daa-abe70d184689, components.Client]<br/>
        /// [child, Microsoft.Practices.CompositeUI.WorkItem]<br/>
        /// child WorkItem.WorkItems:<br/>
        /// child WorkItem.Services:<br/>
        /// [Microsoft.Practices.CompositeUI.WorkItem, Microsoft.Practices.CompositeUI.WorkItem]<br/>
        /// [Microsoft.Practices.CompositeUI.WinForms.IControlActivationService, Microsoft.Practices.CompositeUI.WinForms.ControlActivationService]<br/>
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
        /// [components.IService, components.Service]<br/>
        /// child WorkItem.Items:<br/>
        /// [06de0395-dd4e-4a3f-b291-9c9a3694e344, Microsoft.Practices.CompositeUI.State]<br/>
        /// -&gt; child WorkItem의 Services에 서비스 객체를 등록하지 않았는데도 존재하고 있는 것으로 보임; root WorkItem의 Services와 공유되고 있는 것으로 보임.
        /// </summary>
        private void printWorkItemCollections()
        {
            printCollection("root WorkItem.WorkItems", RootWorkItem.WorkItems);
            printCollection("root WorkItem.Services", RootWorkItem.Services);
            printCollection("root WorkItem.Items", RootWorkItem.Items);

            WorkItem childWorkItem = RootWorkItem.WorkItems.Get<WorkItem>("child");
            printCollection("child WorkItem.WorkItems", childWorkItem.WorkItems);
            printCollection("child WorkItem.Services", childWorkItem.Services);
            printCollection("child WorkItem.Items", childWorkItem.Items);

            System.Console.WriteLine(System.Object.ReferenceEquals(
                RootWorkItem.Services.Get<components.IService>(), childWorkItem.Services.Get<components.IService>()
            ));  // True
        }

        private void printCollection(string name, ICollection collection)
        {
            System.Console.WriteLine($"{name}:");
            foreach (var item in collection)
                System.Console.WriteLine(item);
        }
    }
}

namespace components
{
    public class Client
    {
        [ServiceDependency]
        public IService service1 { get; set; }

        private IService service2;

        public Client([ServiceDependency] IService service2)
        {
            this.service2 = service2;
        }

        public void doSomething()
        {
            service1.foobar();
            service2.foobar();
        }
    }

    public interface IService {
        void foobar();
    }

    public class Service : IService {
        public void foobar()
        {
            System.Console.WriteLine("foobar.");
        }
    }
}