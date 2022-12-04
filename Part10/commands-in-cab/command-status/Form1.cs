using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace command_status
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

        [CommandHandler("ChangeStatusCommand")]
        public void changeStatusOfTheButton(Object sender, EventArgs e)
        {
            Command command = rootWorkItem.Commands["ShowMessageCommand"];
            
            if (command.Status == CommandStatus.Enabled)
                command.Status = CommandStatus.Disabled;
            else
                command.Status = CommandStatus.Enabled;
        }
    }
}
