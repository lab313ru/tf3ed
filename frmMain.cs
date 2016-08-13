using System;
using System.IO;
using System.Windows.Forms;
using Helpers;
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
            int offset = Consts.LzhTable1 + 2;
            for (int i = 0; i < Consts.LzhTable1Items; ++i)
            {
                int data_offset = (int)rom.ReadLong(offset); offset += 4;
                int dec_size = (int)rom.ReadLong(offset); offset += 4;
                int enc_size = (int)rom.ReadLong(offset); offset += 4;

                ListViewItem item = new ListViewItem(
                    new string[]
                    {
                        (i + 1).ToString("D2"),
                        string.Format("0x{0:X6}", Consts.LzhTable1),
                        string.Format("0x{0:X6}", Consts.LzhTable1 + data_offset),
                        string.Format("0x{0:X4} ({1})", enc_size, enc_size),
                        string.Format("0x{0:X4} ({1})", dec_size, dec_size),
                    });
                item.Tag = new Tuple<int, int, int>(Consts.LzhTable1 + data_offset, enc_size, dec_size);

                lvLzh.Items.Add(item);
            }
        }

        byte[] ExtractLzhData(int offset, int decodedSize)
        {
            byte[] data;
            LzhCompressor.DecompressData(rom, offset, out data, decodedSize);
            return data;
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
            lvLzh.Items[0].Selected = true;
            lvLzh.Select();
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

        private void btnLzhExtract_Click(object sender, EventArgs e)
        {
            if (lvLzh.SelectedItems.Count == 0) return;

            Tuple<int, int, int> item_data = (Tuple<int, int, int>)lvLzh.SelectedItems[0].Tag;

            string name = string.Format("lzh_{0:X6}_{1:D}.bin", item_data.Item1, lvLzh.SelectedItems[0].Index + 1);
            File.WriteAllBytes(name, ExtractLzhData(item_data.Item1, item_data.Item3));
            MessageBox.Show(string.Format("Successfully extracted to \"{0}\"", name), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
