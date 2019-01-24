using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuaniuSearchBar
{
    public partial class BasicSettings : System.Windows.Forms.Form
    {
        MainForm mainForm;
        public BasicSettings(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BasicSettings_Load(object sender, EventArgs e)
        {
        


        }
     

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            AppHotKey.UnRegKey(mainForm.Handle, MainForm.HotKeyID);
            if (comboBox1.SelectedIndex==0)
            {
                AppHotKey.RegKey(mainForm.Handle, MainForm.HotKeyID, AppHotKey.KeyModifiers.Ctrl | AppHotKey.KeyModifiers.Alt, Keys.Z);
            }
            if (comboBox1.SelectedIndex == 1)
            {
                AppHotKey.RegKey(mainForm.Handle, MainForm.HotKeyID, AppHotKey.KeyModifiers.Ctrl | AppHotKey.KeyModifiers.Shift | AppHotKey.KeyModifiers.Alt, Keys.D);
            }

        }
    }
}
