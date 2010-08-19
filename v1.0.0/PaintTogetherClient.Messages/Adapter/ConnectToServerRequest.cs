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

using System.Drawing;

namespace PaintTogetherClient.Messages.Adapter
{
    /// <summary>
    /// Aufforderung zum Verbindungsaufbau mit
    /// dem angegebenem Server
    /// </summary>
    public class ConnectToServerRequest
    {
        /// <summary>
        /// Servername oder IP
        /// </summary>
        public string ServernameOrIp { get; set; }

        /// <summary>
        /// Serverport
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Alias des Nutzers
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Malfarbe des Nutzers
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Statusmeldung über den Verbindungsaufbau
        /// ! wird dem Nutzer angezeigt !
        /// </summary>
        public string Result { get; set; }
    }
}
