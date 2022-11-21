using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Shell
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// shell form은 자동으로 container managed이므로 DI 받을 수 있다.
        /// </summary>
        [ServiceDependency]
        public WorkItem ParentWorkItem { get; set; }

        [CreateNew]
        public User.Component UserComponent { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void showRootWorkItemsCollectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserComponent.displayRootWorkItemsCollections();
        }
    }
}

namespace User
{
    public class Component
    {
        /// <summary>
        /// Component는 shell form에서 생성되면서 Items 컬렉션에 주입되기 때문에 container managed된다.<br/>
        /// 따라서 DI를 받을 수 있게 된다.
        /// </summary>
        [ServiceDependency]
        public WorkItem ParentWorkItem { get; set; }

        /// <summary>
        /// 출력 예시:<br/>
        /// Items:<br/>
        /// [87085dc2-d9ea-43fa-9cf0-862244598d72, Microsoft.Practices.CompositeUI.State]<br/>
        /// [4a738492-de31-493d-a6c7-a45bb8d6cff0, Shell.Form1, Text: Form1]<br/>
        /// [5f1381c9-3772-43a1-8445-dc1908c3da48, User.Component]<br/>
        /// [Shell, Shell.Form1, Text: Form1]<br/>
        /// [89bfc1b2-dea1-4adc-a122-24b91948a950, Blue.Module]<br/>
        /// [1dd7e561-a0d5-4fce-8bef-f57c69f68449, Red.Module]<br/>
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
        /// [Microsoft.Practices.CompositeUI.WinForms.IControlActivationService, Microsoft.Practices.CompositeUI.WinForms.ControlActivationService]
        /// </summary>
        public void displayRootWorkItemsCollections()
        {
            System.Console.WriteLine("Items:");
            foreach (KeyValuePair<string, object> item in ParentWorkItem.Items)
            {
                System.Console.WriteLine(item);
            }

            System.Console.WriteLine("Services:");
            foreach (KeyValuePair<Type, object> item in ParentWorkItem.Services)
            {
                System.Console.WriteLine(item);
            }
        }
    }
}