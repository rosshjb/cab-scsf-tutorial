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

namespace command_handler_usage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [CommandHandler("ShowMessageCommand")]
        public void button1_command_handler(object sender, EventArgs e)
        {
            MessageBox.Show("button using command handler clicked");
        }

        /// <summary>
        /// .NET button click event handler
        /// </summary>
        /// <param name="sender">event invoker</param>
        /// <param name="e">event arguments</param>
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("buton using .NET event handler clicked");
        }
    }
}
