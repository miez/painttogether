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
using System.Text.RegularExpressions;
using PaintTogetherServer.Run;

namespace PaintTogetherStartSelector.Portal
{
    /// <summary>
    /// Stellt Methoden zur Validierung der Nutzereingaben bereit
    /// </summary>
    internal static class ValidateInputUtils
    {
        /// <summary>
        /// Validiert einen Alias ob er nur aus gültigen Zeichen besteht
        /// </summary>
        /// <param name="alias"></param>
        /// <returns>null wenn gültig, oder Nutzerbenachrichtigung</returns>
        public static string ValidateAlias(string alias)
        {
            var regex = new Regex(StartServerParams.AliasRegEx);
            var match = regex.Match(alias);
            if (match != null && match.Success && match.Length == alias.Length)
            {
                return null;
            }
            return "Ungültige Zeichen vorhanden. Es sind nur Buchstaben, Zahlen und Leerzeichen und '-', ':', '_' erlaubt.";
        }

        /// <summary>
        /// Validiert die Breite des Malbereichs
        /// </summary>
        /// <param name="width"></param>
        /// <returns>null wenn gültig, oder Nutzerbenachrichtigung</returns>
        public static string ValidateWidth(string width)
        {
            int result;
            if (!Int32.TryParse(width, out result))
            {
                return "Sie müssen eine Zahl für die Breite angeben.";
            }

            if (result < StartServerParams.MinWidth)
            {
                return string.Format("Sie können keine Breite kleiner als {0} angeben.", StartServerParams.MinWidth);
            }

            if (result > StartServerParams.MaxWidth)
            {
                return string.Format("Sie können keine Breite größer als {0} angeben.", StartServerParams.MaxWidth);
            }
            return null;
        }

        /// <summary>
        /// Validiert die Höhe des Malbereichs
        /// </summary>
        /// <param name="height"></param>
        /// <returns>null wenn gültig, oder Nutzerbenachrichtigung</returns>
        public static string ValidateHeight(string height)
        {
            int result;
            if (!Int32.TryParse(height, out result))
            {
                return "Sie müssen eine Zahl für die Höhe angeben.";
            }

            if (result < StartServerParams.MinHeight)
            {
                return string.Format("Sie können keine Höhe kleiner als {0} angeben.", StartServerParams.MinHeight);
            }

            if (result > StartServerParams.MaxHeight)
            {
                return string.Format("Sie können keine Höhe größer als {0} angeben.", StartServerParams.MaxHeight);
            }
            return null;
        }

        /// <summary>
        /// Validiert den Port eines Servers
        /// </summary>
        /// <param name="port"></param>
        /// <returns>null wenn gültig, oder Nutzerbenachrichtigung</returns>
        public static string ValidatePort(string port)
        {
            int result;
            if (!Int32.TryParse(port, out result))
            {
                return "Sie müssen eine Zahl für den Port angeben.";
            }

            if (result < StartServerParams.MinPort)
            {
                return string.Format("Sie können keinen Port kleiner als {0} angeben.", StartServerParams.MinPort);
            }

            if (result > StartServerParams.MaxPort)
            {
                return string.Format("Sie können keinen Port größer als {0} angeben.", StartServerParams.MaxPort);
            }
            return null;
        }
    }
}
