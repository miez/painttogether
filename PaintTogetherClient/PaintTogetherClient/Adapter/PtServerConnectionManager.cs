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
using PaintTogetherClient.Contracts.Adapter;
using PaintTogetherClient.Messages.Adapter;
using PaintTogetherClient.Messages.Adapter.ServerConnectionManager;
using PaintTogetherCommunicater.Messages;
using System.Net.Sockets;
using log4net;
using PaintTogetherCommunicater.Messages.ClientServerCommunication.Client;
using PaintTogetherCommunicater.Messages.ClientServerCommunication.Server;

namespace PaintTogetherClient.Adapter
{
    /// <summary>
    /// EBC für die Verwaltung der Server-Socket-Verbindung und 
    /// den Nachrichtenaustausch zwischen diesem Server und dem Client
    /// </summary>
    internal class PtServerConnectionManager : IPtServerConnectionManager
    {
        #region Outputpins - Kommentare siehe Interface
        public event Action<CurrentPaintContentMessage> OnCurrentPaintContent;

        public event Action<AliasPaintedMessage> OnAliasPainted;

        public event Action<ConnectedMessage> OnConnected;

        public event Action<ServerConnectionLostMessage> OnServerConnectionLost;

        public event Action<NewAliasMessage> OnNewAlias;

        public event Action<AliasDisconnectedMessage> OnAliasDisconnected;

        public event Action<SendMessageMessage> OnSendMessage;

        public event Action<StartReceivingMessage> OnStartReceiving;

        public event Action<StopReceivingMessage> OnStopReceiving;
        #endregion

        /// <summary>
        /// Die Server-Socket-Verbindung
        /// </summary>
        private Socket _serverConnection;

        /// <summary>
        /// Gibt an, ob eine Verbindung zu dem Server besteht
        /// </summary>
        private bool Connected
        {
            get
            {
                return _serverConnection != null && _serverConnection.Connected;
            }
        }

        /// <summary>
        /// log4net-Logger
        /// </summary>
        private static ILog Log
        {
            get
            {
                return LogManager.GetLogger("PtServerConnectionManager");
            }
        }

        #region Inputpins - Kommentare siehe Interface
        public void ProcessConEstablishedMessage(ConEstablishedMessage message)
        {
            _serverConnection = message.Socket;

            // Nachrichtenempfang starten
            OnStartReceiving(new StartReceivingMessage { ToWatchSoketConnection = _serverConnection });

            // Informationen des angemeldeten Nutzers an den Server schicken
            var toSendContent = new ConnectScm
                {
                    Alias = message.Alias,
                    Color = message.Color
                };

            OnSendMessage(new SendMessageMessage { Message = toSendContent, SoketConnection = _serverConnection });

            Log.Warn("Clientinformationen an den Server verschickt");
        }

        public void ProcessNewPaintMessage(NewPaintMessage message)
        {
            if (!Connected)
            {
                Log.Warn("Malaufforderung kann nicht an Server weitergeleitet werden, da keine Verbindung besteht");
                return;
            }

            // Malaufforderung an den Server weiterleiten
            var toSendContent = new PaintScm()
            {
                Color = message.Color,
                Point = message.Point
            };

            OnSendMessage(new SendMessageMessage { Message = toSendContent, SoketConnection = _serverConnection });

            Log.Info("Malaufforderung erfolgreich an den Server weitergeleitet");
        }

        public void ProcessCloseConnectionMessage(CloseConnectionMessage message)
        {
            if (Connected)
            {
                _serverConnection.Disconnect(false);
                _serverConnection.Close();

                Log.Debug("Verbindung zum Server wurde getrennt");
            }
        }

        public void ProcessConLostMessage(ConLostMessage message)
        {
            OnServerConnectionLost(new ServerConnectionLostMessage());

            // Überwachung muss nicht mehr gestoppt werden/ Bei conLost wird die
            // Überwachung automatisch im Communicater beendet
        }

