
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

using NUnit.Framework;
using PaintTogetherClient.Messages.Adapter;
using PaintTogetherClient.Core;
using PaintTogetherClient.Contracts.Core;

using System;
using PaintTogetherClient.Messages.Core.ClientStarter;

namespace PaintTogetherClient.Test.Core.PtClientStarterCS
{
    [TestFixture]
    public class ProcessPaintContentTest
    {
        [Test]
        public void PaintContent_ohne_vorherigen_start()
        {
            IPtClientStarter ptClientStarter = new PtClientStarter();

            try
            {
                ptClientStarter.ProcessCurrentPaintContentMessage(new CurrentPaintContentMessage());
                Assert.Fail("CurrentPaintContentMessage ohne vorherigen Start ist nicht gestattet");
            }
            catch (InvalidOperationException)
            {
                // Alles ok - Exception sollte ohne vorherige StartNachricht auftreten
            }
        }

        [Test]
        public void PaintContent_doppelt_senden()
        {
            IPtClientStarter ptClientStarter = new PtClientStarter();
            ptClientStarter.OnRequestConnectToServer += request => Assert.True(true);// Dummyverdrahtung
            // Ohne StartClient wird der Content nicht verarbeitet
            ptClientStarter.ProcessStartClientRequest(new StartClientRequest());

            ptClientStarter.ProcessCurrentPaintContentMessage(new CurrentPaintContentMessage());

            try
            {
                ptClientStarter.ProcessCurrentPaintContentMessage(new CurrentPaintContentMessage());
                Assert.Fail("CurrentPaintContentMessage darf nicht zweimal verarbeitet werden!");
            }
            catch (InvalidOperationException)
            {
                // Alles ok - Exception sollte bei doppeltem Aufruf auftreten
            }
        }
    }
}
