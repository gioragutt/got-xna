namespace GameOfForms
{
    partial class NameForm
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
            this.NameTB = new System.Windows.Forms.TextBox();
            this.ConnectNameB = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NameTB
            // 
            this.NameTB.BackColor = System.Drawing.SystemColors.MenuText;
            this.NameTB.ForeColor = System.Drawing.SystemColors.Menu;
            this.NameTB.Location = new System.Drawing.Point(59, 37);
            this.NameTB.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.NameTB.Name = "NameTB";
            this.NameTB.Size = new System.Drawing.Size(195, 26);
            this.NameTB.TabIndex = 0;
            this.NameTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ConnectNameB
            // 
            this.ConnectNameB.BackColor = System.Drawing.SystemColors.MenuText;
            this.ConnectNameB.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ConnectNameB.ForeColor = System.Drawing.SystemColors.Menu;
            this.ConnectNameB.Location = new System.Drawing.Point(303, 32);
            this.ConnectNameB.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.ConnectNameB.Name = "ConnectNameB";
            this.ConnectNameB.Size = new System.Drawing.Size(119, 37);
            this.ConnectNameB.TabIndex = 1;
            this.ConnectNameB.Text = "Accept";
            this.ConnectNameB.UseVisualStyleBackColor = false;
            this.ConnectNameB.Click += new System.EventHandler(this.ConnectNameB_Click);
            // 
            // NameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(471, 99);
            this.Controls.Add(this.ConnectNameB);
            this.Controls.Add(this.NameTB);
            this.Font = new System.Drawing.Font("Aharoni", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.Name = "NameForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hello";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NameTB;
        private System.Windows.Forms.Button ConnectNameB;
    }
}

