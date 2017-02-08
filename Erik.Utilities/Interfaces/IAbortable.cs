using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Erik.Utilities.Interfaces
{
    public interface IAbortable
    {
        event EventHandler Aborted;

        void Abort();
    }
}
