using System;
using System.Collections.Generic;
using System.Text;

namespace MArchiveBatchTool.Psb
{
    public class EmoteCryptFilter : IPsbFilter
    {
        XorShift128 rand;
        uint buffer;
        int bytesLeft;

        public EmoteCryptFilter(uint seed)
        {
            rand = new XorShift128(seed);
        }

        public void Filter(byte[] data)
        {
            for (int i = 0; i < data.Length; ++i)
            {
                if (buffer == 0) // M2 bug: they're checking buffer instead of bytes left
                {
                    buffer = rand.Next();
                    bytesLeft = sizeof(uint);
                }

                data[i] ^= (byte)buffer;
                buffer >>= 8;
                --bytesLeft;
            }
        }
    }
}
