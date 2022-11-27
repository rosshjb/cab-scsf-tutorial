using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.WinForms;
using System;

namespace service_attribute
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

            components.IService svc = RootWorkItem.Services.Get<components.IService>();
            svc.foobar();  // do implementation
        }
    }
}

namespace components
{
    public interface IService {
        void foobar();
    }

    [Service(registerAs: typeof(IService), AddOnDemand = false)]
    public class ServiceImpl : IService {
        public void foobar()
        {
            System.Console.WriteLine("do implementation");
        }
    }
}
