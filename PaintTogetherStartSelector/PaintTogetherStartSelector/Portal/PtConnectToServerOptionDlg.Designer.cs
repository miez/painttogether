namespace PaintTogetherStartSelector.Portal
{
    partial class PtConnectToServerOptionDlg
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
            this.btnConnectToServer = new System.Windows.Forms.Button();
            this.lbServer = new System.Windows.Forms.Label();
            this.lbHint1 = new System.Windows.Forms.Label();
            this.lbHint2 = new System.Windows.Forms.Label();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(1, 133);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(326, 1);
            this.panel1.TabIndex = 0;
            // 
            // btnConnectToServer
            // 
            this.btnConnectToServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnectToServer.Location = new System.Drawing.Point(166, 148);
            this.btnConnectToServer.Name = "btnConnectToServer";
            this.btnConnectToServer.Size = new System.Drawing.Size(150, 23);
            this.btnConnectToServer.TabIndex = 2;
            this.btnConnectToServer.Text = "Der Malerei beitreten";
            this.btnConnectToServer.UseVisualStyleBackColor = true;
            this.btnConnectToServer.Click += new System.EventHandler(this.BtnConnectToServerClick);
            // 
            // lbServer
            // 
            this.lbServer.AutoSize = true;
            this.lbServer.Location = new System.Drawing.Point(12, 78);
            this.lbServer.Name = "lbServer";
            this.lbServer.Size = new System.Drawing.Size(122, 13);
            this.lbServer.TabIndex = 4;
            this.lbServer.Text = "Rechner (Name oder IP)";
            // 
            // lbHint1
            // 
            this.lbHint1.AutoSize = true;
            this.lbHint1.Location = new System.Drawing.Point(18, 20);
            this.lbHint1.Name = "lbHint1";
            this.lbHint1.Size = new System.Drawing.Size(296, 13);
            this.lbHint1.TabIndex = 7;
            this.lbHint1.Text = "Geben Sie hier bitte den Servernamen oder IP des Rechners,";
            // 
            // lbHint2
            // 
            this.lbHint2.AutoSize = true;
            this.lbHint2.Location = new System.Drawing.Point(18, 42);
            this.lbHint2.Name = "lbHint2";
            this.lbHint2.Size = new System.Drawing.Size(267, 13);
            this.lbHint2.TabIndex = 8;
            this.lbHint2.Text = "auf dem die Malerei geonnen wurde sowie den Port an.";
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(169, 75);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(150, 20);
            this.tbServer.TabIndex = 9;
            this.tbServer.Text = "localhost";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(169, 101);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(150, 20);
            this.tbPort.TabIndex = 10;
            this.tbPort.Text = "6969";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Port (Standardport 6969)";
            // 
            // PtConnectToServerOptionDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 183);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.tbServer);
            this.Controls.Add(this.lbHint2);
            this.Controls.Add(this.lbHint1);
            this.Controls.Add(this.lbServer);
            this.Controls.Add(this.btnConnectToServer);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(343, 221);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(343, 221);
            this.Name = "PtConnectToServerOptionDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PaintTogether Malereisuche";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnConnectToServer;
        private System.Windows.Forms.Label lbServer;
        private System.Windows.Forms.Label lbHint1;
        private System.Windows.Forms.Label lbHint2;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label label1;
    }
}