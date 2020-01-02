using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MArchiveBatchTool.Psb;

namespace MArchiveBatchTool
{
    public class CliStreamSource : IPsbStreamSource
    {
        string baseDir;

        public CliStreamSource(string baseDir)
        {
            if (string.IsNullOrEmpty(baseDir)) throw new ArgumentNullException(nameof(baseDir));
            this.baseDir = baseDir;
        }

        public Stream GetStream(string identifier)
        {
            string postproc = identifier.TrimStart('_').Replace(':', '_');
            return File.OpenRead(Path.Combine(baseDir, postproc));
        }
    }
}
