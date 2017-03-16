namespace VRGoHelper
{
    partial class Main
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
            this.CommodityContent = new System.Windows.Forms.DataGridView();
            this.Buttton_Save = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CommodityContent)).BeginInit();
            this.SuspendLayout();
            // 
            // CommodityContent
            // 
            this.CommodityContent.BackgroundColor = System.Drawing.SystemColors.ActiveBorder;
            this.CommodityContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CommodityContent.GridColor = System.Drawing.SystemColors.ButtonFace;
            this.CommodityContent.Location = new System.Drawing.Point(12, 12);
            this.CommodityContent.Name = "CommodityContent";
            this.CommodityContent.RowTemplate.Height = 23;
            this.CommodityContent.Size = new System.Drawing.Size(1817, 620);
            this.CommodityContent.TabIndex = 0;
            // 
            // Buttton_Save
            // 
            this.Buttton_Save.Location = new System.Drawing.Point(793, 638);
            this.Buttton_Save.Name = "Buttton_Save";
            this.Buttton_Save.Size = new System.Drawing.Size(75, 23);
            this.Buttton_Save.TabIndex = 1;
            this.Buttton_Save.Text = "保存";
            this.Buttton_Save.UseVisualStyleBackColor = true;
            this.Buttton_Save.Click += new System.EventHandler(this.Buttton_Save_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1841, 673);
            this.Controls.Add(this.Buttton_Save);
            this.Controls.Add(this.CommodityContent);
            this.Name = "Main";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.CommodityContent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView CommodityContent;
        private System.Windows.Forms.Button Buttton_Save;
    }
}

