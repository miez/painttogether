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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using PaintTogetherClient.Contracts;
using PaintTogetherClient.Messages.Portal;

namespace PaintTogetherClient
{
    /// <summary>
    /// GUI für den PaintTogetherClient
    /// </summary>
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

        #region IPtClientPortal Member, Kommentare am Interface

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

        /// <summary>
        /// Leert die Aliasliste
        /// </summary>
        private void RemoveAllAlias()
        {
            lstViewPainter.Items.Clear();
        }

        /// <summary>
        /// Malt eine Linie in den Malbereich
        /// </summary>
        /// <param name="message"></param>
        private void PaintLine(PaintedMessage message)
        {
            pnContentPanel.PaintLine(message.StartPoint, message.EndPoint, message.Color);
        }

        /// <summary>
        /// Entfernt einen Alias aus der Beteiligtenliste
        /// </summary>
        /// <param name="message"></param>
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

        /// <summary>
        /// Fügt einen neuen Beteiligten in die Beteiligtenliste hinzu
        /// </summary>
        /// <param name="message"></param>
        private void AddAlias(AddAliasMessage message)
        {
            lstViewPainter.Items.Add(new ListViewItem(message.Alias) { ForeColor = message.Color });
        }

        /// <summary>
        /// Clickeventbehandlung für btnTakePciture <para/>
        /// Löst das Aufnehmen eines Bildes aus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTakePictureClick(object sender, EventArgs e)
        {
            TakePicture();
        }

        /// <summary>
        /// Fordert den Anwender zum angeben einer Datei für die Speicherung
        /// des Bildstandes auf und speichert dort ein Bild des aktuellen
        /// Malbereichs
        /// </summary>
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

        /// <summary>
        /// Clickeventbehandlung für btnCloseClint<para/>
        /// Schließt die Anwendung
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCloseClientClick(object sender, EventArgs e)
        {
            Close(); // Programm schließen
        }

        /// <summary>
        /// Beim Schließen der Anwendung wird die Serververbindung getrennt
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            // Serververbindung beenden
            OnClientClose(new CloseMessage());

            base.OnClosing(e);
        }

        /// <summary>
        /// Wird ausgelöst, wenn der Anwender die Maus über den Malbereich bewegt <para/>
        /// Wenn der Anwender die Maus gedrückt hält, dann werden zwischen den einzelnen
        /// Punkten Linien gemalt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PnPaintContentMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var curPoint = new Point(e.X, e.Y);
                if (_lastMousePos.X == _lastMousePos.Y && _lastMousePos.X == -100)
                {
                    // erster Punkt, hier nicht malen, das wird durch MouseClick gemacht
                    _lastMousePos = curPoint;
                    return;
                }

                PaintNewLine(_lastMousePos,curPoint);

                _lastMousePos = curPoint;
                return;
            }

            // Markieren, das nicht gemalt werden soll
            _lastMousePos = new Point(-100, -100);
        }

        /// <summary>
        /// Wird ausgelöst, wenn der Anwender mit der Maus auf den Malbereich klickt.
        /// Das löst das Malen eines "Punktes" an der Stelle aus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PnContentPanelMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Da zur Zeit nur das Malen von Strichen vorgesehen ist, muss
                // damit auch ein einfacher Punkt gemalt werden kann, ein 2Pixel langer
                // Strich gemalt werden. Später könnte man noch eine Nachricht für das 
                // malen von Punkten erstellen
                PaintNewLine(new Point(e.X, e.Y), new Point(e.X + 1, e.Y));
            }
        }

        private void PaintNewLine(Point fstPnt, Point sndPtn)
        {
            // Im Clientbereich schon einmal malen, ohne das die Bestätigung vom Server kommt
            pnContentPanel.PaintLine(fstPnt, sndPtn, ClientColor);

            // Malanfrage in extra Thread damit GUI nicht hackt
            var thread = new Thread(SendOnPaint);
            thread.Start(new PaintSelfMessage { Color = ClientColor, StartPoint = fstPnt, EndPoint = sndPtn });
        }

        /// <summary>
        /// Sendet eine neu gemalte Linie an den Server
        /// </summary>
        /// <param name="message"></param>
        private void SendOnPaint(object message)
        {
            OnPaintSelf(message as PaintSelfMessage);
        }

        /// <summary>
        /// Initialisiert den Malbereich
        /// </summary>
        /// <param name="message"></param>
        private void InitContent(InitPortalMessage message)
        {
            // titel der Anwendung setzen
            Text = string.Concat("PaintTogether - Malerei von ", message.ServerAlias, " auf ", message.ServerName, ":", message.ServerPort);
            Alias = message.Alias;
            ClientColor = message.Color;

            pnContentPanel.InitPaintContent(message.PaintContent);
        }

        /// <summary>
        /// Wird ausgelöst, wenn die Größe des Malbereichs geändert wird und
        /// reorganisiert die Größe des Clientfensters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnContentPanel_SizeChanged(object sender, EventArgs e)
        {
            ResizeForm();
        }

        /// <summary>
        /// Bestimmt anhand der Größe des Malbereich die Größe für die Clientanwendung
        /// </summary>
        private void ResizeForm()
        {
            var size = new Size(pnContentPanel.Width + pnRight.Width + _winBorderWidth, pnContentPanel.Height + _winBorderHeight);
            Size = size;
            MinimumSize = size;
            MaximumSize = size;
        }
    }
}
