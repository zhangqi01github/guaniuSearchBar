using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuaniuSearchBar
{
    public partial class Advises : NoBorderFormBase
    {
        public Advises()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            HttpHelper.HttpGet(HttpHelper.baseUrl + "advise/" + this.tbContact.Text + "/" + tbProblem.Text + "/");

            this.pbFeedback.Visible = true;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Advises_Load(object sender, EventArgs e)
        {

        }




    }
}
