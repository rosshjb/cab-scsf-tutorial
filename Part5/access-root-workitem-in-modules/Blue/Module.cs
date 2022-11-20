﻿using Microsoft.Practices.CompositeUI;

namespace Blue
{
    public class Module : ModuleInit
    {
        [ServiceDependency]
        public WorkItem parentWorkItem { get; set; }

        public override void Load()
        {
            base.Load();

            System.Console.WriteLine($"parentWorkItem in Blue module : {parentWorkItem}");

            Form1 form = new Form1();
            form.Show();
        }
    }
}
