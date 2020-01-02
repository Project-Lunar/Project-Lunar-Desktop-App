using System;
using System.Collections.Generic;
using System.Text;

namespace MArchiveBatchTool.Psb
{
    enum PsbTokenType
    {
        Invalid,
        Null,
        Bool,
        Int,
        Long,
        UInt,
        Key,
        String,
        Stream,
        Float,
        Double,
        TokenArray,
        Object,
        BStream
    }
}
