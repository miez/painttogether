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
using System.Windows.Forms;
using PaintTogetherStartSelector.Contracts;
using PaintTogetherStartSelector.Messages;
using System.Drawing;

namespace PaintTogetherStartSelector.Portal
{
    /// <summary>
    /// GUI des StartSelectors für Nutzereingabenvalidierung zum
    /// Start eines PT-Servers mit PT-Client oder nur eines PT-Client
    /// </summary>
    public partial class PtStartSelectorPortal : Form, IPtStartSelectorPortal
    {
        /// <summary>
        /// Der Dialog für die Einstellungen des Malbereichs
        /// </summary>
        private readonly PtPictureOptionDlg _serverDlg = new PtPictureOptionDlg();

        /// <summary>
        /// Der Dialog für die Einstellungen zum Verbinden mit einer Malerei
        /// </summary>
        private readonly PtConnectToServerOptionDlg _clientDlg = new PtConnectToServerOptionDlg();

        /// <summary>
        /// Aufforderung eine neue Malerei zu starten
        /// sowie einen Client der sich mit der neuen 
        /// Malerei verbindet
        /// </summary>
        public event Action<CreateNewPictureMessage> OnCreateNewPicture;

        /// <summary>
        /// Aufforderung einen Client für den angegebenen Server zu starten
        /// </summary>
        public event Action<ConnectToPictureMessage> OnConnectToPicture;

        /// <summary>
        /// Aufforderung den angegebenen Port auf Verfügbarkeit zu prüfen
        /// </summary>
        public event Action<TestLocalPortRequest> OnRequestTestLocalPort;

        /// <summary>
        /// Aufforderung den angegebenen Server mit Port auf
        /// Verbindungsfähigkeit zu prüfen
        /// </summary>
        public event Action<TestServerRequest> OnRequestTestServer;

        public PtStartSelectorPortal()
        {
            InitializeComponent();

            // Prüfungsaufforderungen der Dialoge weiterleiten
            _clientDlg.OnRequestTestServer += OnRequestTestServer;
            _serverDlg.OnRequestTestLocalPort += OnRequestTestLocalPort;
        }

        private Color Color { get; set; }

        private void ChooseColor()
        {
            var dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Color = dlg.Color;
                tbAlias.ForeColor = Color;
            }
        }

        private void StartServerDlg()
        {
            Hide();

            if (_serverDlg.ShowDialog() == DialogResult.OK)
            {
                OnCreateNewPicture(new CreateNewPictureMessage
                   {
                       Alias = tbAlias.Text,
                       Color = Color,
                       Port = _serverDlg.Port,
                       Size = _serverDlg.PictureSize
                   });
            }

            // Startanwendung wird nicht mehr benötigt 
            // (auch wenn Nutzer den anderen Dialog abgebrochen hat - selbst schuld)
            Close();
        }

        private void StartClientDlg()
        {
            Hide();

            if (_clientDlg.ShowDialog() == DialogResult.OK)
            {
                OnConnectToPicture(new ConnectToPictureMessage()
                {
                    Alias = tbAlias.Text,
                    Color = Color,
                    Port = _clientDlg.Port,
                    ServernameOrIp = _clientDlg.Server
                });
            }

            // Startanwendung wird nicht mehr benötigt 
            // (auch wenn Nutzer den anderen Dialog abgebrochen hat - selbst schuld)
            Close();
        }

        /// <summary>
        /// Validiert Farbe und Alias und zeigt ggf. Fehler an
        /// </summary>
        /// <returns></returns>
        private bool ValidateInputAndShowErrors()
        {
            var result = ValidateInputUtils.ValidateAlias(tbAlias.Text);
            if (!string.IsNullOrEmpty(result))
            {
                MessageBox.Show(result, "Alias fehlerhaft", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (Color == Color.Empty)
            {
                MessageBox.Show("Keine Farbe ausgewählt.", "Farbe fehlt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void BtnChooseColorClick(object sender, EventArgs e)
        {
            ChooseColor();
        }

        private void BtnStartServerClick(object sender, EventArgs e)
        {
            if (ValidateInputAndShowErrors())
            {
                StartServerDlg();
            }
        }

        private void BtnConnectToServerClick(object sender, EventArgs e)
        {
            if (ValidateInputAndShowErrors())
            {
                StartClientDlg();
            }
        }
    }
}
