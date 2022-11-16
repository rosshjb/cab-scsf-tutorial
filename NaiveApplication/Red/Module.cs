using Microsoft.Practices.CompositeUI;

namespace Red
{
    public class Module : ModuleInit
    {
        public override void Load()
        {
            base.Load();

            Form1 form = new Form1();
            form.Show();
        }
    }
}