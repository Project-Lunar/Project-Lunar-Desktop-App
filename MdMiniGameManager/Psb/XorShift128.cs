using System;
using System.Collections.Generic;
using System.Text;

namespace MArchiveBatchTool.Psb
{
    /// <summary>
    /// Random number generator used in E-mote with custom W seed
    /// 
    /// See http://dx.doi.org/10.18637/jss.v008.i14, page 5, xor128()
    /// </summary>
    class XorShift128
    {
        uint x = 123456789;
        uint y = 362436069;
        uint z = 521288629;
        uint w;

        public XorShift128(uint seed = 88675123)
        {
            w = seed;
        }

        public uint Next()
        {
            uint t = x ^ (x << 11);
            x = y;
            y = z;
            z = w;
            w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
            return w;
        }
    }
}
