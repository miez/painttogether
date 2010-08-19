namespace PaintTogetherStartSelector.Portal
{
    partial class PtPictureOptionDlg
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.lbWidth = new System.Windows.Forms.Label();
            this.lbHeight = new System.Windows.Forms.Label();
            this.tbHeight = new System.Windows.Forms.TextBox();
            this.lbHint1 = new System.Windows.Forms.Label();
            this.lbHint2 = new System.Windows.Forms.Label();
            this.tbWidth = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(1, 155);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(326, 1);
            this.panel1.TabIndex = 0;
            // 
            // btnStartServer
            // 
            this.btnStartServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartServer.Location = new System.Drawing.Point(166, 170);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(150, 23);
            this.btnStartServer.TabIndex = 2;
            this.btnStartServer.Text = "Malerei beginnen";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.BtnStartServerClick);
            // 
            // lbWidth
            // 
            this.lbWidth.AutoSize = true;
            this.lbWidth.Location = new System.Drawing.Point(13, 127);
            this.lbWidth.Name = "lbWidth";
            this.lbWidth.Size = new System.Drawing.Size(76, 13);
            this.lbWidth.TabIndex = 4;
            this.lbWidth.Text = "Breite (in Pixel)";
            // 
            // lbHeight
            // 
            this.lbHeight.AutoSize = true;
            this.lbHeight.Location = new System.Drawing.Point(13, 99);
            this.lbHeight.Name = "lbHeight";
            this.lbHeight.Size = new System.Drawing.Size(75, 13);
            this.lbHeight.TabIndex = 5;
            this.lbHeight.Text = "Höhe (in Pixel)";
            // 
            // tbHeight
            // 
            this.tbHeight.Location = new System.Drawing.Point(169, 96);
            this.tbHeight.Name = "tbHeight";
            this.tbHeight.Size = new System.Drawing.Size(150, 20);
            this.tbHeight.TabIndex = 6;
            this.tbHeight.Text = "350";
            // 
            // lbHint1
            // 
            this.lbHint1.AutoSize = true;
            this.lbHint1.Location = new System.Drawing.Point(18, 20);
            this.lbHint1.Name = "lbHint1";
            this.lbHint1.Size = new System.Drawing.Size(255, 13);
            this.lbHint1.TabIndex = 7;
            this.lbHint1.Text = "Sie können bestimmen, wie groß der Malbereich sein";
            // 
            // lbHint2
            // 
            this.lbHint2.AutoSize = true;
            this.lbHint2.Location = new System.Drawing.Point(18, 42);
            this.lbHint2.Name = "lbHint2";
            this.lbHint2.Size = new System.Drawing.Size(206, 13);
            this.lbHint2.TabIndex = 8;
            this.lbHint2.Text = "und unter welchem Port Sie gestartet wird.";
            // 
            // tbWidth
            // 
            this.tbWidth.Location = new System.Drawing.Point(169, 127);
            this.tbWidth.Name = "tbWidth";
            this.tbWidth.Size = new System.Drawing.Size(150, 20);
            this.tbWidth.TabIndex = 9;
            this.tbWidth.Text = "750";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(169, 70);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(150, 20);
            this.tbPort.TabIndex = 10;
            this.tbPort.Text = "6969";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Port (Standardport 6969)";
            // 
            // PtPictureOptionDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 205);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.tbWidth);
            this.Controls.Add(this.lbHint2);
            this.Controls.Add(this.lbHint1);
            this.Controls.Add(this.tbHeight);
            this.Controls.Add(this.lbHeight);
            this.Controls.Add(this.lbWidth);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(343, 243);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(343, 243);
            this.Name = "PtPictureOptionDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PaintTogether Malereieinstellungen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Label lbWidth;
        private System.Windows.Forms.Label lbHeight;
        private System.Windows.Forms.TextBox tbHeight;
        private System.Windows.Forms.Label lbHint1;
        private System.Windows.Forms.Label lbHint2;
        private System.Windows.Forms.TextBox tbWidth;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label label1;
    }
}