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

namespace PaintTogetherCommunicater
{
    /// <summary>
    /// Alle verfügbaren Nachrichtentypen. Aufgelistet für die Typangabe bei
    /// der XML-Serializierung.
    /// </summary>
    public enum ServerClientMessageType
    {
        /// <summary>
        /// Unbekannt
        /// </summary>
        Unknown,

        /// <summary>
        /// Verbindungsanfrage vom Client
        /// </summary>
        ClientConnect,

        /// <summary>
        /// Malaufforderung vom Client
        /// </summary>
        ClientPaint,

        /// <summary>
        /// Information über alle aktuellen Beteiligten
        /// </summary>
        ServerAllConnections,

        /// <summary>
        /// Bestätigung des Servers für das Aktzeptieren einer Clientverbindung
        /// </summary>
        ServerClientConnected,

        /// <summary>
        /// Information vom Server über den Verlust eines Beteiligten
        /// </summary>
        ServerConnectionLost,

        /// <summary>
        /// Information vom Server über einen neuen Beteiligten
        /// </summary>
        ServerNewConnection,

        /// <summary>
        /// Der aktuelle, gesamte Malbereich
        /// </summary>
        ServerPaintContent,

        /// <summary>
        /// Information vom Server über einen neu bemalten Punkt
        /// </summary>
        ServerPainted
    }
}
