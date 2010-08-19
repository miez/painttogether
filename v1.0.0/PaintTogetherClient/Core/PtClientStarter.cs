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

using PaintTogetherClient.Contracts.Core;
using PaintTogetherClient.Messages.Adapter;
using PaintTogetherClient.Messages.Core.ClientStarter;
using PaintTogetherClient.Messages.Core.PaintContentManager;
using PaintTogetherClient.Messages.Portal;
using log4net;

namespace PaintTogetherClient.Core
{
    /// <summary>
    /// EBC die den Start und die Initialisierung des Clients beauftragt
    /// </summary>
    internal class PtClientStarter : IPtClientStarter
    {
        #region Outputpins
        /// <summary>
        /// Aufforderung das Portal zu initialisieren
        /// </summary>
        public event Action<InitPortalMessage> OnInitPortal;

        /// <summary>
        /// Aufforderung eine Verbindung zum Server aufzubauen
        /// </summary>
        public event Action<ConnectToServerRequest> OnRequestConnectToServer;

        /// <summary>
        /// Aufforderung den Malbereich zu initialisieren
        /// </summary>
        public event Action<InitPaintManagerMessage> OnInitPaintManager;
        #endregion

        /// <summary>
        /// Anfragedaten für Init-Outputs speichern
        /// </summary>
        private StartClientRequest _startClientRequest;

        /// <summary>
        /// Daten für Init-Outputs speichern
        /// </summary>
        private CurrentPaintContentMessage _curPaintContentMessage;

        /// <summary>
        /// Daten für Init-Outputs speichern
        /// </summary>
        private ConnectedMessage _connectedMessage;

        /// <summary>
        /// log4net-Logger
        /// </summary>
        private static ILog Log
        {
            get
            {
                return LogManager.GetLogger("PtClientStarter");
            }
        }

        /// <summary>
        /// Verarbeitet die Information mit dem initialen Malstand
        /// </summary>
        /// <param name="message"></param>
        public void ProcessCurrentPaintContentMessage(CurrentPaintContentMessage message)
        {
            if (_startClientRequest == null)
            {
                throw new InvalidOperationException("CurrentPaintContent-Nachricht kann erst nach StartClient-Beauftragung verarbeitet werden");
            }

            if (_curPaintContentMessage != null)
            {
                throw new InvalidOperationException("Initialer Malbereich kann wurde schon verarbeitet");
            }

            _curPaintContentMessage = message;
            TryInit();
        }

        /// <summary>
        /// Erst wenn alle für die Portalinitialisierung vorhandenen Daten vorliegen,
        /// kann 
        /// </summary>
        private void TryInit()
        {
            Log.Debug("Es wird versucht den Client zu initialisieren");
            if (_curPaintContentMessage != null && _connectedMessage != null)
            {
                OnInitPaintManager(new InitPaintManagerMessage { PaintContent = _curPaintContentMessage.PaintContent });

                var initPortalMessage = new InitPortalMessage
                {
                    Alias = _startClientRequest.Alias,
                    Color = _startClientRequest.Color,
                    PaintContent = _curPaintContentMessage.PaintContent,
                    ServerAlias = _connectedMessage.Alias,
                    ServerName = _startClientRequest.ServernameOrIp,
                    ServerPort = _startClientRequest.Port
                };

                OnInitPortal(initPortalMessage);

                Log.Debug("Clientinitialisierung erfolgreich durchgeführt");
            }
            else
            {
                Log.Info("Noch nicht alle Daten für Clientinitialisierung vom Server erhalten");
            }
        }

        /// <summary>
        /// Initialer Auffrag den Client zu Initialisieren und eine Verbindung
        /// mit dem Server aufzubauen
        /// </summary>
        /// <param name="request"></param>
        public void ProcessStartClientRequest(StartClientRequest request)
        {
            if (_startClientRequest != null)
            {
                throw new InvalidOperationException("Client wurde schon gestartet");
            }

            Log.Debug("Clientinitialisierung wird gestartet");

            // Die Daten speichern, damit bei der Auslösung der 
            // Initialisierungnachricht die Daten vorhanden sind
            _startClientRequest = request;

            var conToSRequest = new ConnectToServerRequest();
            conToSRequest.Alias = request.Alias;
            conToSRequest.Color = request.Color;
            conToSRequest.Port = request.Port;
            conToSRequest.ServernameOrIp = request.ServernameOrIp;

            OnRequestConnectToServer(conToSRequest);

            request.Result = conToSRequest.Result;
        }

        /// <summary>
        /// Verarbeitet die Information über den Alias des Serverstarters
        /// </summary>
        /// <param name="message"></param>
        public void ProcessConnectedMessage(ConnectedMessage message)
        {
            if (_startClientRequest == null)
            {
                throw new InvalidOperationException("Connectednachricht kann erst nach StartClient-Beauftragung verarbeitet werden");
            }

            if (_connectedMessage != null)
            {
                throw new InvalidOperationException("Initiale Concected-Meldung kann wurde schon verarbeitet");
            }

            _connectedMessage = message;

            TryInit();
        }
    }
}
