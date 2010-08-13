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
using PaintTogetherClient.Contracts.Core;
using PaintTogetherClient.Messages.Portal;
using PaintTogetherClient.Messages.Core.PaintContentManager;
using System.IO;
using log4net;

namespace PaintTogetherClient.Core
{
    /// <summary>
    /// EBC die von dem aktuellen Malbereich eine
    /// Bilddatei erzeugt und speichert
    /// </summary>
    internal class PtPictureTaker : IPtPictureTaker
    {
        /// <summary>
        /// Erfragt den Malbereich
        /// </summary>
        public event Action<GetPaintContentRequest> OnRequestPaintContent;

        /// <summary>
        /// Log4net-Logger
        /// </summary>
        private static ILog Log
        {
            get
            {
                return LogManager.GetLogger("PtPictureTaker");
            }
        }

        /// <summary>
        /// Verarbeitet den Auftrag den aktuellen Malstand in einer Datei zu speichern
        /// </summary>
        /// <param name="request"></param>
        public void ProcessTakePictureRequest(TakePictureRequest request)
        {
            var getContentRequest = new GetPaintContentRequest();
            OnRequestPaintContent(getContentRequest);

            string userMessage;

            try
            {
                getContentRequest.Result.Save(request.Filename);

                userMessage = string.Format("Aktueller Malbereich erfolgreich in der Datei '{0}' gespeichert.", request.Filename);
                Log.Debug(userMessage);
            }
            catch (Exception e)
            {
                userMessage = string.Format("Fehler beim Speichern des aktuellen Malbereichs in der Datei='{0}'\nFehler: {1}", request.Filename, e.Message);
                Log.Error(userMessage, e);
            }
            request.Result = userMessage;
        }
    }
}
