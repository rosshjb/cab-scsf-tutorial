using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.WinForms;
using System;
using System.Windows.Forms;

namespace command_for_custom_event
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

            demonstrateDotnetEvent();
            demonstrateCABCommandWithCustomEvent();
        }

        private void demonstrateDotnetEvent()
        {
            var invoker = new EventInvoker();
            
            var receiver = new EventSTDReceiver();
            invoker.customEvent += new EventHandler(receiver.sink);

            invoker.request();
        }

        private void demonstrateCABCommandWithCustomEvent()
        {
            var commandMapService = RootWorkItem.Services.Get<ICommandAdapterMapService>();
            commandMapService.Register(typeof(EventInvoker), typeof(EventCommandAdapter<EventInvoker>));
            
            var invoker = new EventInvoker();
            var receiver = RootWorkItem.Items.AddNew<EventCABReceiver>();

            RootWorkItem.Commands["ShowMessageCommand"]
                .AddInvoker(invoker, "customEvent");

            invoker.request();

            displayRootWorkItemCollections();
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
        /// [a03c420a-5a73-4e74-bf38-98a50a37724e, Microsoft.Practices.CompositeUI.State]<br/>
        /// [6959fe6b-2e86-4f3f-81d9-de3a3d459d34, command_for_custom_event.Form1, Text: Form1]<br/>
        /// [543c8807-f1cc-49da-b3b9-083d214dca9d, command_for_custom_event.EventCABReceiver]<br/>
        /// [ShowMessageCommand, Microsoft.Practices.CompositeUI.Commands.Command]<br/>
        /// Commands:<br/>
        /// [ShowMessageCommand, Microsoft.Practices.CompositeUI.Commands.Command]<br/>
        /// </summary>
        private void displayRootWorkItemCollections()
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

    public class EventInvoker
    {
        public event EventHandler customEvent;

        public void request()
        {
            customEvent(this, EventArgs.Empty);
        }
    }

    public class EventSTDReceiver
    {
        public void sink(object sender, EventArgs e)
        {
            MessageBox.Show("sink the event with .NET");
        }
    }

    public class EventCABReceiver
    {
        [CommandHandler("ShowMessageCommand")]
        public void sink(object sender, EventArgs e)
        {

            MessageBox.Show("sink the event with CAB");
        }
    }
}