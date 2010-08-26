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
using PaintTogetherClient.Messages.Core.ClientStarter;
using PaintTogetherClient.Contracts;
using System.Windows.Forms;

namespace PaintTogetherClient.Run
{
    /// <summary>
    /// Clientplatine die einen Pt-Client darstellt. Dazu werden
    /// die 3 benötigten EBC "PtClientCore", "PTClientPortal" sowie "PtClientServerApdater"
    /// verdrahtet
    /// </summary>
    internal class Client
    {
        /// <summary>
        /// Einziger Outputpin dieser Klasse (es handelt sich hier auch um eine EBC)
        /// </summary>
        public Action<StartClientRequest> OnStartClient;

        /// <summary>
        /// Die Portal-EBC des Clients - eine Windowsform
        /// </summary>
        private readonly IPtClientPortal _portal = new PtClientPortal();

        internal IPtClientPortal Portal { get { return _portal; } }

        /// <summary>
        /// Die ClientServerAdapter-EBC zur Verwaltung der Serververbindung
        /// </summary>
        private readonly IPtClientServerAdapter _adapter = new PtClientServerAdapter();

        /// <summary>
        /// Die funktionale EBC für die inhaltliche Datenverarbeitung des Malclients
        /// </summary>
        private readonly IPtClientCore _core = new PtClientCore();

        internal Client()
        {
            // die 3 EBCs verbinden, dabei einfach von allen EBC die Outpins (Events)
            // mit einen entsprechenden Inputpin (Process..-Methode) verbinden
            _portal.OnClientClose += message => _core.ProcessCloseMessage(message);
            _portal.OnPaintSelf += message => _core.ProcessPaintSelfMessage(message);
            _portal.OnRequestTakePicture += request => _core.ProcessTakePictureRequest(request);
            _core.OnAddAlias += message => _portal.ProcessAddAliasMessage(message);
            _core.OnCloseConnection += message => _adapter.ProcessCloseConnectionMessage(message);
            _core.OnInitPortal += message => _portal.ProcessInitPortalMessage(message);
            _core.OnNewPaint += message => _adapter.ProcessNewPaintMessage(message);
            _core.OnPainted += message => _portal.ProcessPaintedMessage(message);
            _core.OnRemoveAlias += message => _portal.ProcessRemoveAliasMessage(message);
            _core.OnRequestConnectToServer += request => _adapter.ProcessConnectToServerRequest(request);
            _core.OnServerClosed += message => _portal.ProcessServerClosedMessage(message);
            _adapter.OnAliasDisconnected += message => _core.ProcessAliasDisconnectedMessage(message);
            _adapter.OnAliasPainted += message => _core.ProcessAliasPaintedMessage(message);
            _adapter.OnConnected += message => _core.ProcessConnectedMessage(message);
            _adapter.OnCurrentPaintContent += message => _core.ProcessCurrentPaintContentMessage(message);
            _adapter.OnNewAlias += message => _core.ProcessNewAliasMessage(message);
            _adapter.OnServerConnectionLost += message => _core.ProcessServerConLostMessage(message);
            // Jetzt muss noch der offene Input-Pin "ProcessStartClientMessage" der CoreEBC
            // bedient werden. Da es sich bei der Clientklasse hier eigentlich auch um eine
            // Platine handelt, habe ich mit "OnStartClient" den passenden Outputpin für
            // die ClientCoreEBC geschaffen. nun verbinden
            OnStartClient += message => _core.ProcessStartClientRequest(message);
        }

        /// <summary>
        /// Startet den PaintTogetherClient
        /// </summary>
        /// <param name="startParams"></param>
        internal void Start(StartClientParams startParams)
        {
            var request = new StartClientRequest
            {
                Alias = startParams.Alias,
                ServernameOrIp = startParams.Server,
                Color = startParams.Color,
                Port = startParams.Port
            };

            OnStartClient(request);

            MessageBox.Show(request.Result, "Information über Clientstart", MessageBoxButtons.OK);
        }
    }
}
