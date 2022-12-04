using Microsoft.Practices.CompositeUI.Commands;
using System;
using System.Windows.Forms;

namespace command_handler_parameters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [CommandHandler("ShowMessageCommand")]
        public void showMessage(object sender, EventArgs e)
        {
            Console.WriteLine(sender.GetType());           // Microsoft.Practices.CompositeUI.Commands.Command
            Console.WriteLine(e.Equals(EventArgs.Empty));  // True
            
            MessageBox.Show("hello, world");
        }

        private void eventButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine(sender.GetType());  // System.Windows.Forms.ToolStripButton

            MessageBox.Show("hello, world");
        }
    }
}
