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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaintTogetherServer.Messages.Core;

namespace PaintTogetherServer.Run
{
    internal class Server
    {
        /// <summary>
        /// Einziger Outputpin dieser Klasse (es handelt sich hier auch um eine EBC)
        /// </summary>
        public Action<StartServerMessage> OnStartServer;

        /// <summary>
        /// Die Portal-EBC des Servers (hier nur eine Hilfsklasse für die Konsolenanwendung)
        /// </summary>
        private readonly PtServerPortal _portal = new PtServerPortal();

        /// <summary>
        /// Die ServerClientAdapter-EBC zur Verwaltung der Clientverbindungen
        /// </summary>
        private readonly PtServerClientAdapter _adapter = new PtServerClientAdapter();

        /// <summary>
        /// Die funktionale EBC für die inhaltliche Datenverarbeitung des Malservers
        /// </summary>
        private readonly PtServerCore _core = new PtServerCore();

        internal Server()
        {
            // die 3 EBCs verbinden, dabei einfach von allen EBC die Outpins (Events)
            // mit einen entsprechenden Inputpin (Process..-Methode) verbinden
            _portal.OnServerClose += _core.ProcessCloseMessage;
            _core.OnDisconnectAllClients += _adapter.ProcessDisconnectAllClientsMessage;
            _core.OnNotifyClientDisconnected += _adapter.ProcessNotifyClientDisconnectedMessage;
            _core.OnNotifyNewClient += _adapter.ProcessNotifyNewClientMessage;
            _core.OnNotifyPaint += _adapter.ProcessNotifyPaintMessage;
            _core.OnSLog += _portal.ProcessSLogMessage;
            _core.OnInitAdapter += _adapter.ProcessInitAdapterMessage;
            _adapter.OnClientDisconnected += _core.ProcessClientDisconnectedMessage;
            _adapter.OnClientPainted += _core.ProcessClientPainted;
            _adapter.OnNewClient += _core.ProcessNewClientMessage;
            _adapter.OnRequestCurPaintContent += _core.ProcessGetCurrentPaintContentRequest;
            _adapter.OnRequestCurPainter += _core.ProcessGetCurrentPainterRequest;

            // Jetzt muss noch der offene Input-Pin "ProcessStartServerMessage" der CoreEBC
            // bedient werden. Da es sich bei der Serverklasse hier eigentlich auch um eine
            // Platine handelt, habe ich mit "OnStartServer" den passenden Outputpin für
            // die ServerCoreEBC geschaffen. nun verbinden
            OnStartServer += _core.ProcessStartServerMessage;
        }

        /// <summary>
        /// Startet den PaintTogetherServer
        /// </summary>
        /// <param name="startParams"></param>
        internal void Start(StartServerParams startParams)
        {
            OnStartServer(new StartServerMessage
                  {
                      Height = startParams.Height,
                      Width = startParams.Width,
                      Port = startParams.Port,
                      Alias = startParams.Alias
                  });
        }
    }
}
