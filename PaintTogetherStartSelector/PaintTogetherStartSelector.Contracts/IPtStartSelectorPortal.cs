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
    /// GUI des StartSelectors für Nutzereingabenvalidierung zum
    /// Start eines PT-Servers mit PT-Client oder nur eines PT-Client
    /// </summary>
    public interface IPtStartSelectorPortal
    {
        /// <summary>
        /// Aufforderung eine neue Malerei zu starten
        /// sowie einen Client der sich mit der neuen 
        /// Malerei verbindet
        /// </summary>
        event Action<CreateNewPictureMessage> OnCreateNewPicture;

        /// <summary>
        /// Aufforderung einen Client für den angegebenen Server zu starten
        /// </summary>
        event Action<ConnectToPictureMessage> OnConnectToPicture;

        /// <summary>
        /// Aufforderung den angegebenen Port auf Verfügbarkeit zu prüfen
        /// </summary>
        event Action<TestLocalPortRequest> OnRequestTestLocalPort;

        /// <summary>
        /// Aufforderung den angegebenen Server mit Port auf
        /// Verbindungsfähigkeit zu prüfen
        /// </summary>
        event Action<TestServerRequest> OnRequestTestServer;
    }
}
