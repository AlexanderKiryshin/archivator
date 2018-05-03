using System.IO;
namespace archiver
{
    public abstract class BlockReader
    {
        protected const int BLOCK_SIZE_DATA= 4;
        protected readonly Stream stream;
        protected int blockCounter;

        public BlockReader(Stream stream)
        {
            this.stream = stream;
        }
        public static Stream GetInputStream(string inputFile)
        {
            if (!File.Exists(inputFile))
            {
                throw new FileNotFoundException("Файл не найден", inputFile);
            }

            return File.Open(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public static void DisposeStream(Stream stream)
        {
            if (stream != null)
            {
                stream.Close();
                stream.Dispose();
            }
        }
        protected byte[] GetNextBlockBytes(int amountBytes)
        {
            byte[] buffer = new byte[amountBytes];
            int bytesReaded = stream.Read(buffer, 0, amountBytes);

            if (bytesReaded == 0)
                return null;

            return buffer;
        }
        public Block GetNextBlock(int blockSize)
        {
            var buffer = GetNextBlockBytes(blockSize);

            if (buffer != null)
            {
                Block block = new Block()
                {
                    Id = blockCounter++,
                    Size =Config.BLOCK_SIZE,
                    Data = buffer
                };
                return block;
            }
           
            return null;
        }

    }
}