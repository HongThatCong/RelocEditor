using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RelocEditor
{
    public partial class frmEditRelocation : Form
    {
        private UInt64 oldAddress;
        private UInt64 newAddress;
        private PeHeader.BASE_RELOCATION_TYPE type;

        public frmEditRelocation(UInt64 oldAddress, PeHeader.BASE_RELOCATION_TYPE oldType)
        {
            InitializeComponent();

            this.oldAddress = oldAddress;

            txtAddress.Text = oldAddress.ToString("X8");
            cboType.SelectedIndex = (int)oldType - 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void txtNewAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            string chars = "0123456789abcdefABCDEF";

            if (e.KeyChar == (char)Keys.Back)
                return;

            if (!chars.Contains(e.KeyChar.ToString()))
                e.Handled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!UInt64.TryParse(txtNewAddress.Text, System.Globalization.NumberStyles.AllowHexSpecifier, null, out newAddress))
            {
                MessageBox.Show("\"" + txtNewAddress.Text.ToUpper() + "\" isn't a valid address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            type = (PeHeader.BASE_RELOCATION_TYPE)(cboType.SelectedIndex + 1);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public UInt64 GetOldAddress()
        {
            return oldAddress;
        }

        public UInt64 GetNewAddress()
        {
            return newAddress;
        }

        public PeHeader.BASE_RELOCATION_TYPE GetRelocType()
        {
            return type;
        }

        private void txtNewAddress_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                txtNewAddress.Text = Clipboard.GetText();
                txtNewAddress.SelectionStart = txtNewAddress.Text.Length;
                txtNewAddress.SelectionLength = 0;
            }
        }
    }
}
