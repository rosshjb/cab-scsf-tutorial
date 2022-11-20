using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.WinForms;
using System;

namespace Shell
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

            this.Shell.IsMdiContainer = true;

            RootWorkItem.Items.Add(this.Shell, "Shell");
        }
    }
}
