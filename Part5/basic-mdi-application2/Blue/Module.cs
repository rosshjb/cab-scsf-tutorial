using Microsoft.Practices.CompositeUI;
using System.Windows.Forms;

namespace Blue
{
    public class Module : ModuleInit
    {
        /// <summary>
        /// module들은 CAB가 module 생성 및 로드 시에 container managed object로 만드므로 DI를 받을 수 있다.<br/>
        /// 마찬가지로 WorkItem 또한 container managed object가 된다(WorkItem 자신의 Services 컬렉션에 속하게 됨).
        /// </summary>
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
