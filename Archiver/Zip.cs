using System.IO;
using System.IO.Compression;
namespace archiver
{
   public class Zip
    {
        public Block Compress(Block block)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory,CompressionMode.Compress))
                {
                    int blockSize = block.Data.Length;
                        gzip.Write(block.Data, 0, blockSize);               
                }
                block.Data = memory.ToArray();
                block.Size = block.Data.Length;
                return block;
            }
        }
        public Block Decompress(Block block)
        {
                using (MemoryStream memory = new MemoryStream(block.Data))
                {
                    using (GZipStream gzip = new GZipStream(memory,CompressionMode.Decompress))
                    {
                            block.Data = new byte[block.Size];
                            gzip.Read(block.Data, 0, block.Size);
                    }
                    block.Size = block.Data.Length;
                    return block;
                }
            }
        }

    
}
