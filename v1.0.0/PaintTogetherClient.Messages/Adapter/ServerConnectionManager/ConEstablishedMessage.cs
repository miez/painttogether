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
using System.Net.Sockets;

namespace PaintTogetherClient.Messages.Adapter.ServerConnectionManager
{
    /// <summary>
    /// Information über erfolgreichen Verbinungsaufbau zum Server
    /// </summary>
    internal class ConEstablishedMessage
    {
        /// <summary>
        /// Die ServerSocket-Verbindung
        /// </summary>
        public Socket Socket{ get; set; }

        /// <summary>
        /// Alias des Clientnutzers
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Malfarbe des Clientnutzers
        /// </summary>
        public Color Color { get; set; }
    }
}
