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

using PaintTogetherStartSelector.Contracts;
using PaintTogetherStartSelector.Portal;
using System.Windows.Forms;

namespace PaintTogetherStartSelector.Run
{
    /// <summary>
    /// Platine die die vier EBCs der StartSelectoranwendung verbindet
    /// </summary>
    public class StartSelector
    {
        /// <summary>
        /// Die Portal-EBC
        /// </summary>
        private readonly IPtStartSelectorPortal _portal = new PtStartSelectorPortal();

        /// <summary>
        /// Die ClientStarterAdapter-EBC
        /// </summary>
        private readonly IPtStartClientAdapter _clientAdapter = new PtStartClientAdapter();

        /// <summary>
        /// Die ServerStarterAdapter-EBC
        /// </summary>
        private readonly IPtStartServerAdapter _serverAdapter = new PtStartServerAdapter();

        /// <summary>
        /// Die funktionale KernEBC der Startanwendung
        /// </summary>
        private readonly IPtStartSelector _core = new PtStartSelector();

        /// <summary>
        /// Startform für Application.Run
        /// </summary>
        internal Form Portal
        {
            get
            {
                return _portal as Form;
            }
        }

        internal StartSelector()
        {
            // die 4 EBCs verbinden, dabei einfach von allen EBC die Outpins (Events)
            // mit einen entsprechenden Inputpin (Process..-Methode) verbinden
            _portal.OnConnectToPicture += _core.ProcessConnectToPictureMessage;
            _portal.OnCreateNewPicture += _core.ProcessCreateNewPictureMessage;
            _portal.OnRequestTestLocalPort += _core.ProcessTestLocalPortRequest;
            _portal.OnRequestTestServer += _core.ProcessTestServerRequest;
            _core.OnStartClient += _clientAdapter.ProcessStartClientMessage;
            _core.OnStartServer += _serverAdapter.ProcessStartServerMessage;
        }
    }
}
