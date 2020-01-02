using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MArchiveBatchTool.Psb.Writing
{
    public abstract class NameNode
    {
        public uint Index { get; set; }
        public uint ParentIndex { get; set; }
        public NameNode Parent { get; set; }
        public byte Character { get; set; }

        public abstract void WriteDot(TextWriter writer);
    }
}
