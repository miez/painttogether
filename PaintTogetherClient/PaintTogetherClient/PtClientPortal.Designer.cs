/* $HeadURL: http://ws201075/svn/deg/dotNet/trunk/Pegasus/src/HostApplicationForm.cs $
-----------------------------------------------------------------------------
        (c) by Martin Blankenstein

Dieses Dokument und die hierin enthaltenen Informationen unterliegen
dem Urheberrecht und duerfen ohne die schriftliche Genehmigung des
Herausgebers weder als ganzes noch in Teilen dupliziert oder reproduziert
noch manipuliert werden.

-----------------+------------------------------------------------------------
Version          : $Revision: 450 $
-----------------+------------------------------------------------------------
Last Change      : $LastChangedDate: 2009-02-23 18:26:54 +0100 (Mo, 23 Feb 2009) $
-----------------+------------------------------------------------------------
Last User        : $LastChangedBy: NLBERLIN\mblankenstein $
-----------------+------------------------------------------------------------
Beschreibung     :
-----------------+ 

-----------------+------------------------------------------------------------
Updates          :
-----------------+

$Id: HostApplicationForm.cs 450 2009-02-23 17:26:54Z NLBERLIN\mblankenstein $

*/


namespace PaintTogetherClient
{
    partial class PtClientPortal
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
            this.pnRight = new System.Windows.Forms.Panel();
            this.btnCloseClient = new System.Windows.Forms.Button();
            this.btnTakePicture = new System.Windows.Forms.Button();
            this.lstViewPainter = new System.Windows.Forms.ListView();
            this.lbPainter = new System.Windows.Forms.Label();
            this.pnContentPanel = new PaintTogetherClient.Portal.PaintContentPanel();
            this.pnRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnRight
            // 
            this.pnRight.Controls.Add(this.btnCloseClient);
            this.pnRight.Controls.Add(this.btnTakePicture);
            this.pnRight.Controls.Add(this.lstViewPainter);
            this.pnRight.Controls.Add(this.lbPainter);
            this.pnRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnRight.Location = new System.Drawing.Point(506, 0);
            this.pnRight.MinimumSize = new System.Drawing.Size(139, 50);
            this.pnRight.Name = "pnRight";
            this.pnRight.Size = new System.Drawing.Size(139, 333);
            this.pnRight.TabIndex = 1;
            // 
            // btnCloseClient
            // 
            this.btnCloseClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseClient.Location = new System.Drawing.Point(9, 307);
            this.btnCloseClient.Name = "btnCloseClient";
            this.btnCloseClient.Size = new System.Drawing.Size(118, 23);
            this.btnCloseClient.TabIndex = 3;
            this.btnCloseClient.Text = "&Beenden";
            this.btnCloseClient.UseVisualStyleBackColor = true;
            this.btnCloseClient.Click += new System.EventHandler(this.BtnCloseClientClick);
            // 
            // btnTakePicture
            // 
            this.btnTakePicture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTakePicture.Location = new System.Drawing.Point(9, 278);
            this.btnTakePicture.Name = "btnTakePicture";
            this.btnTakePicture.Size = new System.Drawing.Size(118, 23);
            this.btnTakePicture.TabIndex = 2;
            this.btnTakePicture.Text = "&Photo aufnehmen";
            this.btnTakePicture.UseVisualStyleBackColor = true;
            this.btnTakePicture.Click += new System.EventHandler(this.BtnTakePictureClick);
            // 
            // lstViewPainter
            // 
            this.lstViewPainter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstViewPainter.Location = new System.Drawing.Point(0, 25);
            this.lstViewPainter.Name = "lstViewPainter";
            this.lstViewPainter.Size = new System.Drawing.Size(139, 247);
            this.lstViewPainter.TabIndex = 1;
            this.lstViewPainter.UseCompatibleStateImageBehavior = false;
            // 
            // lbPainter
            // 
            this.lbPainter.AutoSize = true;
            this.lbPainter.Location = new System.Drawing.Point(6, 9);
            this.lbPainter.Name = "lbPainter";
            this.lbPainter.Size = new System.Drawing.Size(50, 13);
            this.lbPainter.TabIndex = 0;
            this.lbPainter.Text = "Beteiligte";
            // 
            // pnContentPanel
            // 
            this.pnContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnContentPanel.Location = new System.Drawing.Point(0, 0);
            this.pnContentPanel.Name = "pnContentPanel";
            this.pnContentPanel.Size = new System.Drawing.Size(506, 333);
            this.pnContentPanel.TabIndex = 2;
            this.pnContentPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PnPaintContentMouseMove);
            this.pnContentPanel.SizeChanged += new System.EventHandler(this.pnContentPanel_SizeChanged);
            // 
            // PtClientPortal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 333);
            this.Controls.Add(this.pnContentPanel);
            this.Controls.Add(this.pnRight);
            this.Name = "PtClientPortal";
            this.Text = "PtClientPortal";
            this.pnRight.ResumeLayout(false);
            this.pnRight.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnRight;
        private System.Windows.Forms.Button btnCloseClient;
        private System.Windows.Forms.Button btnTakePicture;
        private System.Windows.Forms.ListView lstViewPainter;
        private System.Windows.Forms.Label lbPainter;
        private PaintTogetherClient.Portal.PaintContentPanel pnContentPanel;

    }
}