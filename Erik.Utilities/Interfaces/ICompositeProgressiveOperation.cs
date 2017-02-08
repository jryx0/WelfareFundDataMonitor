using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Erik.Utilities.Interfaces
{
    public interface ICompositeProgressiveOperation :
        IProgressiveOperation
    {
        IProgressiveOperation CurrentOperation { get; }

        event EventHandler NewOperation;
    }
}
