namespace tf3ed
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuROM = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpenROM = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveROMAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCloseROM = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpCompression = new System.Windows.Forms.TabPage();
            this.tcCompression = new System.Windows.Forms.TabControl();
            this.tbLzh = new System.Windows.Forms.TabPage();
            this.lvLzh = new System.Windows.Forms.ListView();
            this.chLzhIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLzhTableAddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLzhDataOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLzhCmpSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLzhDecSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbRle = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dlgOpenRom = new System.Windows.Forms.OpenFileDialog();
            this.dlgSaveRom = new System.Windows.Forms.SaveFileDialog();
            this.pnlLzhButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnLzhImport = new System.Windows.Forms.Button();
            this.btnLzhExtract = new System.Windows.Forms.Button();
            this.mnuMain.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpCompression.SuspendLayout();
            this.tcCompression.SuspendLayout();
            this.tbLzh.SuspendLayout();
            this.pnlLzhButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuROM,
            this.toolsToolStripMenuItem,
            this.mnuHelp});
            this.mnuMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.mnuMain.Size = new System.Drawing.Size(843, 24);
            this.mnuMain.TabIndex = 2;
            // 
            // mnuROM
            // 
            this.mnuROM.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpenROM,
            this.mnuSaveROMAs,
            this.mnuCloseROM,
            this.mnuSeparator,
            this.mnuExit});
            this.mnuROM.Name = "mnuROM";
            this.mnuROM.Size = new System.Drawing.Size(40, 18);
            this.mnuROM.Text = "&ROM";
            // 
            // mnuOpenROM
            // 
            this.mnuOpenROM.Name = "mnuOpenROM";
            this.mnuOpenROM.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnuOpenROM.Size = new System.Drawing.Size(228, 22);
            this.mnuOpenROM.Text = "&Open TF3 ROM...";
            this.mnuOpenROM.Click += new System.EventHandler(this.mnuOpenROM_Click);
            // 
            // mnuSaveROMAs
            // 
            this.mnuSaveROMAs.Name = "mnuSaveROMAs";
            this.mnuSaveROMAs.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuSaveROMAs.Size = new System.Drawing.Size(228, 22);
            this.mnuSaveROMAs.Text = "&Save ROM As...";
            // 
            // mnuCloseROM
            // 
            this.mnuCloseROM.Enabled = false;
            this.mnuCloseROM.Name = "mnuCloseROM";
            this.mnuCloseROM.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.mnuCloseROM.Size = new System.Drawing.Size(228, 22);
            this.mnuCloseROM.Text = "&Close ROM";
            this.mnuCloseROM.Click += new System.EventHandler(this.mnuCloseROM_Click);
            // 
            // mnuSeparator
            // 
            this.mnuSeparator.Name = "mnuSeparator";
            this.mnuSeparator.Size = new System.Drawing.Size(225, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mnuExit.Size = new System.Drawing.Size(228, 22);
            this.mnuExit.Text = "&Exit...";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(54, 18);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(47, 18);
            this.mnuHelp.Text = "&Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(130, 22);
            this.mnuAbout.Text = "&About...";
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpCompression);
            this.tcMain.Controls.Add(this.tabPage2);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.tcMain.Location = new System.Drawing.Point(0, 24);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(843, 401);
            this.tcMain.TabIndex = 3;
            // 
            // tpCompression
            // 
            this.tpCompression.Controls.Add(this.tcCompression);
            this.tpCompression.Location = new System.Drawing.Point(4, 23);
            this.tpCompression.Name = "tpCompression";
            this.tpCompression.Padding = new System.Windows.Forms.Padding(3);
            this.tpCompression.Size = new System.Drawing.Size(835, 374);
            this.tpCompression.TabIndex = 0;
            this.tpCompression.Text = "Compression";
            this.tpCompression.UseVisualStyleBackColor = true;
            // 
            // tcCompression
            // 
            this.tcCompression.Controls.Add(this.tbLzh);
            this.tcCompression.Controls.Add(this.tbRle);
            this.tcCompression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcCompression.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.tcCompression.Location = new System.Drawing.Point(3, 3);
            this.tcCompression.Name = "tcCompression";
            this.tcCompression.SelectedIndex = 0;
            this.tcCompression.Size = new System.Drawing.Size(829, 368);
            this.tcCompression.TabIndex = 4;
            // 
            // tbLzh
            // 
            this.tbLzh.Controls.Add(this.lvLzh);
            this.tbLzh.Controls.Add(this.pnlLzhButtons);
            this.tbLzh.Location = new System.Drawing.Point(4, 23);
            this.tbLzh.Name = "tbLzh";
            this.tbLzh.Padding = new System.Windows.Forms.Padding(3);
            this.tbLzh.Size = new System.Drawing.Size(821, 341);
            this.tbLzh.TabIndex = 0;
            this.tbLzh.Text = "LZH";
            this.tbLzh.UseVisualStyleBackColor = true;
            // 
            // lvLzh
            // 
            this.lvLzh.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chLzhIndex,
            this.chLzhTableAddr,
            this.chLzhDataOffset,
            this.chLzhCmpSize,
            this.chLzhDecSize});
            this.lvLzh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLzh.FullRowSelect = true;
            this.lvLzh.GridLines = true;
            this.lvLzh.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvLzh.LabelWrap = false;
            this.lvLzh.Location = new System.Drawing.Point(3, 35);
            this.lvLzh.MultiSelect = false;
            this.lvLzh.Name = "lvLzh";
            this.lvLzh.ShowGroups = false;
            this.lvLzh.Size = new System.Drawing.Size(815, 303);
            this.lvLzh.TabIndex = 0;
            this.lvLzh.UseCompatibleStateImageBehavior = false;
            this.lvLzh.View = System.Windows.Forms.View.Details;
            // 
            // chLzhIndex
            // 
            this.chLzhIndex.Text = "#";
            this.chLzhIndex.Width = 45;
            // 
            // chLzhTableAddr
            // 
            this.chLzhTableAddr.Text = "Table Address";
            this.chLzhTableAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chLzhTableAddr.Width = 122;
            // 
            // chLzhDataOffset
            // 
            this.chLzhDataOffset.Text = "Data Offset";
            this.chLzhDataOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chLzhDataOffset.Width = 115;
            // 
            // chLzhCmpSize
            // 
            this.chLzhCmpSize.Text = "Archive Size";
            this.chLzhCmpSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chLzhCmpSize.Width = 130;
            // 
            // chLzhDecSize
            // 
            this.chLzhDecSize.Text = "Data Size";
            this.chLzhDecSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chLzhDecSize.Width = 110;
            // 
            // tbRle
            // 
            this.tbRle.Location = new System.Drawing.Point(4, 23);
            this.tbRle.Name = "tbRle";
            this.tbRle.Padding = new System.Windows.Forms.Padding(3);
            this.tbRle.Size = new System.Drawing.Size(821, 341);
            this.tbRle.TabIndex = 1;
            this.tbRle.Text = "RLE";
            this.tbRle.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(835, 374);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dlgOpenRom
            // 
            this.dlgOpenRom.DefaultExt = "bin";
            this.dlgOpenRom.Filter = "Genesis ROM Files (*.bin, *.gen)|*.bin;*.gen|All Files (*.*)|*.*";
            this.dlgOpenRom.ReadOnlyChecked = true;
            this.dlgOpenRom.RestoreDirectory = true;
            this.dlgOpenRom.Title = "Please, select TF3 ROM file...";
            // 
            // dlgSaveRom
            // 
            this.dlgSaveRom.DefaultExt = "bin";
            this.dlgSaveRom.Filter = "Genesis ROM Files (*.bin)|*.bin|All Files (*.*)|*.*";
            this.dlgSaveRom.RestoreDirectory = true;
            this.dlgSaveRom.Title = "Where to save changed file?";
            // 
            // pnlLzhButtons
            // 
            this.pnlLzhButtons.AutoSize = true;
            this.pnlLzhButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLzhButtons.ColumnCount = 2;
            this.pnlLzhButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLzhButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLzhButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.pnlLzhButtons.Controls.Add(this.btnLzhImport, 1, 0);
            this.pnlLzhButtons.Controls.Add(this.btnLzhExtract, 0, 0);
            this.pnlLzhButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLzhButtons.Location = new System.Drawing.Point(3, 3);
            this.pnlLzhButtons.Margin = new System.Windows.Forms.Padding(4);
            this.pnlLzhButtons.Name = "pnlLzhButtons";
            this.pnlLzhButtons.RowCount = 1;
            this.pnlLzhButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLzhButtons.Size = new System.Drawing.Size(815, 32);
            this.pnlLzhButtons.TabIndex = 2;
            // 
            // btnLzhImport
            // 
            this.btnLzhImport.AutoSize = true;
            this.btnLzhImport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLzhImport.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLzhImport.Location = new System.Drawing.Point(78, 4);
            this.btnLzhImport.Margin = new System.Windows.Forms.Padding(4);
            this.btnLzhImport.Name = "btnLzhImport";
            this.btnLzhImport.Size = new System.Drawing.Size(59, 24);
            this.btnLzhImport.TabIndex = 1;
            this.btnLzhImport.Text = "Import";
            this.btnLzhImport.UseVisualStyleBackColor = true;
            // 
            // btnLzhExtract
            // 
            this.btnLzhExtract.AutoSize = true;
            this.btnLzhExtract.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLzhExtract.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLzhExtract.Location = new System.Drawing.Point(4, 4);
            this.btnLzhExtract.Margin = new System.Windows.Forms.Padding(4);
            this.btnLzhExtract.Name = "btnLzhExtract";
            this.btnLzhExtract.Size = new System.Drawing.Size(66, 24);
            this.btnLzhExtract.TabIndex = 0;
            this.btnLzhExtract.Text = "Extract";
            this.btnLzhExtract.UseVisualStyleBackColor = true;
            this.btnLzhExtract.Click += new System.EventHandler(this.btnLzhExtract_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 425);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.mnuMain);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.tpCompression.ResumeLayout(false);
            this.tcCompression.ResumeLayout(false);
            this.tbLzh.ResumeLayout(false);
            this.tbLzh.PerformLayout();
            this.pnlLzhButtons.ResumeLayout(false);
            this.pnlLzhButtons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuROM;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenROM;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveROMAs;
        private System.Windows.Forms.ToolStripMenuItem mnuCloseROM;
        private System.Windows.Forms.ToolStripSeparator mnuSeparator;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpCompression;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tcCompression;
        private System.Windows.Forms.TabPage tbLzh;
        private System.Windows.Forms.TabPage tbRle;
        private System.Windows.Forms.ListView lvLzh;
        private System.Windows.Forms.ColumnHeader chLzhIndex;
        private System.Windows.Forms.ColumnHeader chLzhDataOffset;
        private System.Windows.Forms.ColumnHeader chLzhTableAddr;
        private System.Windows.Forms.ColumnHeader chLzhCmpSize;
        private System.Windows.Forms.ColumnHeader chLzhDecSize;
        private System.Windows.Forms.OpenFileDialog dlgOpenRom;
        private System.Windows.Forms.SaveFileDialog dlgSaveRom;
        private System.Windows.Forms.TableLayoutPanel pnlLzhButtons;
        private System.Windows.Forms.Button btnLzhImport;
        private System.Windows.Forms.Button btnLzhExtract;
    }
}

