using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Erik.Utilities.WinFormsUI;
using Erik.Utilities.Interfaces;

namespace Erik.Utilities
{
    public static class Lib
    {
        public static void StartProgressiveOperation(
            IProgressiveOperation operation,
            IWin32Window owner)
        {
            frmModalProgressForm f = 
                new frmModalProgressForm(operation);

            f.ShowDialog(owner);
        }

        public static void StartProgressiveOperation(
        ICompositeProgressiveOperation operation,
        IWin32Window owner)
        {
            frmModalCompositeProgressForm f =
                new frmModalCompositeProgressForm(operation);

            f.ShowDialog(owner);
        }
    }
}
