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
using System.Drawing;
using System.Net.Sockets;
using System.Threading;
using log4net;
using PaintTogetherCommunicater.Messages;
using PaintTogetherCommunicater.Messages.ClientServerCommunication.Server;
using PaintTogetherServer.Contracts.Adapter;
using PaintTogetherServer.Messages.Adapter;
using PaintTogetherServer.Messages.Adapter.ConnectionManager;
using PaintTogetherCommunicater.Messages.ClientServerCommunication;
using PaintTogetherCommunicater.Messages.ClientServerCommunication.Client;

namespace PaintTogetherServer.Adapter
{
    /// <summary>
    /// Verwaltet alle Socketverbindungen
    /// </summary>
    internal class PtConnectionMananger : IPtConnectionMananger
    {
        #region Outputpins - Kommentare finden sich am Interface - hier aus Übersichtlichkeit weggelassen
        public event Action<NewClientConnectedMessage> OnNewClient;

        public event Action<ClientDisconnectedMessage> OnClientDisconnected;

        public event Action<GetCurrentPaintContentRequest> OnRequestCurPaintContent;

        public event Action<GetCurrentPainterRequest> OnRequestCurPainter;

        public event Action<ClientPaintedMessage> OnClientPainted;

        public event Action<SendMessageMessage> OnSendMessage;

        public event Action<StartReceivingMessage> OnStartReceiving;

        public event Action<StopReceivingMessage> OnStopReceiving;
        #endregion

        /// <summary>
        /// Ein Objekt was für das locken vor dem "Ändern" (nicht lesen)
        /// auf die Connectionlisten vorgesehen ist
        /// </summary>
        private readonly object _lockObject = new object();

        /// <summary>
        /// Alle aktuell verbundenen Clientverbindungen
        /// </summary>
        private readonly List<Socket> _connectedSockets = new List<Socket>();

        /// <summary>
        /// Zuordnung aller erfolgreich verbundener Clients mit Farbe und Alias
        /// </summary>
        /// <returns></returns>
        private readonly Dictionary<Socket, KeyValuePair<string, Color>> _confirmedConnections = new Dictionary<Socket, KeyValuePair<string, Color>>();

        /// <summary>
        /// Der Alias der Person, die den Server gestartet hat
        /// </summary>
        private string _serverAlias = "N/A";

        /// <summary>
        /// Logger für Logging über log4net
        /// </summary>
        private static ILog Log
        {
            get
            {
                return LogManager.GetLogger("PtConnectionManager");
            }
        }

        #region Input-Pin-Methoden - Kommentare stehen am Interface, aus Übersichtlichkeit hier weggelassen
        public void ProcessDisconnectAllClientsMessage(DisconnectAllClientsMessage message)
        {
            Log.Debug("Alle offenen Clientverbindungen werden geschlossen");

            var toStopSockets = new List<Socket>();
            toStopSockets.AddRange(_connectedSockets); // muss kopiert werden da bei DisconnectSocket auf _connectedSockets zugegriffen wird

            // Jetzt erst die Sockets stoppen, damit durch die Eventbearbeitung nicht
            // die Liste _connectedSockets während des Durchlaufens geändert wird
            foreach (var socket in toStopSockets)
            {
                DisconnectSocket(socket);
            }

            Log.Debug("Alle offenen Clientverbindungen wurden geschlossen");
        }

        /// <summary>
        /// Beendet die Überwachung eines verbundenen Sockets und entfernt ihn aus der aktuellen
        /// Verbundsliste !!! Achtung vor Deadlocks, diese Methode nicht aufrufen wenn
        /// _connectedSockets gelockt wurde
        /// </summary>
        /// <param name="socket"></param>
        private void DisconnectSocket(Socket socket)
        {
            OnStopReceiving(new StopReceivingMessage { SoketConnection = socket });

            Thread.Sleep(100); // sleep damit durch das Stoppen nicht ConLost ausgelöst wird

            lock (_lockObject) // Locken damit kein anderer Thread im Hintergrund ein Element hinzufügt/entfernt
            {
                if (_confirmedConnections.ContainsKey(socket))
                {
                    _confirmedConnections.Remove(socket);
                }

                if (_connectedSockets.Contains(socket))
                {
                    _connectedSockets.Remove(socket);
                }
            }

            socket.Disconnect(false);
            socket.Close();

            Log.Debug(" - Verbindung geschlossen/ Empfangsüberwachung der Verbindung beendet");
        }

