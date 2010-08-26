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
using PaintTogetherStartSelector.Messages;

namespace PaintTogetherStartSelector.Portal
{
    /// <summary>
    /// Dialog zum Eingeben der Daten die für einen Clientstart
    /// erforderlich sind
    /// </summary>
    public partial class PtConnectToServerOptionDlg : Form
    {
        /// <summary>
        /// Aufforderung den angegebenen Server mit Port auf
        /// Verbindungsfähigkeit zu prüfen
        /// </summary>
        public event Action<TestServerRequest> OnRequestTestServer;

        /// <summary>
        /// Der vom Nutzer ausgewählte Port,
        /// wenn DialogResult = OK, dann wird hier
        /// ein validerer Port zurückgegeben
        /// </summary>
        public int Port
        {
            get
            {
                return Int32.Parse(tbPort.Text);
            }
        }

        /// <summary>
        /// Der vom Nutzer ausgewählte Server,
        /// wenn DialogResult = OK, dann wird hier
        /// ein erreichbarer Server zurückgegeben
        /// </summary>
        public string Server
        {
            get
            {
                return tbServer.Text;
            }
        }

        public PtConnectToServerOptionDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Überprüft ob zu dem vom Nutzer angegebenem Rechner und dem
        /// angegebenem Port eine Connection aufgebaut werden kann
        /// </summary>
        /// <returns></returns>
        private bool TestServer()
        {
            var request = new TestServerRequest();
            request.Port = Port;
            request.ServernameOrIp = Server;

            OnRequestTestServer(request);

            return request.Result;
        }

        /// <summary>
        /// Validiert die Eingaben des Nutzers und zeigt ggf. einen Fehler an
        /// </summary>
        /// <returns></returns>
        private bool ValidateInputsAndShowErrors()
        {
            var result = ValidateInputUtils.ValidatePort(tbPort.Text);
            if (!string.IsNullOrEmpty(result))
            {
                MessageBox.Show(result, "Port fehlerhaft", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if(string.IsNullOrEmpty(tbServer.Text))
            {
                MessageBox.Show("Sie müssen einen Server angeben.", "Kein Server angegeben", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// ClickEventBehandlung von btnConnectToServer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConnectToServerClick(object sender, EventArgs e)
        {
            if (ValidateInputsAndShowErrors())
            {
                if (TestServer())
                {
                    DialogResult = DialogResult.OK;
                    Close(); // Beenden damits im Portal weiter geht
                }
                else
                {
                    MessageBox.Show("Der angegebene Server mit dem angegeben Port ist nicht verfügbar.", "Server nicht erreichbar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
