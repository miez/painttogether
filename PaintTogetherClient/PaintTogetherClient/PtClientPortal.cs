﻿/* $HeadURL: http://ws201075/svn/deg/dotNet/trunk/Pegasus/src/HostApplicationForm.cs $
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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using PaintTogetherClient.Contracts;
using PaintTogetherClient.Messages.Portal;

namespace PaintTogetherClient
{
    public partial class PtClientPortal : Form, IPtClientPortal
    {
        /// <summary>
        /// Dieses Objekt hilft dabei, dass bei allen Inputpins die
        /// Daten einfach in den Hauptthread übergeben werden können ohne
        /// explizit ein Invoke aufrufen zu müssen
        /// </summary>
        private readonly SynchronizationContext _synchContext;

        /// <summary>
        /// Breite der beiden Ränder (rechts, links) für spätere Größenbestimmung
        /// </summary>
        private readonly int _winBorderWidth;

        /// <summary>
        /// Höhe der beiden Ränder (oben, unten) für spätere Größenbestimmung
        /// </summary>
        private readonly int _winBorderHeight;

        public PtClientPortal()
        {
            InitializeComponent();

            _synchContext = SynchronizationContext.Current;
            _winBorderWidth = Width - pnContentPanel.Width - pnRight.Width;
            _winBorderHeight = Height - pnContentPanel.Height;
        }

        /// <summary>
        /// Der Alias des Client-Nutzers
        /// </summary>
        private string Alias { get; set; }

        /// <summary>
        /// Die Malfarbe des Client-Nutzers
        /// </summary>
        private Color ClientColor { get; set; }

        /// <summary>
        /// Der zuletzt mit gedrückter linke Maustaste berüherte Punkt
        /// wenn -100, -100, dann war linke maustaste zuletzt nicht gedrückt
        /// </summary>
        private Point _lastMousePos = new Point(-100, -100);

        #region IPtClientPortal Member

        public event Action<TakePictureRequest> OnRequestTakePicture;

        public event Action<CloseMessage> OnClientClose;

        public event Action<PaintSelfMessage> OnPaintSelf;

        public void ProcessInitPortalMessage(InitPortalMessage message)
        {
            // Malbereich in dem Hauptthread initialisieren
            _synchContext.Post(dummy => InitContent(message), message);
        }

        public void ProcessPaintedMessage(PaintedMessage message)
        {
            // Malvorgang im Hauptthread durchführen
            _synchContext.Post(dummy => PaintLine(message), message);
        }

        public void ProcessServerClosedMessage(ServerClosedMessage message)
        {
            MessageBox.Show("Verbindung zum Server verloren. Beenden Sie den Client, alle weiteren Malaktionen finden nur lokal statt.", "Serververbindung verloren", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Leeren der Beteiligtenliste im Hauptthread durchführen
            _synchContext.Post(dummy => RemoveAllAlias(), message);
        }

        public void ProcessAddAliasMessage(AddAliasMessage message)
        {
            // Hinzufügen eines neuen Beteiligten im Hauptthread durchführen
            _synchContext.Post(dummy => AddAlias(message), message);
        }

        public void ProcessRemoveAliasMessage(RemoveAliasMessage message)
        {
            // Entfernen eines Beteiligten im Hauptthread durchführen
            _synchContext.Post(dummy => RemoveAlias(message), message);
        }
        #endregion

        private void RemoveAllAlias()
        {
            lstViewPainter.Items.Clear();
        }

        private void PaintLine(PaintedMessage message)
        {
            pnContentPanel.PaintLine(message.StartPoint, message.EndPoint, message.Color);
        }

        private void RemoveAlias(RemoveAliasMessage message)
        {
            ListViewItem toRemoveItem = null;
            foreach (var item in lstViewPainter.Items)
            {
                if ((item as ListViewItem).ForeColor == message.Color
                    && (item as ListViewItem).Text == message.Alias)
                {
                    toRemoveItem = item as ListViewItem;
                    break;
                }
            }

            if (toRemoveItem != null)
            {
                lstViewPainter.Items.Remove(toRemoveItem);
            }
        }

        private void AddAlias(AddAliasMessage message)
        {
            lstViewPainter.Items.Add(new ListViewItem(message.Alias) { ForeColor = message.Color });
        }

        private void BtnTakePictureClick(object sender, EventArgs e)
        {
            TakePicture();
        }

        private void TakePicture()
        {
            var saveDlg = new SaveFileDialog();
            saveDlg.AddExtension = true;
            saveDlg.Filter = "PaintTogether-Bild|*.png";
            saveDlg.OverwritePrompt = true;

            if (saveDlg.ShowDialog(this) == DialogResult.OK)
            {
                var request = new TakePictureRequest { Filename = saveDlg.FileName };
                OnRequestTakePicture(request);
                MessageBox.Show(request.Result, "Information über Speichervorgang", MessageBoxButtons.OK);
            }
        }

        private void BtnCloseClientClick(object sender, EventArgs e)
        {
            Close(); // Programm schließen
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Serververbindung beenden
            OnClientClose(new CloseMessage());

            base.OnClosing(e);
        }

        private void PnPaintContentMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var curPoint = new Point(e.X, e.Y);
                if (_lastMousePos.X == _lastMousePos.Y && _lastMousePos.X == -100)
                {
                    // erster Punkt, noch nicht malen
                    _lastMousePos = curPoint;
                    return;
                }

                pnContentPanel.PaintLine(_lastMousePos, curPoint, ClientColor);

                // Malanfrage in extra Thread damit GUI nicht hackt
                var thread = new Thread(SendOnPaint);
                thread.Start(new PaintSelfMessage { Color = ClientColor, StartPoint = _lastMousePos, EndPoint = curPoint });

                _lastMousePos = curPoint;
                return;
            }

            // Markieren, das nicht gemalt werden soll
            _lastMousePos = new Point(-100, -100);
        }

        private void SendOnPaint(object message)
        {
            OnPaintSelf(message as PaintSelfMessage);
        }

        private void InitContent(InitPortalMessage message)
        {
            // titel der Anwendung setzen
            Text = string.Concat("PaintTogether - Malerei von ", message.ServerAlias, " auf ", message.ServerName, ":", message.ServerPort);
            Alias = message.Alias;
            ClientColor = message.Color;

            pnContentPanel.InitPaintContent(message.PaintContent);
        }

        private void pnContentPanel_SizeChanged(object sender, EventArgs e)
        {
            ResizeForm();
        }

        private void ResizeForm()
        {
            var size = new Size(pnContentPanel.Width + pnRight.Width + _winBorderWidth, pnContentPanel.Height + _winBorderHeight);
            Size = size;
            MinimumSize = size;
            MaximumSize = size;
        }
    }
}
