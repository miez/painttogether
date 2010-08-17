
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
using PaintTogetherStartSelector.Portal;

namespace PaintTogetherStartSelector.Test.Portal.ValidateInputUtilsCS
{
    [TestFixture]
    public class ValidateAliasTest
    {
        [Test]
        public void gueltiger_Alias_buchstaben()
        {
            Assert.IsNullOrEmpty(ValidateInputUtils.ValidateAlias("gueltig"));
        }

        [Test]
        public void gueltiger_Alias_umlaute()
        {
            Assert.IsNullOrEmpty(ValidateInputUtils.ValidateAlias("gültig"));
        }

        [Test]
        public void gueltiger_Alias_sz()
        {
            Assert.IsNullOrEmpty(ValidateInputUtils.ValidateAlias("spaß"));
        }

        [Test]
        public void gueltiger_Alias_sonderzeichen()
        {
            Assert.IsNullOrEmpty(ValidateInputUtils.ValidateAlias("-:_"));
        }

        [Test]
        public void gueltiger_Alias_leerzeichen()
        {
            Assert.IsNullOrEmpty(ValidateInputUtils.ValidateAlias("was gibts"));
        }

        [Test]
        public void gueltiger_Alias_zahlen()
        {
            Assert.IsNullOrEmpty(ValidateInputUtils.ValidateAlias("12354125"));
        }

        [Test]
        public void ungueltiger_Alias()
        {
            Assert.IsNotNullOrEmpty(ValidateInputUtils.ValidateAlias("ungueltig]]["));
        }
    }
}
