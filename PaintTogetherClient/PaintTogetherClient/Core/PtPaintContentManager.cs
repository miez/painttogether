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
using PaintTogetherClient.Messages.Adapter;
using PaintTogetherClient.Messages.Portal;
using PaintTogetherClient.Messages.Core.PaintContentManager;
using System.Drawing;
using log4net;

namespace PaintTogetherClient.Core
{
    /// <summary>
    /// Diese EBC verwaltet den Malbereich im PaintTogetherClient
    /// </summary>
    internal class PtPaintContentManager : IPtPaintContentManager
    {
        /// <summary>
        /// Information das jemand einen Punkt bemalt hat
        /// </summary>
        public event Action<PaintedMessage> OnPainted;

        /// <summary>
        /// Information das Anwender einen Punkt bemalen möchte
        /// </summary>
        public event Action<NewPaintMessage> OnNewPaint;

        /// <summary>
        /// Der aktuelle Malbereich
        /// </summary>
        private Bitmap _paintContent;

        /// <summary>
        /// log4net-Logger
        /// </summary>
        private static ILog Log
        {
            get
            {
                return LogManager.GetLogger("PtPaintContentManager");
            }
        }

        /// <summary>
        /// Bearbeitet die Anfrage des Anwenders, einen Punkt zu bemalen
        /// </summary>
        /// <param name="message"></param>
        public void ProcessPaintSelfMessage(PaintSelfMessage message)
        {

            Log.DebugFormat("Nutzer möchte Punkt '{0}:{1}' übermalen", message.Point.X, message.Point.Y);

            lock (_paintContent)
            {
                if (_paintContent.GetPixel(message.Point.X, message.Point.Y) == message.Color)
                {
                    Log.DebugFormat("Punkt '{0}:{1}' hat schon die richtige Farbe", message.Point.X, message.Point.Y);

                    // Punkt hat schon die gewünschte Farbe
                    return;
                }
            }

            // Bemalung für alle anderen Beteiligten über den Server bauftragen
            OnNewPaint(new NewPaintMessage
                   {
                       Color = message.Color,
                       Point = message.Point
                   });

            lock (_paintContent)
            {
                // Trotzdem schon die Bemalung am Client "vornehmen", auch wenn Serverbestätigung 
                // noch kommen wird
                _paintContent.SetPixel(message.Point.X, message.Point.Y, message.Color);
            }

            Log.InfoFormat("Malnachfrage für Puntk '{0}:{1}' an Server gestellt", message.Point.X, message.Point.Y);
        }

        /// <summary>
        /// Setzt den aktuellen Malinhalt als Resulteigenschaft an die Nachricht
        /// </summary>
        /// <param name="request"></param>
        public void ProcessGetPaintContentRequest(GetPaintContentRequest request)
        {
            Log.Debug("aktueller Malinhalt wird abgefragt");
            request.Result = new Bitmap(_paintContent);
        }

        /// <summary>
        /// Verarbeitet die Information, das ein Beteiligter einen Punkt bemalt hat
        /// </summary>
        /// <param name="message"></param>
        public void ProcessAliasPaintedMessage(AliasPaintedMessage message)
        {
            Log.DebugFormat("Server sendet Bemalung eines Punktes ('{0}:{1}')", message.Point.X, message.Point.Y);

            // Bemalung für alle anderen Beteiligten über den Server bauftragen
            OnPainted(new PaintedMessage
            {
                Color = message.Color,
                Point = message.Point
            });

            lock (_paintContent)
            {
                _paintContent.SetPixel(message.Point.X, message.Point.Y, message.Color);
            }

            Log.InfoFormat("Bemalung durch Beteiligten von Punkt '{0}:{1}' wurde übernommen", message.Point.X, message.Point.Y);

        }

        /// <summary>
        /// Initialisiert die EBC mit den angegeben Daten
        /// </summary>
        /// <param name="message"></param>
        public void ProcessInitPaintMananger(InitPaintManagerMessage message)
        {
            Log.Debug("Manager wird initialisiert");

            // Malbereich kopieren
            _paintContent = new Bitmap(message.PaintContent);
        }
    }
}
