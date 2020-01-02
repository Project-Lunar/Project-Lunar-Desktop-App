using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MArchiveBatchTool.Psb.Writing
{
    public class RegularNameNode : NameNode
    {
        public uint ValueOffset { get; set; }
        public Dictionary<byte, NameNode> Children { get; } = new Dictionary<byte, NameNode>();

        public override void WriteDot(TextWriter writer)
        {
            string outputChar;
            char ch = (char)Character;
            if (char.IsControl(ch) || char.IsWhiteSpace(ch))
                outputChar = string.Format("0x{0:x2}", Character);
            else
                outputChar = ch.ToString();

            if (Index != 0)
            {
                writer.WriteLine("{0} [label=\"{{{{index|\\<{1}\\>}}|{{valueOffset|{2}}}|{{char|{3}}}}}\"];",
                Index, Index, ValueOffset, outputChar);
                writer.WriteLine("{1} -> {0};", Index, ParentIndex);
            }
            else
            {
                writer.WriteLine("{0} [label=\"{{{{index|\\<{1}\\>}}|{{valueOffset|{2}}}}}\"];", Index, Index, ValueOffset);
            }
        }
    }
}
