using System;
using System.IO;

namespace archiver
{
  public abstract class BlockWriter
    {
        protected readonly Stream stream;

        public BlockWriter(Stream stream)
        {
            this.stream = stream;
        }
        public static Stream GetOutputStream(string inputFile)
        {
            if (File.Exists(inputFile))
            {
                throw new ArgumentException("Выходной файл уже существует.", inputFile);
            }

            return File.Open(inputFile, FileMode.CreateNew, FileAccess.Write, FileShare.Write);
        }

        public static void DisposeStream(Stream stream)
        {
            if (stream != null)
            {
                stream.Close();
                stream.Dispose();
            }
        }
        public abstract void WriteNextBlock(Block block);     
    }
}

