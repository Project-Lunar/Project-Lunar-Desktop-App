using System;
using System.Collections.Generic;
using System.Text;

namespace MArchiveBatchTool.Psb
{
    [Flags]
    public enum PsbFlags
    {
        None = 0,
        HeaderFiltered = 1 << 0,
        BodyFiltered = 1 << 1,
    }
}
