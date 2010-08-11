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
using PaintTogetherClient.Messages.Portal;
using PaintTogetherClient.Messages.Core.PaintContentManager;

namespace PaintTogetherClient.Contracts.Core
{
    /// <summary>
    /// Diese EBC verwaltet den Malbereich im PaintTogetherClient
    /// </summary>
    internal interface IPtPaintContentManager
    {
        /// <summary>
        /// Information das jemand einen Punkt bemalt hat
        /// </summary>
        event Action<PaintedMessage> OnPainted;

        /// <summary>
        /// Information das Anwender einen Punkt bemalen möchte
        /// </summary>
        event Action<NewPaintMessage> OnNewPaint;

        /// <summary>
        /// Bearbeitet die Anfrage des Anwenders, einen Punkt zu bemalen
        /// </summary>
        /// <param name="message"></param>
        void ProcessPaintSelfMessage(PaintSelfMessage message);

        /// <summary>
        /// Setzt den aktuellen Malinhalt als Resulteigenschaft an die Nachricht
        /// </summary>
        /// <param name="request"></param>
        void ProcessGetPaintContentRequest(GetPaintContentRequest request);

        /// <summary>
        /// Verarbeitet die Information, das ein Beteiligter einen Punkt bemalt hat
        /// </summary>
        /// <param name="message"></param>
        void ProcessAliasPaintedMessage(AliasPaintedMessage message);

        /// <summary>
        /// Initialisiert die EBC mit den angegeben Daten
        /// </summary>
        /// <param name="message"></param>
        void ProcessInitPaintMananger(InitPaintManagerMessage message);
    }
}
