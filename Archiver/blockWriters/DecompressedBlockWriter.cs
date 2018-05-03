using System.IO;

namespace archiver.blockWriters
{
   public class DecompressedBlockWriter:BlockWriter
    {
        public DecompressedBlockWriter(Stream stream) : base(stream)
        {
        }
        public override void WriteNextBlock(Block block)
        {
            stream.Write(block.Data, 0, block.Data.Length);
        }
    }
}
