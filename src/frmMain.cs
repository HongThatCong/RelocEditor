using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace RelocEditor
{
    public partial class frmMain : Form
    {
        private Relocations rel = null;
        private int pageIndex = 0;
        private UInt64 baseAddress = 0;
        private string argPath = "";

        public frmMain(string[] args)
        {
            InitializeComponent();

            if (args.Length > 0)
                argPath = args[0];
        }

        private void mnuMainFileExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuMainHelpAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This program has been coded by gta126 + HTC for x64", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuMainFileOpen_Click(object sender, EventArgs e)
        {
            string path;
            if (!string.IsNullOrEmpty(argPath))
            {
                path = argPath;
                argPath = "";
            }
            else
            {
                dlgOpen.Title = "Select an executable file";
                dlgOpen.Filter = "Executable (*.exe)|*.exe" +
                      "|Dynamic Link Library (*.dll)|*.dll" +
                      "|Drivers (*.sys)|*.sys" +
                      "|Windows Visual Style (*.msstyles)|*.msstyles" +
                      "|Configuration Panel Widget (*.cpl)|*.cpl" +
                      "|ActiveX Library (*.ocx)|*.ocx" +
                      "|ActiveX Cache Library (*.oca)|*.oca" +
                      "|Multi User Interface (*.mui)|*.mui" +
                      "|Codecs (*.acm, *.ax)|*.acm;*.ax" +
                      "|Borland / Delphi Library (*.bpl, *.dpl)|*.bpl;*.dpl" +
                      "|Screensaver (*.scr)|*.scr" +
                      "|All Executables (*.exe, *.dll, *.sys, *.msstyles, *.cpl, *.ocx, *.oca, *.mui, *.acm, *.ax, *.bpl, *.dpl, *.scr)|*.exe;*.dll;*.sys;*.msstyles;*.cpl;*.ocx;*.oca;*.mui;*.acm;*.ax;*.bpl;*.dpl;*.scr" +
                      "|All Files (*.*)|*.*";

                if (dlgOpen.ShowDialog() != DialogResult.OK)
                    return;

                path = dlgOpen.FileName;
            }

            try
            {
                rel = new Relocations(path);

                this.Text = "Relocation Editor - " + rel.GetPath();

                RefreshData();

                cmnuPages.Enabled = true;
                cmnuRelocations.Enabled = true;
                mnuMainFileSaveAs.Enabled = true;
                mnuMainFileSave.Enabled = true;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lvPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPage.SelectedItems.Count < 1)
                return;

            lvRelocation.Items.Clear();

            UInt64 address = (UInt64)lvPage.SelectedItems[0].Tag;
            baseAddress = address;
            pageIndex = lvPage.SelectedItems[0].Index;

            if (!rel.TryGetRelocs(address, out List<Relocations.Reloc> relocs))
                return;

            UInt64 itemBase = baseAddress - rel.GetImageBase();

            foreach (Relocations.Reloc reloc in relocs)
            {
                ListViewItem item;

                if (reloc.type == PeHeader.BASE_RELOCATION_TYPE.IMAGE_REL_BASED_ABSOLUTE)
                    item = new ListViewItem("0x" + reloc.offset.ToString("X8"));
                else
                    item = new ListViewItem("0x" + (address + reloc.offset).ToString("X8"));
                item.SubItems.Add((rel.GetRawAddress() + reloc.offset).ToString("X8"));
                item.SubItems.Add(reloc.offset.ToString("X8"));
                item.SubItems.Add((itemBase + reloc.offset).ToString("X4"));
                item.SubItems.Add(reloc.type.ToString());
                item.Tag = reloc;

                lvRelocation.Items.Add(item);
            }
        }

        private void cmuRelocationsDelete_Click(object sender, EventArgs e)
        {
            if (lvRelocation.SelectedIndices.Count < 1)
                return;

            Relocations.Reloc reloc = (Relocations.Reloc)lvRelocation.SelectedItems[0].Tag;
            if (MessageBox.Show("Are you sure to delete the relocation address \"" + (baseAddress + reloc.offset) + "\" ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                return;

            rel.DeleteRelocation(baseAddress + reloc.offset);

            RefreshData();
        }

        private void RefreshData()
        {
            int index = lvPage.SelectedIndices.Count < 1 ? 0 : lvPage.SelectedIndices[0];
            lvPage.Items.Clear();
            lvRelocation.Items.Clear();

            foreach (Relocations.Page page in rel.GetPages())
            {
                ListViewItem item = new ListViewItem("0x" + page.address.ToString("X8"));
                item.SubItems.Add("0x" + (page.address - rel.GetImageBase()).ToString("X8"));
                item.SubItems.Add("0x" + page.size.ToString("X8"));
                item.SubItems.Add(page.count.ToString());
                item.Tag = page.address;

                lvPage.Items.Add(item);
            }

            if (lvPage.Items.Count > 0)
            {
                if (index < lvPage.Items.Count)
                    lvPage.Items[index].Selected = true;
                else
                    lvPage.Items[0].Selected = true;
            }

            RefreshSize();
        }

        private void RefreshSize()
        {
            staLblCurrentSize.Text = "Current reloc section size: 0x" + rel.GetVirtualSize().ToString("X8");
            staLblMaxSize.Text = "Max reloc section size: 0x" + rel.GetRawSize().ToString("X8");
            staLblImageBase.Text = "Image base: 0x" + rel.GetImageBase().ToString("X8");
            staLblVirtualAddress.Text = "Virtual address: 0x" + rel.GetVirtualAddress().ToString("X8");

            staPbSize.Minimum = 0;
            staPbSize.Maximum = (int)rel.GetRawSize();
            staPbSize.Value = (int)rel.GetVirtualSize();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            cmnuPages.Enabled = false;
            cmnuRelocations.Enabled = false;
            mnuMainFileSaveAs.Enabled = false;
            mnuMainFileSave.Enabled = false;

            if (!string.IsNullOrEmpty(argPath))
                mnuMainFileOpen_Click(null, null);
        }

        private void cmnuPagesDelete_Click(object sender, EventArgs e)
        {
            if (lvPage.SelectedItems.Count < 1)
                return;

            if (MessageBox.Show("Are you sure to delete this page ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                return;

            foreach (ListViewItem item in lvRelocation.Items)
            {
                Relocations.Reloc reloc = (Relocations.Reloc)item.Tag;
                rel.DeleteRelocation(baseAddress + reloc.offset);
            }

            lvRelocation.Items.Clear();
            lvPage.Items[lvPage.SelectedIndices[0]].Remove();

            RefreshSize();
        }

        private void mnuAdd_Click(object sender, EventArgs e)
        {
            frmAddRelocation frm = new frmAddRelocation();

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            int code = rel.AddRelocation(frm.GetAddress(), frm.GetRelocType());

            switch (code)
            {
                case -1:
                    MessageBox.Show("This address is already in the relocation table", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case 0:
                    MessageBox.Show("Cannot add this address (0x" + frm.GetAddress().ToString("X8") + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case 1:
                case 2:
                    RefreshData();
                    break;
                default:
                    MessageBox.Show("Unknown error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

        }

        private void mnuRelocationsEdit_Click(object sender, EventArgs e)
        {
            if (lvRelocation.SelectedItems.Count < 1)
                return;

            Relocations.Reloc reloc = (Relocations.Reloc)lvRelocation.SelectedItems[0].Tag;
            frmEditRelocation frm = new frmEditRelocation(baseAddress + reloc.offset, reloc.type);

            if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            if (!rel.EditRelocation(frm.GetOldAddress(), frm.GetNewAddress(), frm.GetRelocType()))
            {
                MessageBox.Show("Cannot edit this address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshData();
        }

        private void mnuMainFileSave_Click(object sender, EventArgs e)
        {
            if (!rel.WriteRelocations())
                MessageBox.Show("File not saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rel != null && rel.IsNotSaved)
            {
                if (MessageBox.Show("Would you really exit this program without save the data ?", "Not Saved", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void mnuMainFileSaveAs_Click(object sender, EventArgs e)
        {
            dlgSave.Filter = "Executable (*.exe)|*.exe" +
                      "|Dynamic Link Library (*.dll)|*.dll" +
                      "|Drivers (*.sys)|*.sys" +
                      "|Windows Visual Style (*.msstyles)|*.msstyles" +
                      "|Configuration Panel Widget (*.cpl)|*.cpl" +
                      "|ActiveX Library (*.ocx)|*.ocx" +
                      "|ActiveX Cache Library (*.oca)|*.oca" +
                      "|Multi User Interface (*.mui)|*.mui" +
                      "|Codecs (*.acm, *.ax)|*.acm;*.ax" +
                      "|Borland / Delphi Library (*.bpl, *.dpl)|*.bpl;*.dpl" +
                      "|Screensaver (*.scr)|*.scr" +
                      "|All Executables (*.exe, *.dll, *.sys, *.msstyles, *.cpl, *.ocx, *.oca, *.mui, *.acm, *.ax, *.bpl, *.dpl, *.scr)|*.exe;*.dll;*.sys;*.msstyles;*.cpl;*.ocx;*.oca;*.mui;*.acm;*.ax;*.bpl;*.dpl;*.scr" +
                      "|All Files (*.*)|*.*";

            switch (Path.GetExtension(rel.GetPath()))
            {
                case ".exe":
                    dlgSave.FilterIndex = 1;
                    break;
                case ".dll":
                    dlgSave.FilterIndex = 2;
                    break;
                case ".sys":
                    dlgSave.FilterIndex = 3;
                    break;
                case ".msstyles":
                    dlgSave.FilterIndex = 4;
                    break;
                case ".cpl":
                    dlgSave.FilterIndex = 5;
                    break;
                case ".ocx":
                    dlgSave.FilterIndex = 6;
                    break;
                case ".oca":
                    dlgSave.FilterIndex = 7;
                    break;
                case ".mui":
                    dlgSave.FilterIndex = 8;
                    break;
                case ".acm":
                case ".ax":
                    dlgSave.FilterIndex = 9;
                    break;
                case ".bpl":
                case ".dpl":
                    dlgSave.FilterIndex = 10;
                    break;
                case ".scr":
                    dlgSave.FilterIndex = 11;
                    break;
            }

            if (dlgSave.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            if (!rel.WriteRelocations(dlgSave.FileName))
                MessageBox.Show("File not saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                this.Text = "Relocation Editor - " + rel.GetPath();
        }
    }
}
