using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            var cfg= Config.ReadHotkeyConfigFromFile();
       
                //暂时关闭事件
                checkBox1.CheckedChanged -= checkBox1_CheckedChanged;
                comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
                //设置画面显示
                checkBox1.Checked = cfg.enabled;
                comboBox1.SelectedIndex = cfg.hotkeyIndex;
                //恢复事件
                checkBox1.CheckedChanged += checkBox1_CheckedChanged;
                comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            
        }
     

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                AppHotKey.UnRegKey(mainForm.Handle);
                AppHotKey.RegHotKey(mainForm.Handle, comboBox1.SelectedIndex);
                Config.SaveHotkeyConfig(new Config.HotKeyConfig(comboBox1.SelectedIndex,true));
            }

        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            AppHotKey.UnRegKey(mainForm.Handle);
            if (checkBox1.Checked)
            {
                AppHotKey.RegHotKey(mainForm.Handle, comboBox1.SelectedIndex);
            }
            Config.SaveHotkeyConfig(new Config.HotKeyConfig(comboBox1.SelectedIndex,checkBox1.Enabled));

        }
    }
}
