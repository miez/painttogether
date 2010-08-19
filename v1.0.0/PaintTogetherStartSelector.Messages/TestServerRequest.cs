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

namespace PaintTogetherStartSelector.Messages
{
    /// <summary>
    /// Aufforderung für Überprüfung eines Servers + Port
    /// </summary>
    public class TestServerRequest
    {
        /// <summary>
        /// Zu prüfender Port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Servername/IP des zu prüfenden Rechners
        /// </summary>
        public string ServernameOrIp{ get; set; }

        /// <summary>
        /// Zu setztende Antwort - Ist der Port auf dem angegebenen Server verbindbar?
        /// </summary>
        public bool Result{ get; set; }
    }
}
