/**
 * Addition to the ListView class which should cause it to flicker less on unnecessary updates
 * 
 * Source: http://geekswithblogs.net/CPound/archive/2006/02/27/70834.aspx also posted on http://stackoverflow.com/questions/442817/c-sharp-flickering-listview-on-update
 * By: Dan Koster, 27 Feb 2006
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadCollor
{
    /// <summary>
    /// ListViewNF (No Flicker) is used to reduce unnecessary refreshes in the ListView
    /// </summary>
    class ListViewNF : System.Windows.Forms.ListView
    {
        public ListViewNF()
        {
            //Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
    }
}
