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
using System.Collections.Generic;
using System.Drawing;
using PaintTogetherServer.Contracts.Core;
using PaintTogetherServer.Messages.Adapter;

namespace PaintTogetherServer.Core
{
    /// <summary>
    /// Verwaltet die aktuell an der Malerei beteiligten Personen
    /// </summary>
    internal class PtPlayerListManager : IPtPlayerListManager
    {
        /// <summary>
        /// Beauftragt die Benachrichtung aller Beteiligten über einen neuen
        /// Beteiligten
        /// </summary>
        public event Action<NotifyNewClientMessage> OnNotifyNewClient;

        /// <summary>
        /// Beauftrag die noch verbundenen Clients über einen
        /// getrennten Beteiligten zu informieren
        /// </summary>
        public event Action<NotifyClientDisconnectedMessage> OnNotifyClientDisconnected;

        /// <summary>
        /// Alle aktuell beteiligten Personen
        /// </summary>
        private readonly List<KeyValuePair<string, Color>> _players = new List<KeyValuePair<string, Color>>();

        /// <summary>
        /// Verarbeitet einen neu verbundenen Beteiligten
        /// </summary>
        /// <param name="message"></param>
        public void ProcessNewClientMessage(NewClientConnectedMessage message)
        {
            // Jetzt jeden Client erlauben, später könnte man über ein Result an der
            // Message noch ab X Beteiligten jeden weiteren Beteiligten verbieten.
            // --
            // Oder man sagt das Name und Farbe eindeutig sein müssen, dann könnte man
            // auch hier über eine neue Resulteigenschaft die Beteiligung verhindern
            // --
            // Jetzt aber jeden neuen Client zulassen und die anderen über den neuen informieren
            _players.Add(new KeyValuePair<string, Color>(message.Alias, message.Color));
            OnNotifyNewClient(new NotifyNewClientMessage { Alias = message.Alias, Color = message.Color });
        }

        /// <summary>
        /// Verarbeitet einen nicht mehr beteiligten Nutzer
        /// </summary>
        /// <param name="message"></param>
        public void ProcessClientDisconnectedMessage(ClientDisconnectedMessage message)
        {
            if (_players.Contains(new KeyValuePair<string, Color>(message.Alias, message.Color)))
            {
                // Nur wenn ein Client vorhanden war, auch eine DisconnectedNotification senden
                _players.Remove(new KeyValuePair<string, Color>(message.Alias, message.Color));
                OnNotifyClientDisconnected(new NotifyClientDisconnectedMessage { Alias = message.Alias, Color = message.Color });
            }
        }

        /// <summary>
        /// Setzt im Result der Anfrage die Informationen über die aktuellen
        /// Beteiligten 
        /// </summary>
        /// <param name="request"></param>
        public void ProcessGetCurrentPainterRequest(GetCurrentPainterRequest request)
        {
            // Inhalt der Spielerliste kopieren, damit diese nicht ausversehen von außen manipuliert wird
            var clonedPlayers = new List<KeyValuePair<string, Color>>();
            foreach(var curPlayer in _players)
            {
                clonedPlayers.Add(new KeyValuePair<string, Color>(curPlayer.Key, curPlayer.Value));
            }

            request.Result = clonedPlayers.ToArray();
        }
    }
}