        public void ProcessNewMessageMessage(NewMessageReceivedMessage message)
        {
            Log.Debug("Neue Nachricht vom Server empfangen. Inhalt wird jetzt verarbeitet.");

            if (message.Message is AllConnectionsScm)
            {
                ProcessAllConnectionsScm(message.Message as AllConnectionsScm);
                return;
            }
            if (message.Message is ConnectedScm)
            {
                ProcessConnectedScm(message.Message as ConnectedScm);
                return;
            }
            if (message.Message is ConnectionLostScm)
            {
                ProcessConnectionLostScm(message.Message as ConnectionLostScm);
                return;
            }
            if (message.Message is NewConnectionScm)
            {
                ProcessNewConnectionScm(message.Message as NewConnectionScm);
                return;
            }
            if (message.Message is PaintContentScm)
            {
                ProcessPaintContentScm(message.Message as PaintContentScm);
                return;
            }
            if (message.Message is PaintedScm)
            {
                ProcessPaintedScm(message.Message as PaintedScm);
                return;
            }

            Log.Warn("Es wurde eine Nachricht mit unbekanntem Inhalt empfangen");
        }
        #endregion

        /// <summary>
        /// Verarbeitet die Benachrichtigung über einen neu bemalten Punkt
        /// </summary>
        /// <param name="paintedScm"></param>
        private void ProcessPaintedScm(PaintedScm paintedScm)
        {
            Log.Info("Benachrichtigung über übermalten Punkt vom Server erhalten");
            OnAliasPainted(new AliasPaintedMessage { Color = paintedScm.Color, Point = paintedScm.Point });
        }

        /// <summary>
        /// Nachricht mit dem aktuellen Malinhalt
        /// </summary>
        /// <param name="paintContentScm"></param>
        private void ProcessPaintContentScm(PaintContentScm paintContentScm)
        {
            Log.Info("Aktueller Malinhalt vom Server erhalten");
            OnCurrentPaintContent(new CurrentPaintContentMessage { PaintContent = paintContentScm.PaintContent });
        }

        /// <summary>
        /// Information über neuen Beteiligten wird verarbeitet
        /// </summary>
        /// <param name="newConnectionScm"></param>
        private void ProcessNewConnectionScm(NewConnectionScm newConnectionScm)
        {
            Log.Info("Information über neuen Beteiligten vom Server erhalten");
            OnNewAlias(new NewAliasMessage { Alias = newConnectionScm.Alias, Color = newConnectionScm.Color });
        }

        /// <summary>
        /// Information über einen von der Malerei abgemeldeten Beteiligten wird verarbeitet
        /// </summary>
        /// <param name="connectionLostScm"></param>
        private void ProcessConnectionLostScm(ConnectionLostScm connectionLostScm)
        {
            Log.Info("Information über einen von der Malerei abgemeldeten Beteiligten vom Server erhalten");
            OnAliasDisconnected(new AliasDisconnectedMessage { Alias = connectionLostScm.Alias, Color = connectionLostScm.Color });
        }

        /// <summary>
        /// Verarbeitet Information mit Verbindungsdaten des Servers
        /// </summary>
        /// <param name="connectedScm"></param>
        private void ProcessConnectedScm(ConnectedScm connectedScm)
        {
            Log.Info("Verbindungsdaten des Servers vom Server erhalten");
            OnConnected(new ConnectedMessage { Alias = connectedScm.Alias });
        }

        /// <summary>
        /// Verarbeitet Information mit allen aktuell Beteiligten Personen
        /// </summary>
        /// <param name="allConnectionsScm"></param>
        private void ProcessAllConnectionsScm(AllConnectionsScm allConnectionsScm)
        {
            Log.Info("Aktuelle Beteiligtenliste vom Server erhalten");
            // Jeden Beteiligten einzeln signalisieren
            foreach (var painter in allConnectionsScm.Painter)
            {
                OnNewAlias(new NewAliasMessage { Alias = painter.Key, Color = painter.Value });
            }
        }
    }
}
