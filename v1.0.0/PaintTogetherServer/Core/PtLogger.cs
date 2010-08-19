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
using log4net.Config;
using log4net.Core;
using PaintTogetherServer.Contracts.Core;
using PaintTogetherServer.Messages.Portal;
using log4net.Appender;

namespace PaintTogetherServer.Core
{
    /// <summary>
    /// Fängt alle log4net-Nachrichten durch einen eigenen Appender ab
    /// und erzugt für jede Lognachricht ein Logevent
    /// </summary>
    internal class PtLogger : IPtLogger
    {
        private class LogEventAppender : IAppender
        {
            public void Close() { }

            public void DoAppend(LoggingEvent loggingEvent)
            {
                // Hier muss der Outpin auf Verwendung geprüft werden
                // Da es sein kann, das schon während der Verdrahtung der 
                // In- und Outputpins eine log4net-Nachricht erzeugt wird
                // Diese darf dann hier nicht zum Fehler führen
                if (OnSLog == null) return;

                var message = new SLogMessage();
                message.Message = string.Concat(loggingEvent.LoggerName, ":", loggingEvent.Level, ":", loggingEvent.GetLoggingEventData().Message);
                OnSLog(message);

                // Bei einer Exception einfach zwei Nachrichten verschicken
                if (loggingEvent.ExceptionObject != null)
                {
                    message = new SLogMessage();
                    message.Message = loggingEvent.ExceptionObject.ToString();
                    OnSLog(message);
                }
            }

            public event Action<SLogMessage> OnSLog;

            public string Name { get; set; }
        }

        /// <summary>
        /// EBC initialisieren
        /// </summary>
        public PtLogger()
        {
            var appender = new LogEventAppender();
            appender.OnSLog += message => OnSLog(message);
            BasicConfigurator.Configure(appender);
        }

        /// <summary>
        /// Informiert über eine neue (log4net)Lognachricht
        /// </summary>
        public event Action<SLogMessage> OnSLog;
    }
}
