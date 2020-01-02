using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MArchiveBatchTool.Psb
{
    public interface IPsbStreamSource
    {
        Stream GetStream(string identifier);
    }
}
