using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobResumeSender
{
    public partial class Console : Form
    {
        public Console()
        {
            InitializeComponent();
        }

        private void CanBusiBtn_Click(object sender, EventArgs e)
        {
            CanBusiness canBuis = new CanBusiness();
            canBuis.Show();
        }

        private void TorontoCaBtn_Click(object sender, EventArgs e)
        {
            TorontoCA tc = new TorontoCA();
            tc.Show();
        }

        private void EmailSenderBtn_Click(object sender, EventArgs e)
        {
            Email email = new Email();
            email.Show();
        }

        private void JpBtn_Click(object sender, EventArgs e)
        {
            JP jp = new JP();
            jp.Show();
        }
    }
}
