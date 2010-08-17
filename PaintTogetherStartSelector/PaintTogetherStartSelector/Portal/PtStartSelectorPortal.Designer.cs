namespace PaintTogetherStartSelector.Portal
{
    partial class PtStartSelectorPortal
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
            this.btnStartServer = new System.Windows.Forms.Button();
            this.btnChooseColor = new System.Windows.Forms.Button();
            this.lbColor = new System.Windows.Forms.Label();
            this.lbAlias = new System.Windows.Forms.Label();
            this.tbAlias = new System.Windows.Forms.TextBox();
            this.lbHint1 = new System.Windows.Forms.Label();
            this.lbHint2 = new System.Windows.Forms.Label();
            this.lbHint3 = new System.Windows.Forms.Label();
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
            // btnConnectToServer
            // 
            this.btnConnectToServer.Location = new System.Drawing.Point(10, 170);
            this.btnConnectToServer.Name = "btnConnectToServer";
            this.btnConnectToServer.Size = new System.Drawing.Size(150, 23);
            this.btnConnectToServer.TabIndex = 1;
            this.btnConnectToServer.Text = "An Malerei beteiligen";
            this.btnConnectToServer.UseVisualStyleBackColor = true;
            this.btnConnectToServer.Click += new System.EventHandler(this.BtnConnectToServerClick);
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(166, 170);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(150, 23);
            this.btnStartServer.TabIndex = 2;
            this.btnStartServer.Text = "Neue Malerei beginnen";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.BtnStartServerClick);
            // 
            // btnChooseColor
            // 
            this.btnChooseColor.Location = new System.Drawing.Point(169, 122);
            this.btnChooseColor.Name = "btnChooseColor";
            this.btnChooseColor.Size = new System.Drawing.Size(150, 23);
            this.btnChooseColor.TabIndex = 3;
            this.btnChooseColor.Text = "Farbe wählen";
            this.btnChooseColor.UseVisualStyleBackColor = true;
            this.btnChooseColor.Click += new System.EventHandler(this.BtnChooseColorClick);
            // 
            // lbColor
            // 
            this.lbColor.AutoSize = true;
            this.lbColor.Location = new System.Drawing.Point(13, 127);
            this.lbColor.Name = "lbColor";
            this.lbColor.Size = new System.Drawing.Size(34, 13);
            this.lbColor.TabIndex = 4;
            this.lbColor.Text = "Farbe";
            // 
            // lbAlias
            // 
            this.lbAlias.AutoSize = true;
            this.lbAlias.Location = new System.Drawing.Point(13, 99);
            this.lbAlias.Name = "lbAlias";
            this.lbAlias.Size = new System.Drawing.Size(29, 13);
            this.lbAlias.TabIndex = 5;
            this.lbAlias.Text = "Alias";
            // 
            // tbAlias
            // 
            this.tbAlias.Location = new System.Drawing.Point(169, 96);
            this.tbAlias.Name = "tbAlias";
            this.tbAlias.Size = new System.Drawing.Size(150, 20);
            this.tbAlias.TabIndex = 6;
            // 
            // lbHint1
            // 
            this.lbHint1.AutoSize = true;
            this.lbHint1.Location = new System.Drawing.Point(18, 20);
            this.lbHint1.Name = "lbHint1";
            this.lbHint1.Size = new System.Drawing.Size(276, 13);
            this.lbHint1.TabIndex = 7;
            this.lbHint1.Text = "Willkommen bei PaintTogether, Malen Sie zusammen mit ";
            // 
            // lbHint2
            // 
            this.lbHint2.AutoSize = true;
            this.lbHint2.Location = new System.Drawing.Point(18, 42);
            this.lbHint2.Name = "lbHint2";
            this.lbHint2.Size = new System.Drawing.Size(240, 13);
            this.lbHint2.TabIndex = 8;
            this.lbHint2.Text = "an einer Malerei. Nehmen Sie an einer Malerei teil";
            // 
            // lbHint3
            // 
            this.lbHint3.AutoSize = true;
            this.lbHint3.Location = new System.Drawing.Point(18, 64);
            this.lbHint3.Name = "lbHint3";
            this.lbHint3.Size = new System.Drawing.Size(149, 13);
            this.lbHint3.TabIndex = 9;
            this.lbHint3.Text = "oder erstellen Sie eine eigene.";
            // 
            // PtStartSelectorPortal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 205);
            this.Controls.Add(this.lbHint3);
            this.Controls.Add(this.lbHint2);
            this.Controls.Add(this.lbHint1);
            this.Controls.Add(this.tbAlias);
            this.Controls.Add(this.lbAlias);
            this.Controls.Add(this.lbColor);
            this.Controls.Add(this.btnChooseColor);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.btnConnectToServer);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(343, 243);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(343, 243);
            this.Name = "PtStartSelectorPortal";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PaintTogether";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnConnectToServer;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Button btnChooseColor;
        private System.Windows.Forms.Label lbColor;
        private System.Windows.Forms.Label lbAlias;
        private System.Windows.Forms.TextBox tbAlias;
        private System.Windows.Forms.Label lbHint1;
        private System.Windows.Forms.Label lbHint2;
        private System.Windows.Forms.Label lbHint3;
    }
}