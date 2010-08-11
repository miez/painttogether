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

namespace PaintTogetherServer.Run
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var argList = new List<string>();
                argList.AddRange(args);

                if (argList.Contains("-help") || argList.Contains("-h"))
                {
                    PrintHelp();
                    Console.Read();
                    return;
                }

                var startParams = StartServerParams.ParseArgs(argList);

                if (!startParams.Valid)
                {
                    Console.WriteLine("Ungültige Startparameter");
                    PrintHelp();
                    Console.Read();
                    return;
                }

                var server = new Server();
                server.Start(startParams);
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler beim Starten des PaintTogetherServers");
                Console.WriteLine(e);
            }

            // Beenden sobald eine Taste gedrückt wird
            var close = false;
            while (!close)
            {
                Console.WriteLine("Zum Beenden des Servers 'x' drücken");
                var key = Console.ReadKey();
                close = key.Key == ConsoleKey.X;
            }

            Environment.Exit(0); // Prozess beenden -> Nur so CloseEvent im Portal
        }

        private static void PrintHelp()
        {
            Console.WriteLine("======= PaintTogetherServer =======");
            Console.Write("Dieses Programm stellt den Server für das gemeinsame Zeichnen ");
            Console.Write("mit einfachen Stiften auf einer Maloberfläche da. ");
            Console.Write("Mit der dem PaintTogetherClient kann man sich an/mit einer ");
            Console.WriteLine("Malerei eines PaintTogetherServers beteiligen/verbinden.");
            Console.WriteLine("=========================");
            Console.WriteLine("Folgende Parameter stehen zur Verfügung:");
            Console.WriteLine(StartServerParams.AliasParamName);
            Console.WriteLine(string.Format("   (optional) Beliebiger Name der Person die den Server startet - Standardname ist '{0}'", StartServerParams.DefaultAlias));
            Console.WriteLine(StartServerParams.HeightParamName);
            Console.WriteLine(string.Format("   (optional) Höhe des Malbereichs, muss zwischen '{0}' und '{1}' liegen. Standard ist '{2}'", StartServerParams.MinHeight, StartServerParams.MaxHeight, StartServerParams.DefaultHeigth));
            Console.WriteLine(StartServerParams.WidthParamName);
            Console.WriteLine(string.Format("   (optional) Breite des Malbereichs, muss zwischen '{0}' und '{1}' liegen. Standard ist '{2}'", StartServerParams.MinWidth, StartServerParams.MaxWidth, StartServerParams.DefaultWidth));
            Console.WriteLine(StartServerParams.PortParamName);
            Console.WriteLine(string.Format("   (optional) Port des Servers, muss zwischen '{0}' und '{1}' liegen. Standard ist '{2}'", StartServerParams.MinPort, StartServerParams.MaxPort, StartServerParams.DefaultPort));
        }
    }
}
