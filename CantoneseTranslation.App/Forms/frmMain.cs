using viwik.CantoneseTranslation.App.Manager;
using System.Text;
using viwik.CantoneseLearning.BLL.Core;
using viwik.CantoneseLearning.BLL.Core.Model;
using viwik.CantoneseLearning.Utility;

namespace viwik.CantoneseTranslation.App
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private async void btnExecute_Click(object sender, EventArgs e)
        {
            string source = this.txtSource.Text.Trim();

            if (string.IsNullOrEmpty(source))
            {
                MessageBox.Show("要翻译的内容不能为空！");
                return;
            }

            TranslateType translateType = this.lblSource.Text == "粤语" ? TranslateType.Cantonese2Mandarin : TranslateType.Mandarin2Cantonese;

            List<TextItem> items = new List<TextItem>();

            StringBuilder sbContent = new StringBuilder();
            StringBuilder sbSpecial = new StringBuilder();

            Action<StringBuilder, bool> addItem = (sb, isContent) =>
            {
                if (sb.Length > 0)
                {
                    items.Add(new TextItem() { IsContent = isContent, Text = sb.ToString() });
                    sb.Clear();
                }
            };

            foreach (char ch in source)
            {
                if (StringHelper.IsChineseChar(ch))
                {
                    sbContent.Append(ch);

                    addItem(sbSpecial, false);
                }
                else
                {
                    sbSpecial.Append(ch);

                    addItem(sbContent, true);
                }
            }

            if (sbContent.Length > 0)
            {
                addItem(sbContent, true);
            }

            if (sbSpecial.Length > 0)
            {
                addItem(sbSpecial, false);
            }

            var reservedWords = this.GetReservedWords();

            StringBuilder sbResult = new StringBuilder();

            foreach (TextItem item in items)
            {
                if (item.IsContent)
                {
                    if (reservedWords.Contains(item.Text))
                    {
                        sbResult.Append(item.Text);
                    }
                    else
                    {
                        var result = await DataProcessor.Translate(translateType, item.Text);

                        if (result.Contents.Count > 0)
                        {
                            sbResult.Append(result.Contents.FirstOrDefault());
                        }
                    }
                }
                else
                {
                    sbResult.Append(item.Text);
                }
            }

            this.txtTarget.Text = sbResult.ToString();
        }

        private IEnumerable<string> GetReservedWords()
        {
            string filePath = SettingManager.ReservedFilePath;

            if (File.Exists(filePath))
            {
                return File.ReadAllLines(filePath);
            }

            return Array.Empty<string>();
        }

        private void tsmiReserved_Click(object sender, EventArgs e)
        {
            frmReservedSetting frmReservedSetting = new frmReservedSetting();

            frmReservedSetting.ShowDialog();
        }

        private void btnExchange_Click(object sender, EventArgs e)
        {
            string temp;

            temp = this.lblSource.Text;
            this.lblSource.Text = this.lblTarget.Text;
            this.lblTarget.Text = temp;
        }

        private void tsmiSelectAll_Click(object sender, EventArgs e)
        {
            this.txtTarget.SelectAll();
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.txtTarget.SelectedText);
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.tsmiSelectAll.Visible = this.txtTarget.Text.Trim().Length > 0;
            this.tsmiCopy.Visible = this.txtTarget.SelectedText.Length > 0;
        }
    }

    class TextItem
    {
        public bool IsContent { get; set; }
        public string Text { get; set; }
    }
}
