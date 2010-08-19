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

namespace PaintTogetherClient.Messages.Portal
{
    /// <summary>
    /// Enhält alle Informationen für die Initialisierung des Malbereichs
    /// </summary>
    public class InitPortalMessage
    {
        /// <summary>
        /// aktueller Malbereich inkl. Größe (Größe des Bildes)
        /// </summary>
        public Bitmap PaintContent { get; set; }

        /// <summary>
        /// Alias des Nutzers der den Server gestartet hat
        /// </summary>
        public string ServerAlias { get; set; }

        /// <summary>
        /// Name/IP des Servers, auf dem der PaintTogehterServer läuft
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Alias des Anwenders
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Malfarbe des Anwenders
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Port des Servers, der vom PaintTogetherServer für die Verbindungen
        /// verwendet wird
        /// </summary>
        public int ServerPort { get; set; }
    }
}
