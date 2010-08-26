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
using System.Drawing;
using System.Windows.Forms;
using PaintTogetherStartSelector.Messages;

namespace PaintTogetherStartSelector.Portal
{
    /// <summary>
    /// Dialog zur Eingabe der für die Erstellung eines Pt-Servers benötigten
    /// Daten.
    /// </summary>
    public partial class PtPictureOptionDlg : Form
    {
        /// <summary>
        /// Aufforderung den angegebenen Port auf Verfügbarkeit zu prüfen
        /// </summary>
        public event Action<TestLocalPortRequest> OnRequestTestLocalPort;

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
        /// Die vom Nutzer eingebenen GRöße für die Malerei,
        /// wenn DialogResult = OK, dann wurde eine gültige Größe
        /// eingebenen, die hier abrufbar ist
        /// </summary>
        public Size PictureSize
        {
            get
            {
                return new Size(Int32.Parse(tbWidth.Text), Int32.Parse(tbHeight.Text));
            }
        }

        public PtPictureOptionDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Überprüft die Eingaben des Nutzers und zeigt bei Fehlern einen Hinweis an
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

            result = ValidateInputUtils.ValidateHeight(tbHeight.Text);
            if (!string.IsNullOrEmpty(result))
            {
                MessageBox.Show(result, "Höhe fehlerhaft", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            result = ValidateInputUtils.ValidateWidth(tbWidth.Text);
            if (!string.IsNullOrEmpty(result))
            {
                MessageBox.Show(result, "Breite fehlerhaft", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Testet den vom Nutzer angegeben Port auf Verfügbarkeit
        /// </summary>
        /// <returns>true, wenn Port frei</returns>
        private bool TestPort()
        {
            var request = new TestLocalPortRequest();
            request.Port = Port;

            OnRequestTestLocalPort(request);

            return request.Result;
        }

        /// <summary>
        /// ClickEventbehandlung für btnStartServer <para/>
        /// Validiert die Eingaben des Nutzers und prüft den angegeben
        /// Port und bei Fehlern einen Hinweis an. Bei Erfolg schließt sich der
        /// Dialog, wobei die Eingaben des Nutzers weiterhin am Dialog abgefragt
        /// werden können
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStartServerClick(object sender, EventArgs e)
        {
            if (ValidateInputsAndShowErrors())
            {
                if (TestPort())
                {
                    DialogResult = DialogResult.OK;
                    Close(); // Beenden damits im Portal weiter geht
                }
                else
                {
                    MessageBox.Show("Der angegebene Port wird bereits von einer\nanderen Applikation verwendet. Geben Sie einen anderen Port an.", "Port nicht verfügbar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
