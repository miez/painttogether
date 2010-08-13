
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

using System.Net;
using System.Net.Sockets;
using NUnit.Framework;
using PaintTogetherClient.Messages.Adapter;
using PaintTogetherClient.Core;
using PaintTogetherClient.Contracts.Core;
using System;
using PaintTogetherClient.Messages.Core.ClientStarter;
using PaintTogetherServer;
using PaintTogetherServer.Contracts;
using PaintTogetherServer.Messages.Adapter;
using PaintTogetherClient.Contracts.Adapter;
using PaintTogetherClient.Adapter;
using PaintTogetherClient.Messages.Adapter.ServerConnectionManager;

namespace PaintTogetherClient.Test.Adapter.PtClientAdapterStarterCS
{
    [TestFixture]
    public class ProcessConnectToServerTest
    {
        [Test]
        public void Verbindung_zu_offenem_server_socket_aufbauen()
        {
            // einfach den PaintTogetherServeradapter aus dem Server
            // verwenden um einen Server für den Test zu haben
            IPtServerClientAdapter server = new PtServerClientAdapter();
            server.OnNewClient += message => Assert.True(true); /*Dummyverdrahtung*/
            server.ProcessStartPortListingMessage(new StartPortListingMessage { Port = 34567 });

            ConEstablishedMessage conEstMessage = null;
            IPtClientAdapterStarter clientStarter = new PtClientAdapterStarter();
            clientStarter.OnConEstablished += message => conEstMessage = message;

            // Verbindung aufbauen
            var startRequest = new ConnectToServerRequest { ServernameOrIp = "localhost", Port = 34567 };
            clientStarter.ProcessConnectToServerRequest(startRequest);

            // Jetzt gucken, ob wir eine Socketverbindung zum Server hergestellt haben
            Assert.That(conEstMessage.Socket.Connected, Is.True);
            Assert.That(startRequest.Result.ToUpper(), Contains.Substring("ERFOLG"));
        }

        [Test]
        public void Verbindung_zu_server_mit_nicht_erreichbarem_Port_aufbauen()
        {
            ConEstablishedMessage conEstMessage = null;
            IPtClientAdapterStarter clientStarter = new PtClientAdapterStarter();
            clientStarter.OnConEstablished += message => conEstMessage = message;

            // Verbindung aufbauen
            var startRequest = new ConnectToServerRequest { ServernameOrIp = "localhost", Port = 23456 };
            clientStarter.ProcessConnectToServerRequest(startRequest);

            Assert.IsNull(conEstMessage);
            Assert.That(startRequest.Result.ToUpper(), Contains.Substring("FEHLER"));
        }

        [Test]
        public void Verbindung_zu_nicht_erreichbarem_server_aufbauen()
        {
            ConEstablishedMessage conEstMessage = null;
            IPtClientAdapterStarter clientStarter = new PtClientAdapterStarter();
            clientStarter.OnConEstablished += message => conEstMessage = message;

            // Verbindung aufbauen
            var startRequest = new ConnectToServerRequest { ServernameOrIp = "unbekannterHost", Port = 34567 };
            clientStarter.ProcessConnectToServerRequest(startRequest);

            Assert.IsNull(conEstMessage);
            Assert.That(startRequest.Result.ToUpper(), Contains.Substring("FEHLER"));
        }
    }
}
