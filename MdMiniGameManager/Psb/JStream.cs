using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MArchiveBatchTool.Psb
{
    public class JStream : JValue
    {
        byte[] binaryDataBacking;
        uint index;
        bool isBStream;

        internal PsbReader Reader { get; set; }
        public byte[] BinaryData
        {
            get
            {
                if (binaryDataBacking == null && Reader != null)
                {
                    binaryDataBacking = Reader.GetStreamData(this);
                    Reader = null;
                }
                return binaryDataBacking;
            }
            set
            {
                binaryDataBacking = value;
                Reader = null;
            }
        }

        public uint Index
        {
            get
            {
                return index;
            }
            internal set
            {
                index = value;
                UpdateName();
            }
        }

        public bool IsBStream
        {
            get
            {
                return isBStream;
            }
            internal set
            {
                isBStream = value;
                UpdateName();
            }
        }

        public JStream(bool isBStream) : base(string.Format("_{0}stream:new", isBStream ? "b" : ""))
        {
            this.isBStream = isBStream;
        }

        internal JStream(uint index, bool isBStream, PsbReader parent = null) : base(string.Empty)
        {
            this.index = index;
            this.isBStream = isBStream;
            Reader = parent;
            UpdateName();
        }

        void UpdateName()
        {
            Value = string.Format("_{0}stream:{1}", isBStream ? "b" : "", index);
        }

        public static JStream CreateFromStringRepresentation(string rep)
        {
            string[] split = rep.Split(':');
            if (split.Length != 2 || split[0] != "_stream" && split[0] != "_bstream")
                throw new ArgumentException("String is not stream representation.", nameof(rep));
            uint index = uint.Parse(split[1]);
            bool isBStream = split[0] == "_bstream";
            return new JStream(index, isBStream);
        }
    }
}
