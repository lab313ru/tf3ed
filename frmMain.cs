using System;
using System.IO;
using System.Windows.Forms;
using tf3ed.tools;

namespace tf3ed
{
    public partial class frmMain : Form
    {
        const string title = "TF3Ed v1.0";

        bool romOpened = false;
        string romPath = string.Empty;
        byte[] rom = null;

        bool romChanged
        {
            get { return mnuSaveROMAs.Enabled; }
            set { Text = string.Format("{0} [{1}]" + (value ? "*" : string.Empty), title, romPath).Replace("[]", string.Empty); mnuSaveROMAs.Enabled = value; }
        }

        void ResetVariables()
        {
            rom = null;
            romOpened = false;
            romPath = string.Empty;
            Text = title;
            romChanged = false;
        }

        void OpenRom(string path)
        {
            romOpened = true;
            romPath = path;

            rom = File.ReadAllBytes(path);
            romChanged = false;
        }

        void FillLzhOffsetsTable()
        {
            // 0x20000

        }

        void CloseApp(object sender, EventArgs e)
        {
            mnuCloseROM_Click(sender, e);
            Application.Exit();
        }

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ResetVariables();
        }

        private void mnuOpenROM_Click(object sender, EventArgs e)
        {
            if (romOpened) mnuCloseROM_Click(sender, e);
            romOpened = dlgOpenRom.ShowDialog() == DialogResult.OK;
            if (!romOpened) return;

            OpenRom(dlgOpenRom.FileName);
            FillLzhOffsetsTable();
        }

        private void mnuCloseROM_Click(object sender, EventArgs e)
        {
            ResetVariables();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            CloseApp(sender, e);
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseApp(sender, e);
        }
    }
}
