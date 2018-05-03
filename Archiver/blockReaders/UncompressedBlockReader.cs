using System.IO;
namespace archiver.blockReaders
{
    public class UncompressedBlockReader:BlockReader
    {
       public UncompressedBlockReader(Stream stream) : base(stream)
        {
        }
    }
}
