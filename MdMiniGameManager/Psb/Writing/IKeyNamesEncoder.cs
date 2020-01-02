using System;
using System.Collections.Generic;
using System.Text;

namespace MArchiveBatchTool.Psb.Writing
{
    public interface IKeyNamesEncoder
    {
        bool IsProcessed { get; }
        int TotalSlots { get; }
        void Process(RegularNameNode root, int totalNodes);
    }
}
