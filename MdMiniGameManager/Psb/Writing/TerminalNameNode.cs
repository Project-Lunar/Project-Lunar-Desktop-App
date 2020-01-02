using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MArchiveBatchTool.Psb.Writing
{
    public class TerminalNameNode : NameNode
    {
        public uint TailIndex { get; set; }

        public override void WriteDot(TextWriter writer)
        {
            string outputChar;
            char ch = (char)Character;
            if (char.IsControl(ch) || char.IsWhiteSpace(ch))
                outputChar = string.Format("0x{0:x2}", Character);
            else
                outputChar = ch.ToString();

            writer.WriteLine("{0} [label=\"{{{{index|\\<{1}\\>}}|{{tailIndex|{2}}}|{{char|{3}}}}}\"];",
                Index, Index, TailIndex, outputChar);
            writer.WriteLine("{1} -> {0};", Index, ParentIndex);
        }
    }
}
