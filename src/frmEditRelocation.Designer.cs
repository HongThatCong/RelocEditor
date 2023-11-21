namespace RelocEditor
{
    partial class frmEditRelocation
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblHexa = new System.Windows.Forms.Label();
            this.lblNewAddress = new System.Windows.Forms.Label();
            this.lblHexa2 = new System.Windows.Forms.Label();
            this.txtNewAddress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(254, 122);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(124, 122);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 28);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cboType
            // 
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.FormattingEnabled = true;
            this.cboType.Items.AddRange(new object[] {
            "IMAGE_REL_BASED_HIGH",
            "IMAGE_REL_BASED_LOW",
            "IMAGE_REL_BASED_HIGHLOW",
            "IMAGE_REL_BASED_HIGHADJ",
            "IMAGE_REL_BASED_MIPS_JMPADDR",
            "IMAGE_REL_BASED_SECTION",
            "IMAGE_REL_BASED_REL32",
            "IMAGE_REL_BASED_RESERVED",
            "IMAGE_REL_BASED_MIPS_JMPADDR16",
            "IMAGE_REL_BASED_DIR64"});
            this.cboType.Location = new System.Drawing.Point(124, 79);
            this.cboType.Margin = new System.Windows.Forms.Padding(4);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(230, 24);
            this.cboType.TabIndex = 4;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(52, 82);
            this.lblType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(44, 17);
            this.lblType.TabIndex = 3;
            this.lblType.Text = "Type:";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(124, 15);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtAddress.MaxLength = 8;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.ReadOnly = true;
            this.txtAddress.Size = new System.Drawing.Size(230, 22);
            this.txtAddress.TabIndex = 9;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(33, 18);
            this.lblAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(64, 17);
            this.lblAddress.TabIndex = 7;
            this.lblAddress.Text = "Address:";
            // 
            // lblHexa
            // 
            this.lblHexa.AutoSize = true;
            this.lblHexa.Location = new System.Drawing.Point(103, 18);
            this.lblHexa.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHexa.Name = "lblHexa";
            this.lblHexa.Size = new System.Drawing.Size(22, 17);
            this.lblHexa.TabIndex = 8;
            this.lblHexa.Text = "0x";
            // 
            // lblNewAddress
            // 
            this.lblNewAddress.AutoSize = true;
            this.lblNewAddress.Location = new System.Drawing.Point(0, 50);
            this.lblNewAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNewAddress.Name = "lblNewAddress";
            this.lblNewAddress.Size = new System.Drawing.Size(95, 17);
            this.lblNewAddress.TabIndex = 0;
            this.lblNewAddress.Text = "New Address:";
            // 
            // lblHexa2
            // 
            this.lblHexa2.AutoSize = true;
            this.lblHexa2.Location = new System.Drawing.Point(103, 50);
            this.lblHexa2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHexa2.Name = "lblHexa2";
            this.lblHexa2.Size = new System.Drawing.Size(22, 17);
            this.lblHexa2.TabIndex = 1;
            this.lblHexa2.Text = "0x";
            // 
            // txtNewAddress
            // 
            this.txtNewAddress.Location = new System.Drawing.Point(124, 47);
            this.txtNewAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtNewAddress.MaxLength = 16;
            this.txtNewAddress.Name = "txtNewAddress";
            this.txtNewAddress.Size = new System.Drawing.Size(230, 22);
            this.txtNewAddress.TabIndex = 2;
            this.txtNewAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNewAddress_KeyPress);
            this.txtNewAddress.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtNewAddress_KeyUp);
            // 
            // frmEditRelocation
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(372, 163);
            this.Controls.Add(this.txtNewAddress);
            this.Controls.Add(this.lblHexa2);
            this.Controls.Add(this.lblNewAddress);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.lblHexa);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditRelocation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Relocation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblHexa;
        private System.Windows.Forms.Label lblNewAddress;
        private System.Windows.Forms.Label lblHexa2;
        private System.Windows.Forms.TextBox txtNewAddress;
    }
}