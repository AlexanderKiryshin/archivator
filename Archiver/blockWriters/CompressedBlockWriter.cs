using System;
using System.IO;

namespace archiver.blockWriters
{
    public class CompressedBlockWriter : BlockWriter
    {
        public CompressedBlockWriter(Stream stream) : base(stream)
        {
        }
        public override void WriteNextBlock(Block block)
        {
            stream.Write(BitConverter.GetBytes(block.Data.Length), 0, 4);
            stream.Write(block.Data, 0, block.Data.Length);
        }
        public void WriteFileSize(long size)
        {
            stream.Write(BitConverter.GetBytes(size), 0, 8);
        }
    }
}
