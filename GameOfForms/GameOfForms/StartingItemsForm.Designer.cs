namespace GameOfForms
{
    partial class StartingItemsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartingItemsForm));
            this.AccpetButton = new System.Windows.Forms.Button();
            this.RightHandListBox = new System.Windows.Forms.ListBox();
            this.LeftHandListBox = new System.Windows.Forms.ListBox();
            this.RightHandItemLabel = new System.Windows.Forms.Label();
            this.LeftHandItemLabel = new System.Windows.Forms.Label();
            this.HeadItemLabel = new System.Windows.Forms.Label();
            this.ChestItemLabel = new System.Windows.Forms.Label();
            this.LegsItemLabel = new System.Windows.Forms.Label();
            this.RightHandItemDesc = new System.Windows.Forms.Label();
            this.LeftHandItemDesc = new System.Windows.Forms.Label();
            this.LegsPictureBox = new System.Windows.Forms.PictureBox();
            this.ChestPictureBox = new System.Windows.Forms.PictureBox();
            this.RightHandPictureBox = new System.Windows.Forms.PictureBox();
            this.LeftHandPictureBox = new System.Windows.Forms.PictureBox();
            this.HeadPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LegsPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChestPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightHandPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LeftHandPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeadPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // AccpetButton
            // 
            resources.ApplyResources(this.AccpetButton, "AccpetButton");
            this.AccpetButton.BackColor = System.Drawing.SystemColors.MenuText;
            this.AccpetButton.Name = "AccpetButton";
            this.AccpetButton.UseVisualStyleBackColor = false;
            this.AccpetButton.Click += new System.EventHandler(this.AccpetButton_Click);
            // 
            // RightHandListBox
            // 
            resources.ApplyResources(this.RightHandListBox, "RightHandListBox");
            this.RightHandListBox.BackColor = System.Drawing.SystemColors.MenuText;
            this.RightHandListBox.ForeColor = System.Drawing.SystemColors.Menu;
            this.RightHandListBox.FormattingEnabled = true;
            this.RightHandListBox.Items.AddRange(new object[] {
            resources.GetString("RightHandListBox.Items"),
            resources.GetString("RightHandListBox.Items1")});
            this.RightHandListBox.Name = "RightHandListBox";
            this.RightHandListBox.SelectedIndexChanged += new System.EventHandler(this.RightHandListBox_SelectedIndexChanged);
            // 
            // LeftHandListBox
            // 
            resources.ApplyResources(this.LeftHandListBox, "LeftHandListBox");
            this.LeftHandListBox.BackColor = System.Drawing.SystemColors.MenuText;
            this.LeftHandListBox.ForeColor = System.Drawing.SystemColors.Menu;
            this.LeftHandListBox.FormattingEnabled = true;
            this.LeftHandListBox.Items.AddRange(new object[] {
            resources.GetString("LeftHandListBox.Items"),
            resources.GetString("LeftHandListBox.Items1")});
            this.LeftHandListBox.Name = "LeftHandListBox";
            this.LeftHandListBox.SelectedIndexChanged += new System.EventHandler(this.LeftHandListBox_SelectedIndexChanged);
            // 
            // RightHandItemLabel
            // 
            resources.ApplyResources(this.RightHandItemLabel, "RightHandItemLabel");
            this.RightHandItemLabel.Name = "RightHandItemLabel";
            // 
            // LeftHandItemLabel
            // 
            resources.ApplyResources(this.LeftHandItemLabel, "LeftHandItemLabel");
            this.LeftHandItemLabel.Name = "LeftHandItemLabel";
            // 
            // HeadItemLabel
            // 
            resources.ApplyResources(this.HeadItemLabel, "HeadItemLabel");
            this.HeadItemLabel.Name = "HeadItemLabel";
            // 
            // ChestItemLabel
            // 
            resources.ApplyResources(this.ChestItemLabel, "ChestItemLabel");
            this.ChestItemLabel.Name = "ChestItemLabel";
            // 
            // LegsItemLabel
            // 
            resources.ApplyResources(this.LegsItemLabel, "LegsItemLabel");
            this.LegsItemLabel.Name = "LegsItemLabel";
            // 
            // RightHandItemDesc
            // 
            resources.ApplyResources(this.RightHandItemDesc, "RightHandItemDesc");
            this.RightHandItemDesc.Name = "RightHandItemDesc";
            // 
            // LeftHandItemDesc
            // 
            resources.ApplyResources(this.LeftHandItemDesc, "LeftHandItemDesc");
            this.LeftHandItemDesc.Name = "LeftHandItemDesc";
            // 
            // LegsPictureBox
            // 
            resources.ApplyResources(this.LegsPictureBox, "LegsPictureBox");
            this.LegsPictureBox.BackColor = System.Drawing.SystemColors.MenuText;
            this.LegsPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LegsPictureBox.Image = global::GameOfForms.Properties.Resources.EmptyItemSlot;
            this.LegsPictureBox.Name = "LegsPictureBox";
            this.LegsPictureBox.TabStop = false;
            // 
            // ChestPictureBox
            // 
            resources.ApplyResources(this.ChestPictureBox, "ChestPictureBox");
            this.ChestPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ChestPictureBox.Image = global::GameOfForms.Properties.Resources.EmptyItemSlot;
            this.ChestPictureBox.Name = "ChestPictureBox";
            this.ChestPictureBox.TabStop = false;
            // 
            // RightHandPictureBox
            // 
            resources.ApplyResources(this.RightHandPictureBox, "RightHandPictureBox");
            this.RightHandPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.RightHandPictureBox.Image = global::GameOfForms.Properties.Resources.EmptyItemSlot;
            this.RightHandPictureBox.Name = "RightHandPictureBox";
            this.RightHandPictureBox.TabStop = false;
            this.RightHandPictureBox.Click += new System.EventHandler(this.RightHandPictureBox_Click);
            // 
            // LeftHandPictureBox
            // 
            resources.ApplyResources(this.LeftHandPictureBox, "LeftHandPictureBox");
            this.LeftHandPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LeftHandPictureBox.Image = global::GameOfForms.Properties.Resources.EmptyItemSlot;
            this.LeftHandPictureBox.Name = "LeftHandPictureBox";
            this.LeftHandPictureBox.TabStop = false;
            // 
            // HeadPictureBox
            // 
            resources.ApplyResources(this.HeadPictureBox, "HeadPictureBox");
            this.HeadPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.HeadPictureBox.Image = global::GameOfForms.Properties.Resources.EmptyItemSlot;
            this.HeadPictureBox.Name = "HeadPictureBox";
            this.HeadPictureBox.TabStop = false;
            // 
            // StartingItemsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuText;
            this.Controls.Add(this.LeftHandItemDesc);
            this.Controls.Add(this.RightHandItemDesc);
            this.Controls.Add(this.LegsItemLabel);
            this.Controls.Add(this.ChestItemLabel);
            this.Controls.Add(this.HeadItemLabel);
            this.Controls.Add(this.LeftHandItemLabel);
            this.Controls.Add(this.RightHandItemLabel);
            this.Controls.Add(this.LeftHandListBox);
            this.Controls.Add(this.RightHandListBox);
            this.Controls.Add(this.AccpetButton);
            this.Controls.Add(this.LegsPictureBox);
            this.Controls.Add(this.ChestPictureBox);
            this.Controls.Add(this.RightHandPictureBox);
            this.Controls.Add(this.LeftHandPictureBox);
            this.Controls.Add(this.HeadPictureBox);
            this.ForeColor = System.Drawing.SystemColors.Menu;
            this.Name = "StartingItemsForm";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.LegsPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChestPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightHandPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LeftHandPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeadPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox HeadPictureBox;
        private System.Windows.Forms.PictureBox LeftHandPictureBox;
        private System.Windows.Forms.PictureBox RightHandPictureBox;
        private System.Windows.Forms.PictureBox ChestPictureBox;
        private System.Windows.Forms.PictureBox LegsPictureBox;
        private System.Windows.Forms.Button AccpetButton;
        private System.Windows.Forms.ListBox RightHandListBox;
        private System.Windows.Forms.ListBox LeftHandListBox;
        private System.Windows.Forms.Label RightHandItemLabel;
        private System.Windows.Forms.Label LeftHandItemLabel;
        private System.Windows.Forms.Label HeadItemLabel;
        private System.Windows.Forms.Label ChestItemLabel;
        private System.Windows.Forms.Label LegsItemLabel;
        private System.Windows.Forms.Label RightHandItemDesc;
        private System.Windows.Forms.Label LeftHandItemDesc;
    }
}