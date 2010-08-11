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
using PaintTogetherClient.Messages.Portal;
using PaintTogetherClient.Messages.Core.PaintContentManager;

namespace PaintTogetherClient.Contracts.Core
{
    /// <summary>
    /// EBC die von dem aktuellen Malbereich eine
    /// Bilddatei erzeugt und speichert
    /// </summary>
    internal interface IPtPictureTaker
    {
        /// <summary>
        /// Erfragt den Malbereich
        /// </summary>
        event Action<GetPaintContentRequest> OnRequestPaintContent;

        /// <summary>
        /// Verarbeitet den Auftrag den aktuellen Malstand in einer Datei zu speichern
        /// </summary>
        /// <param name="request"></param>
        void ProcessTakePictureRequest(TakePictureRequest request);
    }
}
