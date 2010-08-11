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
using System.Diagnostics;
using PaintTogetherServer.Contracts;
using PaintTogetherServer.Messages.Portal;

namespace PaintTogetherServer
{
    /// <summary>
    /// GUI für den PaintTogetherServer. Hierbei handelt
    /// es sich nicht um eine richtige GUI, sondern nur um
    /// ein Hilfsmittel bei einer Konsolenanwendung.
    /// </summary>
    public class PtServerPortal : IPtServerPortal
    {
        /// <summary>
        /// Informiert über die Beendigung des Servers
        /// </summary>
        public event Action<CloseMessage> OnServerClose;

        public PtServerPortal()
        {
            // Für das Beenden der Konsole registrieren
            var process = Process.GetCurrentProcess();
            process.Exited += ProcessExited;
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            OnServerClose(new CloseMessage());
        }

        /// <summary>
        /// Verarbeitet eine Lognachricht
        /// </summary>
        /// <param name="message"></param>
        public void ProcessSLogMessage(SLogMessage message)
        {
            Console.WriteLine(message.Message);
        }
    }
}
