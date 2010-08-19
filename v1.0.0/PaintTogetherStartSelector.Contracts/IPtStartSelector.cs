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

namespace PaintTogetherStartSelector.Contracts
{
    /// <summary>
    /// Funktionale Kern-EBC des PT-StartSeletors
    /// </summary>
    public interface IPtStartSelector
    {
        /// <summary>
        /// Aufforderung einen Client zu starten
        /// </summary>
        event Action<StartClientMessage> OnStartClient;

        /// <summary>
        /// Aufforderung einen Server zu starten
        /// </summary>
        event Action<StartServerMessage> OnStartServer;

        /// <summary>
        /// Überprüft den angegeben Port auf Verwendung
        /// </summary>
        /// <param name="request"></param>
        void ProcessTestLocalPortRequest(TestLocalPortRequest request);

        /// <summary>
        /// Testet ob unter den angegebenen Daten eine Verbindung aufgebaut werden kann
        /// </summary>
        /// <param name="request"></param>
        void ProcessTestServerRequest(TestServerRequest request);

        /// <summary>
        /// Startet einen lokalen Server und einen Client der sich mit
        /// dem Server verbindet
        /// </summary>
        /// <param name="message"></param>
        void ProcessCreateNewPictureMessage(CreateNewPictureMessage message);

        /// <summary>
        /// Startet einen Client der sich mit dem angegeben
        /// Server verbindet
        /// </summary>
        /// <param name="message"></param>
        void ProcessConnectToPictureMessage(ConnectToPictureMessage message);
    }
}
