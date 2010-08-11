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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PaintTogetherServer.Contracts.Adapter;
using PaintTogetherServer.Messages.Adapter;
using PaintTogetherServer.Messages.Adapter.ConnectionManager;

namespace PaintTogetherServer.Adapter
{
    /// <summary>
    /// Startet die Überwachung eines Ports und meldet alle eingehenden Verbindungen
    /// </summary>
    internal class PtPortListener : IPtPortListener
    {
        /// <summary>
        /// Informiert über eine neu eingegangene Verbindung
        /// </summary>
        public event Action<NewConnectionMessage> OnNewConnection;

        /// <summary>
        /// Verarbeitet die initiale Aufforderung den Apdater zu starten und
        /// einen Port zu überwachen
        /// </summary>
        /// <param name="message"></param>
        public void ProcessStartPortListingMessage(StartPortListingMessage message)
        {
            // Überwachung wird in anderem Thread gestartet, da sonst der
            // Hauptthread blockieren würde
            var thread = new Thread(StartListing);
            thread.Start(message.Port);

            // TODO listener.Stop ! Extra Event! 
            // (auch wenn durch Shutdown der Anwendung ein Stop automatisch erfolgt)
        }

        private void StartListing(object port)
        {
            // Auf clientverbindung lauschen
            var listener = new TcpListener(IPAddress.Any, (int)port);
            listener.Start();

            while (true) // TODO Stopbedingung
            {
                // Den durch Connect gestarteten Verbindungsaufbau akzeptieren
                while (!listener.Pending())
                {
                    Thread.Sleep(100);
                }

                // In einem separten Thread die neue Verbindung übergeben,
                // damit der Empfang weiterer Verbindungen nicht gestört wird
                var serverSocket = listener.AcceptSocket();
                var thread = new Thread(SignalNewConnection);
                thread.Start(serverSocket);
            }
        }

        private void SignalNewConnection(object socket)
        {
            OnNewConnection(new NewConnectionMessage { Socket = socket as Socket });
        }
    }
}
