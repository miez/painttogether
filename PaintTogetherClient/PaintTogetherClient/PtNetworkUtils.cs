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
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using log4net;

namespace PaintTogetherClient
{
    /// <summary>
    /// Stellt Funktionen für die Netzwerkkommunikation bereit
    /// </summary>
    public static class PtNetworkUtils
    {
        /// <summary>
        /// log4et-Logger
        /// </summary>
        private static ILog Log
        {
            get
            {
                return LogManager.GetLogger("PtNetworkUtils");
            }
        }

        /// <summary>
        /// Ermittelt aus der IP oder dem Servernamen die
        /// IP und prüft ob diese erreichbar ist
        /// </summary>
        /// <param name="servernameOrIp"></param>
        /// <returns>null, wenn Server oder IP nicht erreichbar</returns>
        public static string DetermineAndCheckIp(string servernameOrIp)
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