        public void ProcessNotifyNewClientMessage(NotifyNewClientMessage message)
        {
            Log.DebugFormat("Information über neuen Beteiligten wird an alle {0} Verbindungen gesendet", _confirmedConnections.Count);

            // An alle erfolgreich registrierten Verbindungen die
            // Nachricht über einen neuen Beteiligten schicken
            var toSendContent = new NewConnectionScm { Alias = message.Alias, Color = message.Color };
            SendMessageContentToConfirmedSockets(toSendContent);
        }

        public void ProcessNotifyClientDisconnectedMessage(NotifyClientDisconnectedMessage message)
        {
            Log.DebugFormat("Information über Trennung zu einem Beteiligten wird an alle {0} Verbindungen gesendet", _confirmedConnections.Count);

            var toSendContent = new ConnectionLostScm { Alias = message.Alias, Color = message.Color };
            SendMessageContentToConfirmedSockets(toSendContent);
        }

        public void ProcessNotifyPaintMessage(NotifyPaintToClientsMessage message)
        {
            Log.DebugFormat("Information über neu bemalten Punkt wird an alle {0} Verbindungen gesendet", _confirmedConnections.Count);

            var toSendContent = new PaintedScm { Point = message.Point, Color = message.Color };
            SendMessageContentToConfirmedSockets(toSendContent);
        }

        public void ProcessNewConnectionMessage(NewConnectionMessage message)
        {
            Log.Debug("Neue Verbindung eingegangen. Verbindung wirde jetzt bearbeitet.");
            try
            {
                lock (_lockObject)
                {
                    _connectedSockets.Add(message.Socket);
                }

                // Connected-Nachricht schicken
                IServerClientMessage toSendMessage = new ConnectedScm { Alias = _serverAlias };
                Log.Debug("Connectednachricht wird an die neue Verbindung geschickt.");
                OnSendMessage(new SendMessageMessage { SoketConnection = message.Socket, Message = toSendMessage });

                // Spielfeld senden
                var pContentRequest = new GetCurrentPaintContentRequest();
                OnRequestCurPaintContent(pContentRequest); // Den Spielfeldinhalt ermitteln
                toSendMessage = new PaintContentScm { PaintContent = pContentRequest.Result };
                Log.Debug("Aktueller Malbereich wird an die neue Verbindung geschickt.");
                OnSendMessage(new SendMessageMessage { SoketConnection = message.Socket, Message = toSendMessage });

                // Beteiligte schicken
                var painterRequest = new GetCurrentPainterRequest();
                OnRequestCurPainter(painterRequest);
                toSendMessage = new AllConnectionsScm { Painter = painterRequest.Result };
                Log.Debug("Liste der aktuell Beteiligten wird an die neue Verbindung geschickt.");
                OnSendMessage(new SendMessageMessage { SoketConnection = message.Socket, Message = toSendMessage });

                // Am Ende die Überwachung auf eingehende Nachrichten starten
                Log.Debug("Überwachung auf eingehende Benachrichtigungen für die neue Verbindung wird gestartet.");
                OnStartReceiving(new StartReceivingMessage { ToWatchSoketConnection = message.Socket });
            }
            catch (Exception e)
            {
                Log.Error("Fehler beim Behandeln einer neuen Verbindung", e);

                lock (_lockObject)
                {
                    _connectedSockets.Remove(message.Socket);
                }
            }
        }

