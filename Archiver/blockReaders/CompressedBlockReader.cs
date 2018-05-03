using System;
using System.IO;

namespace archiver.blockReaders
{
    public class CompressedBlockReader:BlockReader
    {
        public CompressedBlockReader(Stream stream):base(stream)
        {
        }
        public int ReadBlockSize()
        {
            byte[] blockSize = new byte[BLOCK_SIZE_DATA];
            stream.Read(blockSize, 0, BLOCK_SIZE_DATA);
            return BitConverter.ToInt32(blockSize, 0);
        }

        public byte[] ReadFileSize()
        {
            byte[] buffer = new byte[8];
            stream.Read(buffer, 0, 8);
            return buffer;
        }
    }
}
