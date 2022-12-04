using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Commands;
using System;
using System.Windows.Forms;

namespace command_execute
{
    public partial class Form1 : Form
    {
        [ServiceDependency]
        public WorkItem rootWorkItem { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        [CommandHandler("ShowMessageCommand")]
        public void showMessage(Object sender, EventArgs e)
        {
            MessageBox.Show("hello, world");
        }

        private void showMessageButton2_Click(object sender, EventArgs e)
        {
            Command command = rootWorkItem.Commands["ShowMessageCommand"];
            command.Execute();
        }
    }
}
