using Microsoft.Practices.CompositeUI;
using System.Windows.Forms;

namespace Blue
{
    public class Module : ModuleInit
    {
        [ServiceDependency]
        public WorkItem ParentWorkItem { get; set; }

        public override void Load()
        {
            base.Load();

            Form shell = (Form)ParentWorkItem.Items["Shell"];

            Form1 form = new Form1();
            form.MdiParent = shell;
            form.Show();
        }
    }
}
