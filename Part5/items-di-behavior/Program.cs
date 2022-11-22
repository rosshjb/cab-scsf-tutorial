using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.WinForms;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections;

namespace items_di_behavior
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

        private WorkItem childWorkItem;

        protected override void AfterShellCreated()
        {
            base.AfterShellCreated();

            // root WorkItem의 Items 컬렉션에 dependency로 사용될 객체 생성 및 등록.
            RootWorkItem.Items.AddNew<components.Dependency>("first_dependency");

            // 자식 WorkItem 생성 및 root WorkItem의 WorkItems 컬렉션에 등록; 생성된 객체가 리턴됨.
            childWorkItem = RootWorkItem.WorkItems.AddNew<WorkItem>();
            try
            {
                // Client 객체를 생성하고 자식 WorkItem의 Items 컬렉션에 추가하려고 하지만,
                // Client에서 요청하는 first_dependency 객체가 동일 WorkItem 상에 존재하지 않으므로(root WorkItem에는 존재함)
                // 객체를 찾지 못해 예외가 던져진다:
                // Microsoft.Practices.ObjectBuilder.DependencyMissingException: Could not locate dependency "components.Dependency".
                childWorkItem.Items.AddNew<components.Client>("client");
            }
            catch (DependencyMissingException e)
            {
                System.Console.WriteLine(e);
            }

            components.Client client = (components.Client)childWorkItem.Items["client"];
            System.Console.WriteLine(client);                           // components.Client
            System.Console.WriteLine(client.first_dependency == null);  // True
            System.Console.WriteLine(client.second_dependency == null); // True

            displayWorkItemsCollections();
        }

        /// <summary>
        /// 출력 예시:<br/>
        /// ————————————————————————————————————————————————————————————————————————————————<br/>
        /// root WorkItem's WorkItems:<br/>
        /// [1e3ebeb4-e41c-4465-8c8f-fafa9d23fcf0, Microsoft.Practices.CompositeUI.WorkItem]<br/>
        /// =&gt; root WorkItem에 추가한 child WorkItem을 확인할 수 있다; 참고로 root Workitem 자신은 자신의 Services 컬렉션에 포함된다.<br/>
        /// root WorkItem's Items:<br/>
        /// [a0ed1d51-deb2-42b3-9688-eb6214336f5a, Microsoft.Practices.CompositeUI.State]<br/>
        /// [4604caba-85c7-4b1b-8523-6265d17ada86, items_di_behavior.Form1, Text: Form1]<br/>
        /// [first_dependency, components.Dependency]<br/>
        /// [1e3ebeb4-e41c-4465-8c8f-fafa9d23fcf0, Microsoft.Practices.CompositeUI.WorkItem]<br/>
        /// =&gt; root WorkItem에 추가한 dependency를 확인할 수 있다.<br/>
        /// 또한, child WorkItem이 WorkItems 뿐만 아니라 Items에도 들어가 있음을 확인할 수 있다.<br/>
        /// root WorkItem's Services:<br/>
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
        /// =&gt; root WorkItem 자신이 컬렉션에 추가되어 있음을 알 수 있다.<br/>
        /// ————————————————————————————————————————————————————————————————————————————————<br/>
        /// child WorkItem's WorkItems:<br/>
        /// =&gt; child WorkItem에는 아무것도 없다; 참고로 child Workitem 자신은 자신의 Services 컬렉션에 포함된다.<br/>
        /// child WorkItem's Items:<br/>
        /// [c5982de1-f2b1-486b-8159-8e580128b33c, Microsoft.Practices.CompositeUI.State]<br/>
        /// [client, components.Client]<br/>
        /// =&gt; Client의 dependency를 찾지 못해 DI는 실패해 예외가 발생했지만 여전히 객체는 생성이 되고 Items 컬렉션에서 관리되고 있는 듯하다.; second_dependency까지 생성 및 등록이 안되어 있음<br/>
        /// child WorkItem's Services:<br/>
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
        /// =&gt; child WorkItem 자신이 컬렉션에 추가되어 있음을 알 수 있다.
        /// </summary>
        private void displayWorkItemsCollections()
        {
            displayCollection("root WorkItem's WorkItems", RootWorkItem.WorkItems);
            displayCollection("root WorkItem's Items", RootWorkItem.Items);
            displayCollection("root WorkItem's Services", RootWorkItem.Services);

            displayCollection("child WorkItem's WorkItems", childWorkItem.WorkItems);
            displayCollection("child WorkItem's Items", childWorkItem.Items);
            displayCollection("child WorkItem's Services", childWorkItem.Services);
        }

        private void displayCollection(string collectionName, ICollection collection)
        {
            System.Console.WriteLine($"{collectionName}:");
            foreach (var item in collection)
                System.Console.WriteLine(item.ToString());
        }
    }
}

namespace components
{
    public class Dependency { }

    public class Client
    {
        /// <summary>
        /// Client는 child WorkItem Items에서 managed될 것이지만 root WorkItem의 Items 컬렉션에 있는 first_dependency를 요청하고 있다 =&gt; DependencyMissingException
        /// </summary>
        [ComponentDependency("first_dependency")]
        public Dependency first_dependency { get; set; }

        [CreateNew]
        public Dependency second_dependency { get; set; }
    }
}