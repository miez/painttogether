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

using System.Collections.Generic;
using NUnit.Framework;

namespace PaintTogetherCommunicater.Test.PtMessageReceiverCS
{
    [TestFixture]
    public class ReadContentTest
    {
        [Test]
        public void Eine_Nachricht()
        {
            var content = new byte[] { 3, 1, 24, 5, 23, 231, 2, 24, 23, 111, 24, 234 };

            var byteList = new List<byte>();
            byteList.AddRange(PtMessageReceiver.StartBlock);
            byteList.AddRange(content);
            byteList.AddRange(PtMessageReceiver.EndBlock);

            var result = PtMessageReceiver.ReadContent(byteList);

            Assert.That(result.Key, Is.EqualTo(content));
            Assert.That(result.Value, Is.EqualTo(byteList.Count));
        }
    }
}
