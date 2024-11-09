namespace CantoneseTranslation
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnExchange = new Button();
            lblSource = new Label();
            lblTarget = new Label();
            menuStrip1 = new MenuStrip();
            tsmiSetting = new ToolStripMenuItem();
            tsmiReserved = new ToolStripMenuItem();
            txtSource = new RichTextBox();
            txtTarget = new RichTextBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            tsmiSelectAll = new ToolStripMenuItem();
            tsmiCopy = new ToolStripMenuItem();
            btnExecute = new Button();
            menuStrip1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnExchange
            // 
            btnExchange.Image = App.Resource.Exchange;
            btnExchange.Location = new Point(59, 25);
            btnExchange.Name = "btnExchange";
            btnExchange.Size = new Size(49, 23);
            btnExchange.TabIndex = 0;
            btnExchange.UseVisualStyleBackColor = true;
            btnExchange.Click += btnExchange_Click;
            // 
            // lblSource
            // 
            lblSource.AutoSize = true;
            lblSource.Location = new Point(12, 28);
            lblSource.Name = "lblSource";
            lblSource.Size = new Size(32, 17);
            lblSource.TabIndex = 1;
            lblSource.Text = "粤语";
            // 
            // lblTarget
            // 
            lblTarget.AutoSize = true;
            lblTarget.Location = new Point(124, 28);
            lblTarget.Name = "lblTarget";
            lblTarget.Size = new Size(44, 17);
            lblTarget.TabIndex = 2;
            lblTarget.Text = "普通话";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { tsmiSetting });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(772, 25);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            // 
            // tsmiSetting
            // 
            tsmiSetting.DropDownItems.AddRange(new ToolStripItem[] { tsmiReserved });
            tsmiSetting.Name = "tsmiSetting";
            tsmiSetting.Size = new Size(44, 21);
            tsmiSetting.Text = "设置";
            // 
            // tsmiReserved
            // 
            tsmiReserved.Name = "tsmiReserved";
            tsmiReserved.Size = new Size(112, 22);
            tsmiReserved.Text = "保留项";
            tsmiReserved.Click += tsmiReserved_Click;
            // 
            // txtSource
            // 
            txtSource.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtSource.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtSource.Location = new Point(2, 61);
            txtSource.Name = "txtSource";
            txtSource.Size = new Size(767, 192);
            txtSource.TabIndex = 4;
            txtSource.Text = "";
            // 
            // txtTarget
            // 
            txtTarget.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtTarget.ContextMenuStrip = contextMenuStrip1;
            txtTarget.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtTarget.Location = new Point(2, 285);
            txtTarget.Name = "txtTarget";
            txtTarget.Size = new Size(767, 190);
            txtTarget.TabIndex = 5;
            txtTarget.Text = "";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { tsmiSelectAll, tsmiCopy });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(101, 48);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // tsmiSelectAll
            // 
            tsmiSelectAll.Name = "tsmiSelectAll";
            tsmiSelectAll.Size = new Size(100, 22);
            tsmiSelectAll.Text = "全选";
            tsmiSelectAll.Click += tsmiSelectAll_Click;
            // 
            // tsmiCopy
            // 
            tsmiCopy.Name = "tsmiCopy";
            tsmiCopy.Size = new Size(100, 22);
            tsmiCopy.Text = "复制";
            tsmiCopy.Click += tsmiCopy_Click;
            // 
            // btnExecute
            // 
            btnExecute.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExecute.Location = new Point(695, 258);
            btnExecute.Name = "btnExecute";
            btnExecute.Size = new Size(75, 23);
            btnExecute.TabIndex = 6;
            btnExecute.Text = "执行";
            btnExecute.UseVisualStyleBackColor = true;
            btnExecute.Click += btnExecute_Click;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(772, 478);
            Controls.Add(btnExecute);
            Controls.Add(txtTarget);
            Controls.Add(txtSource);
            Controls.Add(lblTarget);
            Controls.Add(lblSource);
            Controls.Add(btnExchange);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "粤语翻译";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnExchange;
        private Label lblSource;
        private Label lblTarget;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem tsmiSetting;
        private ToolStripMenuItem tsmiReserved;
        private RichTextBox txtSource;
        private RichTextBox txtTarget;
        private Button btnExecute;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem tsmiSelectAll;
        private ToolStripMenuItem tsmiCopy;
    }
}
