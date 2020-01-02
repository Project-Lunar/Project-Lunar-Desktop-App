using System;
using System.Collections.Generic;
using System.Text;

namespace MArchiveBatchTool.Psb
{
    public interface IPsbFilter
    {
        void Filter(byte[] data);
    }
}
