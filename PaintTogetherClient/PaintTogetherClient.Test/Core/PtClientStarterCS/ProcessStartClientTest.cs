
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

using System.Drawing;
using NUnit.Framework;
using PaintTogetherClient.Messages.Adapter;
using PaintTogetherClient.Messages.Core.ClientStarter;
using PaintTogetherClient.Core;
using PaintTogetherClient.Contracts.Core;
using PaintTogetherClient.Messages.Core.PaintContentManager;
using PaintTogetherClient.Messages.Portal;
using System;

namespace PaintTogetherClient.Test.Core.PtClientStarterCS
{
    [TestFixture]
    public class ProcessStartClientTest
    {
        [Test]
        public void Fehler_bei_Serververbindung_simulieren()
        {
            var request = new StartClientRequest();
            request.Alias = "Theodor";
            request.Color = Color.FromArgb(123, 21, 24);
            request.Port = 12345;
            request.ServernameOrIp = "servername";

            IPtClientStarter ptClientStarter = new PtClientStarter();
            ptClientStarter.OnRequestConnectToServer += sRequest => sRequest.Result = "Fehler";

            ptClientStarter.ProcessStartClientRequest(request);

            Assert.That(request.Result, Is.EqualTo("Fehler"));
        }

        [Test]
        public void Startnachricht_doppelt_senden()
        {
            IPtClientStarter ptClientStarter = new PtClientStarter();
            ptClientStarter.OnRequestConnectToServer += sRequest => sRequest.Result = "egal";
            ptClientStarter.ProcessStartClientRequest(new StartClientRequest());

            try
            {
                ptClientStarter.ProcessStartClientRequest(new StartClientRequest());
                Assert.Fail("2facher Start nicht gestattet");
            }
            catch (InvalidOperationException)
            {
                // Alles ok - Exception sollte bei doppeltem Aufruf auftreten
            }
        }

        /// <summary>
        /// Der ClientStarter wirft erst die Init-Events, wenn
        /// er vom Server den Malbereichsinhalt erhalten hat.
        /// </summary>
        [Test]
        public void Erfolgreiche_Initialisierung_simulieren()
        {
            // Zu erhaltende Nachrichten
            InitPortalMessage initPortalM = null;
            InitPaintManagerMessage initPaintM = null;

            IPtClientStarter ptClientStarter = new PtClientStarter();
            ptClientStarter.OnInitPaintManager += initPaint => initPaintM = initPaint;
            ptClientStarter.OnInitPortal += initPortal => initPortalM = initPortal;
            ptClientStarter.OnRequestConnectToServer += conRequest => Assert.True(true)/*Dummyverdrahtung -> Verhindern einer NRE*/ ;

            var request = new StartClientRequest();
            request.Alias = "Theodor";
            request.Color = Color.FromArgb(123, 21, 24);
            request.Port = 12345;
            request.ServernameOrIp = "servername";
            ptClientStarter.ProcessStartClientRequest(request);

            var pContent = new Bitmap(30, 23);
            var alias = "Peter Fox";

            // Die beiden InitOutputs sollen erst Nachrichten senden, wenn 
            // der Server dem ClientStarter den Malbereich übergibt und wenn
            // die ConnectedNachricht eingetroffen ist
            ptClientStarter.ProcessCurrentPaintContentMessage(new CurrentPaintContentMessage { PaintContent = pContent });
            ptClientStarter.ProcessConnectedMessage(new ConnectedMessage { Alias = alias });

            // PaintManager Initialisierung vergleichen
            Assert.That(initPaintM.PaintContent, Is.EqualTo(pContent));

            // Portal Initialisierung vergleichen
            Assert.That(initPortalM.PaintContent, Is.EqualTo(pContent));
            Assert.That(initPortalM.Alias, Is.EqualTo(request.Alias));
            Assert.That(initPortalM.Color, Is.EqualTo(request.Color));
            Assert.That(initPortalM.ServerAlias, Is.EqualTo(alias));
            Assert.That(initPortalM.ServerName, Is.EqualTo(request.ServernameOrIp));
            Assert.That(initPortalM.ServerPort, Is.EqualTo(request.Port));
        }
    }
}
