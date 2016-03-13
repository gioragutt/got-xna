namespace GameOfForms
{
    partial class IpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IpForm));
            this.txtIp = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.IpLabel = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.grpTeamSelect = new System.Windows.Forms.GroupBox();
            this.radHolyCow = new System.Windows.Forms.RadioButton();
            this.radGOAT = new System.Windows.Forms.RadioButton();
            this.grpTeamSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtIp
            // 
            this.txtIp.BackColor = System.Drawing.SystemColors.Control;
            this.txtIp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIp.ForeColor = System.Drawing.SystemColors.InfoText;
            this.txtIp.Location = new System.Drawing.Point(127, 55);
            this.txtIp.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(159, 24);
            this.txtIp.TabIndex = 0;
            this.txtIp.TextChanged += new System.EventHandler(this.txtIp_TextChanged);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.SystemColors.Control;
            this.btnConnect.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.ForeColor = System.Drawing.SystemColors.InfoText;
            this.btnConnect.Location = new System.Drawing.Point(13, 146);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(113, 37);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // IpLabel
            // 
            this.IpLabel.AutoSize = true;
            this.IpLabel.ForeColor = System.Drawing.SystemColors.InfoText;
            this.IpLabel.Location = new System.Drawing.Point(13, 56);
            this.IpLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.IpLabel.Name = "IpLabel";
            this.IpLabel.Size = new System.Drawing.Size(84, 18);
            this.IpLabel.TabIndex = 2;
            this.IpLabel.Text = "Ip Address:";
            this.IpLabel.Click += new System.EventHandler(this.IpLabel_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.ForeColor = System.Drawing.SystemColors.InfoText;
            this.lblName.Location = new System.Drawing.Point(13, 18);
            this.lblName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(55, 18);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.SystemColors.Control;
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.ForeColor = System.Drawing.SystemColors.InfoText;
            this.txtName.Location = new System.Drawing.Point(127, 15);
            this.txtName.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtName.MaxLength = 10;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(159, 24);
            this.txtName.TabIndex = 4;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.ForeColor = System.Drawing.SystemColors.InfoText;
            this.btnExit.Location = new System.Drawing.Point(171, 146);
            this.btnExit.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(113, 37);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // grpTeamSelect
            // 
            this.grpTeamSelect.Controls.Add(this.radHolyCow);
            this.grpTeamSelect.Controls.Add(this.radGOAT);
            this.grpTeamSelect.Location = new System.Drawing.Point(15, 84);
            this.grpTeamSelect.Name = "grpTeamSelect";
            this.grpTeamSelect.Size = new System.Drawing.Size(269, 55);
            this.grpTeamSelect.TabIndex = 7;
            this.grpTeamSelect.TabStop = false;
            this.grpTeamSelect.Text = "Choose team";
            // 
            // radHolyCow
            // 
            this.radHolyCow.AutoSize = true;
            this.radHolyCow.ForeColor = System.Drawing.SystemColors.InfoText;
            this.radHolyCow.Location = new System.Drawing.Point(157, 24);
            this.radHolyCow.Name = "radHolyCow";
            this.radHolyCow.Size = new System.Drawing.Size(89, 22);
            this.radHolyCow.TabIndex = 1;
            this.radHolyCow.TabStop = true;
            this.radHolyCow.Text = "Holy cow";
            this.radHolyCow.UseVisualStyleBackColor = true;
            // 
            // radGOAT
            // 
            this.radGOAT.AutoSize = true;
            this.radGOAT.ForeColor = System.Drawing.SystemColors.InfoText;
            this.radGOAT.Location = new System.Drawing.Point(23, 24);
            this.radGOAT.Name = "radGOAT";
            this.radGOAT.Size = new System.Drawing.Size(66, 22);
            this.radGOAT.TabIndex = 0;
            this.radGOAT.TabStop = true;
            this.radGOAT.Text = "GOAT";
            this.radGOAT.UseVisualStyleBackColor = true;
            // 
            // IpForm
            // 
            this.AcceptButton = this.btnConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(296, 192);
            this.Controls.Add(this.grpTeamSelect);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.IpLabel);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtIp);
            this.Font = new System.Drawing.Font("Berlin Sans FB Demi", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.Menu;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "IpForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IpForm";
            this.grpTeamSelect.ResumeLayout(false);
            this.grpTeamSelect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label IpLabel;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox grpTeamSelect;
        private System.Windows.Forms.RadioButton radHolyCow;
        private System.Windows.Forms.RadioButton radGOAT;
    }
}