        public void ProcessConLostMessage(ConLostMessage message)
        {
            Log.Debug("Eine Clientverbindung wurde von außen getrennt");

            lock (_lockObject)
            {
                if (_connectedSockets.Contains(message.DisconnectedSoketConnection))
                {
                    _connectedSockets.Remove(message.DisconnectedSoketConnection);
                }
            }

            var data = GetAndRemoveConfirmedConData(message.DisconnectedSoketConnection);

            if (!string.IsNullOrEmpty(data.Key))
            {
                Log.DebugFormat("Server wird über Clientverlust informiert Alias:'{0}'", data.Key);
                OnClientDisconnected(new ClientDisconnectedMessage { Alias = data.Key, Color = data.Value });
            }
            else
            {
                Log.Warn("Die getrennte Clientverbindung wurde noch nicht bestätigt");
            }
        }

        /// <summary>
        /// Liest die Daten über eine bestätigte Verbindung aus der 
        /// Liste aus und löscht diese aus der Liste
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private KeyValuePair<string, Color> GetAndRemoveConfirmedConData(Socket socket)
        {
            lock (_lockObject)
            {
                if (_confirmedConnections.ContainsKey(socket))
                {
                    var data = _confirmedConnections[socket];
                    _confirmedConnections.Remove(socket);
                    return data;
                }
            }
            return new KeyValuePair<string, Color>(null, Color.Empty);
        }

        public void ProcessNewMessageMessage(NewMessageReceivedMessage message)
        {
            Log.Debug("Neue Nachricht eines Clients erhalten");
            try
            {
                var socket = message.SoketConnection;
                var mContent = message.Message;

                if (mContent is ConnectScm)
                {
                    Log.Debug("Es handelt sich um die initiale Verbindungsnachricht des Clients");
                    ProcessClientConnectScm(socket, mContent as ConnectScm);
                    return;
                }
                if (mContent is PaintScm)
                {
                    Log.Debug("Es handelt sich um eine Malanfrage");
                    ProcessClientPaintScm(mContent as PaintScm);
                    return;
                }
                Log.Warn("Bei der empfangenen Nachricht handelt es sich nicht um eine gültigte Clientnachricht");
            }
            catch (Exception e)
            {
                Log.Error("Fehler bei der Verarbeitung einer neu empfangenen Nachricht", e);
            }
        }
        #endregion

        /// <summary>
        /// Verarbeitet eine vom Client eingegangene Malanfrage
        /// </summary>
        /// <param name="paintScm"></param>
        private void ProcessClientPaintScm(PaintScm paintScm)
        {
            var color = paintScm.Color;
            var point = paintScm.Point;

            Log.DebugFormat("Die Malanfrage wird weitergeleitet Punkt:{0}", point);
            OnClientPainted(new ClientPaintedMessage { Color = color, Point = point });
        }

        /// <summary>
        /// Verarbeietet eine vom Client eingegangene Verbindungsbestätigung
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="connectScm"></param>
        private void ProcessClientConnectScm(Socket socket, ConnectScm connectScm)
        {
            var alias = connectScm.Alias;
            var color = connectScm.Color;

            Log.DebugFormat("Der neue Beteiligte '{0}' mit der Farbe '{1}|{2}|{3}'wird verarbeitet", connectScm.Alias, connectScm.Color.R, connectScm.Color.G, connectScm.Color.B);
            lock (_lockObject)
            {
                _confirmedConnections.Add(socket, new KeyValuePair<string, Color>(alias, color));
            }
            OnNewClient(new NewClientConnectedMessage { Alias = alias, Color = color });
        }

        /// <summary>
        /// Sendet an alle erfolgreich verbundenen Sockets eine Nachricht
        /// </summary>
        /// <param name="toSendContent"></param>
        private void SendMessageContentToConfirmedSockets(IServerClientMessage toSendContent)
        {
            var conCopy = new List<Socket>();
            conCopy.AddRange(_confirmedConnections.Keys);

            foreach (var socket in conCopy)
            {
                OnSendMessage(new SendMessageMessage { SoketConnection = socket, Message = toSendContent });
            }
        }

        /// <summary>
        /// Verarbeitet Initialisierungsinformationen 
        /// </summary>
        /// <param name="message"></param>
        public void ProcessInitConnectionManagerMessage(InitConnectionManagerMessage message)
        {
            Log.DebugFormat("Als Alias für den Servernutzer wird '{0}' gesetzt", message.Alias);
            _serverAlias = message.Alias;
        }
    }
}
