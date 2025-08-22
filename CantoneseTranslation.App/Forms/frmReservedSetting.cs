using viwik.CantoneseTranslation.App.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace viwik.CantoneseTranslation.App
{
    public partial class frmReservedSetting : Form
    {
        public frmReservedSetting()
        {
            InitializeComponent();
        }

        private void frmReservedSetting_Load(object sender, EventArgs e)
        {
            string filePath = SettingManager.ReservedFilePath;

            if (File.Exists(filePath))
            {
                this.txtContent.Text = File.ReadAllText(filePath);

                this.txtContent.SelectionStart=this.txtContent.TextLength;
                this.txtContent.SelectionLength = 0;
                this.txtContent.Select();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string content = this.txtContent.Text.Trim();

            string configFolder = SettingManager.ConfigFolder;

            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }

            File.WriteAllText(SettingManager.ReservedFilePath, content);

            MessageBox.Show("保存成功！");

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
