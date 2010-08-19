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
using PaintTogetherServer.Contracts.Core;
using PaintTogetherServer.Messages.Adapter;
using PaintTogetherServer.Messages.Core.PaintFieldManager;
using System.Drawing;
using log4net;

namespace PaintTogetherServer.Core
{
    /// <summary>
    /// Verwaltet den Malbereich in Form einer BitMap
    /// </summary>
    internal class PtPaintFieldManager : IPtPaintFieldManager
    {
        /// <summary>
        /// Beauftragt die Benachrichtigung aller Clients über
        /// einen neu bemalten Punkt
        /// </summary>
        public event Action<NotifyPaintToClientsMessage> OnNotifyPaint;

        /// <summary>
        /// Der aktuelle Malinhalt
        /// </summary>
        private Bitmap _paintContent;

        /// <summary>
        /// Die Grafik, die den Malbereich darstellt
        /// </summary>
        private Graphics _paintGraph;

        /// <summary>
        /// log4net-Logger für Logging
        /// </summary>
        private static ILog Log
        {
            get
            {
                return LogManager.GetLogger("PtPaintFieldManager");
            }
        }

        /// <summary>
        /// Verarbeitet die Initialisierungsaufforderung
        /// </summary>
        /// <param name="message"></param>
        public void ProcessInitMessage(InitMessage message)
        {
            Log.DebugFormat("Malbereich wird mit einer Größe von 'W={0}:H={1}' initialisiert", message.Width, message.Height);
            _paintContent = new Bitmap(message.Width, message.Height);
            _paintGraph = Graphics.FromImage(_paintContent);

            // Content weiß bemalen)
            using (var brush = new SolidBrush(Color.White))
            {
                _paintGraph.FillRectangle(brush, 0f, 0f, message.Width, message.Height);
            }
        }

        /// <summary>
        /// Setzt den aktuellen Malbereich im aktuellen Zustand in
        /// das Result der Anfrage
        /// </summary>
        /// <param name="request"></param>
        public void ProcessGetCurrentPaintContentRequest(GetCurrentPaintContentRequest request)
        {
            Log.Debug("Aktueller Malbereich wird abgefragt");
            if (_paintContent == null)
            {
                Log.Error("Malbereich noch nicht initialisiert - Malbereich ist leer");
                request.Result = new Bitmap(0, 0);
                return;
            }

            // Nicht das Orginalbild weitergeben, da es verändert werden
            // könnte
            request.Result = _paintContent.Clone() as Bitmap;
        }

        /// <summary>
        /// Verarbeitet die Anfrage zum Malen eines Beteiligten
        /// </summary>
        /// <param name="message"></param>
        public void ProcessClientPainted(ClientPaintedMessage message)
        {
            Log.DebugFormat("Neue Malanfrage für Strich 'Xa={0}:Ya={1}' und 'Xb={2}:Yb={3}'", message.StartPoint.X, message.StartPoint.Y, message.EndPoint.X, message.EndPoint.Y);
            if (_paintContent == null)
            {
                Log.Error("Malbereich noch nicht initialisiert - Malvorgang abgebrochen");
                return;
            }

            using (var pen = new Pen(message.Color, 1f))
            {
                _paintGraph.DrawLine(pen, message.StartPoint, message.EndPoint);
            }

            OnNotifyPaint(new NotifyPaintToClientsMessage { Color = message.Color, StartPoint = message.StartPoint, EndPoint = message.EndPoint });
        }
    }
}
