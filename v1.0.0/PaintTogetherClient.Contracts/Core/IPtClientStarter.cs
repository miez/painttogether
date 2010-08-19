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
using PaintTogetherClient.Messages.Adapter;
using PaintTogetherClient.Messages.Core.ClientStarter;
using PaintTogetherClient.Messages.Portal;
using PaintTogetherClient.Messages.Core.PaintContentManager;

namespace PaintTogetherClient.Contracts.Core
{
    /// <summary>
    /// EBC die den Start und die Initialisierung des Clients beauftragt
    /// </summary>
    internal interface IPtClientStarter
    {
        /// <summary>
        /// Aufforderung das Portal zu initialisieren
        /// </summary>
        event Action<InitPortalMessage> OnInitPortal;

        /// <summary>
        /// Aufforderung eine Verbindung zum Server aufzubauen
        /// </summary>
        event Action<ConnectToServerRequest> OnRequestConnectToServer;

        /// <summary>
        /// Aufforderung den Malbereich zu initialisieren
        /// </summary>
        event Action<InitPaintManagerMessage> OnInitPaintManager;

        /// <summary>
        /// Verarbeitet die Information mit dem initialen Malstand
        /// </summary>
        /// <param name="message"></param>
        void ProcessCurrentPaintContentMessage(CurrentPaintContentMessage message);

        /// <summary>
        /// Initialer Auffrag den Client zu Initialisieren und eine Verbindung
        /// mit dem Server aufzubauen
        /// </summary>
        /// <param name="request"></param>
        void ProcessStartClientRequest(StartClientRequest request);

        /// <summary>
        /// Verarbeitet die Information über den Alias des Serverstarters
        /// </summary>
        /// <param name="message"></param>
        void ProcessConnectedMessage(ConnectedMessage message);
    }
}
