using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI.WinForms;
using Microsoft.Practices.ObjectBuilder;
using System;

namespace ctor_injection_in_cab
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

            RootWorkItem.Items.AddNew<User.Service>("svc1");
            RootWorkItem.Items.AddNew<User.Client1>("client1");

            RootWorkItem.Items.AddNew<User.Service>("svc3");
            RootWorkItem.Items.AddNew<User.Client2>("client2");

            RootWorkItem.Items.AddNew<User.Service>("svc5");
            RootWorkItem.Items.AddNew<User.Client3>("client3");

            RootWorkItem.Items.AddNew<User.Client4>("client4");

            try {
                RootWorkItem.Items.AddNew<User.Client5>("client5");
            } catch (ServiceMissingException e) {
                // 예외 발생: 'Microsoft.Practices.CompositeUI.Services.ServiceMissingException'(Microsoft.Practices.CompositeUI.dll)
                // Service User.NotFoundService is not available in the current context.
                System.Console.WriteLine(e.Message);  
            }
            User.Client5 client5 = (User.Client5)RootWorkItem.Items["client5"];
            client5.printFields();  // NotFoundService is null? True, rootWorkItem is null? True

            printRootWorkItemsCollection();
        }

        /// <summary>
        /// Items:<br/>
        /// [79271071-fd2c-4cf9-808e-658ce9ca5168, Microsoft.Practices.CompositeUI.State]<br/>
        /// [f463c0d7-fc1d-4019-a2fc-074e017d6293, ctor_injection_in_cab.Form1, Text: Form1]<br/>
        /// [svc1, User.Service]<br/>
        /// [client1, User.Client1]<br/>
        /// [ef662723-7354-4567-a29c-d67c3c9f2954, User.Service]<br/>
        /// [svc3, User.Service]<br/>
        /// [client2, User.Client2]<br/>
        /// [aaee625f-6876-4d5a-9f5b-94324d149869, User.Service]<br/>
        /// [svc5, User.Service]<br/>
        /// [client3, User.Client3]<br/>
        /// [78761aee-3218-4ec1-a672-9021581b8690, User.Service]<br/>
        /// [client4, User.Client4]<br/>
        /// [client5, User.Client5]<br/>
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
        /// [User.Service, User.Service]
        /// </summary>
        private void printRootWorkItemsCollection()
        {
            System.Console.WriteLine("Items:");
            foreach (var item in RootWorkItem.Items)
                System.Console.WriteLine(item);

            System.Console.WriteLine("Services:");
            foreach (var item in RootWorkItem.Services)
                System.Console.WriteLine(item);
        }
    }
}

namespace User
{
    /// <summary>
    /// ctor injection도 setter injection과 마찬가지로 기본적인 DI API 사용 방식은 동일함. 
    /// </summary>
    public class Client1
    {
        [InjectionConstructor]
        public Client1([ComponentDependency("svc1")] Service svc1, [CreateNew] Service svc2, [ServiceDependency] WorkItem rootWorkItem)
        {
            System.Console.WriteLine($"svc1 : {svc1} : {svc1.GetHashCode()}");          // svc1 : User.Service : 13869071
            System.Console.WriteLine($"svc2 : {svc2} : {svc2.GetHashCode()}");          // svc2 : User.Service : 57712780
            System.Console.WriteLine($"{rootWorkItem} : {rootWorkItem.GetHashCode()}"); // Microsoft.Practices.CompositeUI.WorkItem : 32854180
        }
    }

    /// <summary>
    /// ctor가 하나이므로 InjectionConstructor attribute 불필요<br/>
    /// </summary>
    public class Client2
    {
        public Client2([ComponentDependency("svc3")] Service svc3, [CreateNew] Service svc4, [ServiceDependency] WorkItem rootWorkItem)
        {
            System.Console.WriteLine($"svc3 : {svc3} : {svc3.GetHashCode()}");          // svc3 : User.Service : 44223604
            System.Console.WriteLine($"svc4 : {svc4} : {svc4.GetHashCode()}");          // svc4 : User.Service : 62468121
            System.Console.WriteLine($"{rootWorkItem} : {rootWorkItem.GetHashCode()}"); // Microsoft.Practices.CompositeUI.WorkItem : 32854180
        }
    }

    /// <summary>
    /// ctor가 둘 이상이면 InjectionContructor attribute가 있는 생성자가 선택됨
    /// </summary>
    public class Client3
    {
        public Client3(WorkItem rootWorkItem)
        {
            throw new NotSupportedException();
        }

        [InjectionConstructor]
        public Client3([ComponentDependency("svc5")] Service svc5, [CreateNew] Service svc6, WorkItem rootWorkItem)
        {
            System.Console.WriteLine($"svc5 : {svc5} : {svc5.GetHashCode()}");          // svc5 : User.Service : 26753075
            System.Console.WriteLine($"svc6 : {svc6} : {svc6.GetHashCode()}");          // svc6 : User.Service : 39451090
            System.Console.WriteLine($"{rootWorkItem} : {rootWorkItem.GetHashCode()}"); // Microsoft.Practices.CompositeUI.WorkItem : 32854180
        }
    }

    /// <summary>
    /// 1. ctor가 하나이므로 InjectionConstructor attribute 불필요<br/>
    /// 2. arg에 attribute 생략되었으므로 암묵적으로 ServiceDependency로 간주 -&gt; Services 컬렉션에 Service 객체 없으므로 Services 컬렉션에 새로 생성 후 등록 후 주입됨.<br/>
    ///    기존에 객체가 없으면 새로 생성되는 것이 ServiceDependency attribute의 기본 동작은 아님; ctor injection에서 ServiceDependency가 "암묵적으로" 적용되었을 때에만 그러함.
    /// </summary>
    public class Client4
    {
        public Client4(Service svc7, WorkItem rootWorkItem)
        {
            System.Console.WriteLine($"svc7 : {svc7} : {svc7.GetHashCode()}");          // svc7 : User.Service : 41421720
            System.Console.WriteLine($"{rootWorkItem} : {rootWorkItem.GetHashCode()}"); // Microsoft.Practices.CompositeUI.WorkItem : 32854180
        }
    }

    /// <summary>
    /// 명시적으로 ServiceDependency가 적용된 경우에는, dependency를 찾지 못했다고 해서 생성되어 Services 컬렉션에 추가되거나 하지는 않는다.<br/>
    /// 그냥 <c>ServiceMissingException</c>이 던져진다; 암묵적으로 attribute가 적용된 것이 아니기 때문.
    /// </summary>
    public class Client5
    {
        private NotFoundService svc8;
        private WorkItem rootWorkItem;
        public Client5([ServiceDependency] NotFoundService svc8, WorkItem rootWorkItem)
        {
            this.svc8 = svc8;
            this.rootWorkItem = rootWorkItem;
        }

        public void printFields()
        {
            System.Console.WriteLine($"NotFoundService is null? {svc8 == null}, rootWorkItem is null? {rootWorkItem == null}");
        }
    }

    public class Service { }

    public class NotFoundService { }
}