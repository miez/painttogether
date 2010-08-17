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
using PaintTogetherStartSelector.Messages;
using PaintTogetherStartSelector.Contracts;
using System.Net.Sockets;
using System.Net;
using PaintTogetherClient;
using System.Threading;

namespace PaintTogetherStartSelector
{
    /// <summary>
    /// Funktionale Kern-EBC des PT-StartSeletors
    /// </summary>
    public class PtStartSelector : IPtStartSelector
    {
        /// <summary>
        /// Aufforderung einen Client zu starten
        /// </summary>
        public event Action<StartClientMessage> OnStartClient;

        /// <summary>
        /// Aufforderung einen Server zu starten
        /// </summary>
        public event Action<StartServerMessage> OnStartServer;

        /// <summary>
        /// Überprüft den angegeben Port auf Verwendung
        /// </summary>
        /// <param name="request"></param>
        public void ProcessTestLocalPortRequest(TestLocalPortRequest request)
        {
            var listener = new TcpListener(IPAddress.Any, request.Port);
            try
            {
                // Wenn der Listener startbar ist, dann ist der Port nicht belegt
                // Bei belegtem Port gibts hier eine Exception
                listener.Start();
                listener.Stop();
                request.Result = true;
            }
            catch (Exception)
            {
                request.Result = false;
            }
        }

        /// <summary>
        /// Testet ob unter den angegebenen Daten eine Verbindung aufgebaut werden kann
        /// </summary>
        /// <param name="request"></param>
        public void ProcessTestServerRequest(TestServerRequest request)
        {
            var ip = PtNetworkUtils.DetermineAndCheckIp(request.ServernameOrIp);
            if (string.IsNullOrEmpty(ip))
            {
                request.Result = false;
                return;
            }

            // Socketverbindung erstellen um Port zu prüfen
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                socket.Connect(ip, request.Port);
                request.Result = socket.Connected;
                socket.Disconnect(false);
                socket.Close();
            }
            catch (Exception)
            {
                // Port ist nicht verwendet
                request.Result = false;
            }
        }

        /// <summary>
        /// Startet einen lokalen Server und einen Client der sich mit
        /// dem Server verbindet
        /// </summary>
        /// <param name="message"></param>
        public void ProcessCreateNewPictureMessage(CreateNewPictureMessage message)
        {
            // Zuerst einen Server starten
            var startServerMessage = new StartServerMessage();
            startServerMessage.Alias = message.Alias;
            startServerMessage.Port = message.Port;
            startServerMessage.Size = message.Size;

            OnStartServer(startServerMessage);

            // Dann kurz warten bis der Server gestartet wurde
            Thread.Sleep(1000);

            // Und einen Client, der sich mit dem Server verbinden soll starten
            var startClientMessage = new StartClientMessage();
            startClientMessage.Alias = message.Alias;
            startClientMessage.Color = message.Color;
            startClientMessage.Port = message.Port;
            startClientMessage.ServernameOrIp = "localhost";

            OnStartClient(startClientMessage);
        }

        /// <summary>
        /// Startet einen Client der sich mit dem angegeben
        /// Server verbindet
        /// </summary>
        /// <param name="message"></param>
        public void ProcessConnectToPictureMessage(ConnectToPictureMessage message)
        {
            var startClientMessage = new StartClientMessage();
            startClientMessage.Alias = message.Alias;
            startClientMessage.Color = message.Color;
            startClientMessage.Port = message.Port;
            startClientMessage.ServernameOrIp = message.ServernameOrIp;

            OnStartClient(startClientMessage);
        }
    }
}
