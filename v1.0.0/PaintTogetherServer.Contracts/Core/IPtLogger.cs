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
using PaintTogetherServer.Messages.Portal;

namespace PaintTogetherServer.Contracts.Core
{
    /// <summary>
    /// Fängt alle log4net-Nachrichten durch einen eigenen Appender ab
    /// und erzugt für jede Lognachricht ein Logevent
    /// </summary>
    internal interface IPtLogger
    {
        /// <summary>
        /// Informiert über eine neue (log4net)Lognachricht
        /// </summary>
        event Action<SLogMessage> OnSLog;
    }
}
