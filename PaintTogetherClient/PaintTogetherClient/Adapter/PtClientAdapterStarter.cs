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
using System.Net;
using System.Text.RegularExpressions;
using PaintTogetherClient.Contracts.Adapter;
using PaintTogetherClient.Messages.Adapter;
using PaintTogetherClient.Messages.Adapter.ServerConnectionManager;
using log4net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace PaintTogetherClient.Adapter
{
    /// <summary>
    /// EBC für den initialen Verbindungsaufbau mit dem
    /// angegeben PaintTogetherServer
    /// </summary>
    internal class PtClientAdapterStarter : IPtClientAdapterStarter
    {
        /// <summary>
        /// Informiert über den erfolgreichen Aufbau einer Socket-Verbindung
        /// zu einem PaintTogetherServer
        /// </summary>
        public event Action<ConEstablishedMessage> OnConEstablished;

        /// <summary>
        /// log4et-Logger
        /// </summary>
        private static ILog Log
        {
            get
            {
                return LogManager.GetLogger("PtClientAdapterStarter");
            }
        }

        /// <summary>
        /// Startet den Adapter und baut eine Verbindung zum angegeben Server auf
        /// </summary>
        /// <param name="request"></param>
        public void ProcessConnectToServerRequest(ConnectToServerRequest request)
        {
            Log.DebugFormat("Verbindung zu dem Server '{0}:{1}' wird aufgebaut", request.ServernameOrIp, request.Port);

            // Zuerst die IP aus dem Servernamen ermitteln (falls es nicht schon eine IP ist)
            var ip = DetermineAndCheckIp(request.ServernameOrIp);
            if (string.IsNullOrEmpty(ip)) // ist null, wenn Server nicht aufgelöst werden konnte
            {
                request.Result = string.Format("Fehler beim Verbindungsaufbau zum Server '{0}':'{1}'. Server nicht erreichbar.", request.ServernameOrIp, request.Port);
                return;
            }

            var socket = ConnectToServer(ip, request.Port);
            if (socket == null || !socket.Connected)
            {
                request.Result = string.Format("Fehler beim Verbindungsaufbau zum Server '{0}':'{1}'. Port nicht offen oder Verbindungsfehler.", request.ServernameOrIp, request.Port);
                return;
            }

            OnConEstablished(new ConEstablishedMessage { Socket = socket });
            request.Result = "Verbindungsaufbau erfolgreich";
            Log.InfoFormat("Verbindungsaufbau zu dem Server '{0}:{1}' war erfolgreich", request.ServernameOrIp, request.Port);
        }

        /// <summary>
        /// Baut die Verbindung zum angegeben Server mit dem angegeben Port auf
        /// liefert null falls Verbindung nicht aufgebaut werden konnte
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private static Socket ConnectToServer(string ip, int port)
        {
            try
            {
                var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(Dns.GetHostAddresses(ip)[0], port);
                return clientSocket;
            }
            catch (Exception e)
            {
                Log.Error(string.Format("Fehler beim Verbindungsaufbau zu '{0}:{1}'", ip, port), e);
                return null;
            }
        }

        /// <summary>
        /// Ermittelt aus der IP oder dem Servernamen die
        /// IP und prüft ob diese erreichbar ist
        /// </summary>
        /// <param name="servernameOrIp"></param>
        /// <returns></returns>
        internal static string DetermineAndCheckIp(string servernameOrIp)
        {
            try
            {
                var hostEintrag = Dns.GetHostEntry(servernameOrIp);
                string ip = null;
                foreach (var curIp in hostEintrag.AddressList)
                {
                    if (CheckIP(curIp.ToString()))
                    {
                        ip = curIp.ToString();
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(ip) && IpIsAvaible(ip))
                {
                    Log.DebugFormat("Ip: '{0}' für Server '{1}' bestimmt", ip, servernameOrIp);
                    return ip;
                }
                Log.ErrorFormat("Server '{0}' nicht erreichbar", servernameOrIp);
                return null;
            }
            catch (Exception e)
            {
                Log.Error(string.Format("Fehler bei der IP-Prüfung für '{0}'", servernameOrIp), e);
                return null;
            }
        }

        /// <summary>
        /// Prüft eine IP auf erreichbarkeit
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static bool IpIsAvaible(string ip)
        {
            try
            {
                var ping = new Ping();
                if (ping.Send(IPAddress.Parse(ip)).Status == IPStatus.Success)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error(string.Format("Fehler beim Prüfen der IP '{0}' auf Verfügbarkeit mit ping", ip), e);
            }
            return false;
        }

        /// <summary>
        /// Methode prüft einen String, ob es sich um eine IP handelt (vom Aufbau)<para/>
        /// Quellcode kopiert von http://falkost.de/archives/20
        /// </summary>
        /// <param name="ipstring"></param>
        /// <returns></returns>
        private static bool CheckIP(string ipstring)
        {
            if (Regex.IsMatch(ipstring, @"^([01]?\d\d?|2[0-4]\d|25[0-5])\.([01]?\d\d?|2[0-4]\d|25[0-5])\.([01]?\d\d?|2[0-4]\d|25[0-5])\.([01]?\d\d?|2[0-4]\d|25[0-5])$"))
            {
                return true;
            }
            return false;
        }
    }
}
