namespace GuaniuSearchBar
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.在桌面显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.基础设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.检查更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.在线帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.意见反馈ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pbLeftIcon = new System.Windows.Forms.PictureBox();
            this.pbArrow = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.btnSearch = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLeftIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbArrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tbSearch
            // 
            this.tbSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSearch.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbSearch.Location = new System.Drawing.Point(54, 3);
            this.tbSearch.MaxLength = 100;
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(158, 23);
            this.tbSearch.TabIndex = 0;
            this.tbSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tbSearch_MouseClick);
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            this.tbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.在桌面显示ToolStripMenuItem,
            this.基础设置ToolStripMenuItem,
            this.检查更新ToolStripMenuItem,
            this.在线帮助ToolStripMenuItem,
            this.意见反馈ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 136);
            // 
            // 在桌面显示ToolStripMenuItem
            // 
            this.在桌面显示ToolStripMenuItem.Name = "在桌面显示ToolStripMenuItem";
            this.在桌面显示ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.在桌面显示ToolStripMenuItem.Text = "在桌面显示";
            this.在桌面显示ToolStripMenuItem.Click += new System.EventHandler(this.在桌面显示ToolStripMenuItem_Click);
            // 
            // 基础设置ToolStripMenuItem
            // 
            this.基础设置ToolStripMenuItem.Name = "基础设置ToolStripMenuItem";
            this.基础设置ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.基础设置ToolStripMenuItem.Text = "基础设置";
            this.基础设置ToolStripMenuItem.Click += new System.EventHandler(this.基础设置ToolStripMenuItem_Click);
            // 
            // 检查更新ToolStripMenuItem
            // 
            this.检查更新ToolStripMenuItem.Name = "检查更新ToolStripMenuItem";
            this.检查更新ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.检查更新ToolStripMenuItem.Text = "检查更新";
            this.检查更新ToolStripMenuItem.Click += new System.EventHandler(this.检查更新ToolStripMenuItem_Click);
            // 
            // 在线帮助ToolStripMenuItem
            // 
            this.在线帮助ToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.在线帮助ToolStripMenuItem.Name = "在线帮助ToolStripMenuItem";
            this.在线帮助ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.在线帮助ToolStripMenuItem.Text = "在线帮助";
            this.在线帮助ToolStripMenuItem.Click += new System.EventHandler(this.在线帮助ToolStripMenuItem_Click);
            // 
            // 意见反馈ToolStripMenuItem
            // 
            this.意见反馈ToolStripMenuItem.Name = "意见反馈ToolStripMenuItem";
            this.意见反馈ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.意见反馈ToolStripMenuItem.Text = "意见反馈";
            this.意见反馈ToolStripMenuItem.Click += new System.EventHandler(this.意见反馈ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // pbLeftIcon
            // 
            this.pbLeftIcon.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pbLeftIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbLeftIcon.Image = global::GuaniuSearchBar.Properties.Resources.bd_logo_18x18;
            this.pbLeftIcon.Location = new System.Drawing.Point(1, 0);
            this.pbLeftIcon.Name = "pbLeftIcon";
            this.pbLeftIcon.Size = new System.Drawing.Size(26, 30);
            this.pbLeftIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbLeftIcon.TabIndex = 2;
            this.pbLeftIcon.TabStop = false;
            this.pbLeftIcon.Click += new System.EventHandler(this.pbLeftIcon_Click);
            this.pbLeftIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbLeft_Click);
            this.pbLeftIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbLeft_MouseDown);
            this.pbLeftIcon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbLeftIcon_MouseMove);
            this.pbLeftIcon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbLeftIcon_MouseUp);
            // 
            // pbArrow
            // 
            this.pbArrow.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pbArrow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbArrow.Image = global::GuaniuSearchBar.Properties.Resources.less_btn;
            this.pbArrow.Location = new System.Drawing.Point(33, 13);
            this.pbArrow.Name = "pbArrow";
            this.pbArrow.Size = new System.Drawing.Size(12, 6);
            this.pbArrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbArrow.TabIndex = 3;
            this.pbArrow.TabStop = false;
            this.pbArrow.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbLeft_Click);
            this.pbArrow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbLeft_MouseDown);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox4.Location = new System.Drawing.Point(43, 0);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(174, 30);
            this.pictureBox4.TabIndex = 5;
            this.pictureBox4.TabStop = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSearch.Image = global::GuaniuSearchBar.Properties.Resources.seek_btn;
            this.btnSearch.Location = new System.Drawing.Point(215, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(35, 30);
            this.btnSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnSearch.TabIndex = 6;
            this.btnSearch.TabStop = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Location = new System.Drawing.Point(26, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(18, 30);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbLeft_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.pbLeftIcon);
            this.mainPanel.Controls.Add(this.pictureBox1);
            this.mainPanel.Controls.Add(this.btnSearch);
            this.mainPanel.Controls.Add(this.pbArrow);
            this.mainPanel.Controls.Add(this.tbSearch);
            this.mainPanel.Controls.Add(this.pictureBox4);
            this.mainPanel.Location = new System.Drawing.Point(0, 500);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(247, 32);
            this.mainPanel.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Green;
            this.ClientSize = new System.Drawing.Size(346, 1000);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "Form1";
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLeftIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbArrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.PictureBox pbLeftIcon;
        private System.Windows.Forms.PictureBox pbArrow;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox btnSearch;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem 在桌面显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 基础设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 检查更新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 在线帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 意见反馈ToolStripMenuItem;
        private System.Windows.Forms.Panel mainPanel;
    }
}

