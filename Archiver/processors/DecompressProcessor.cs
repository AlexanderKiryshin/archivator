using archiver.blockReaders;
using archiver.blockWriters;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace archiver.processors
{
   public class DecompressProcessor:Processor
    {
        long fileSize;
        
        public DecompressProcessor(IZipOption option) : base(option)
        {
        }
        protected override void Initialization()
        {
            try
            {
                inputStream = BlockReader.GetInputStream(option.InputFileName);
                outputStream = BlockWriter.GetOutputStream(option.OutputFileName);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не найден");
                isAborted = true;
                return;
            }
            catch (FileLoadException ex)
            {
                OutputConsole.DisplayError(ex);
                isAborted = true;
                return;
            }
            blockReader = new CompressedBlockReader(inputStream);
            blockWriter = new DecompressedBlockWriter(outputStream);
            blocksHandler = new BlockHandler(Environment.ProcessorCount, ex => Callback(ex), CompressionMode.Decompress);
        }
        protected override void ConsumeAction()
        {
        }
        public override void Produce()
        {
            inputStreamLength = inputStream.Length;
            fileSize = BitConverter.ToInt64(((CompressedBlockReader)blockReader).ReadFileSize(), 0);
            outputStreamLength = fileSize;
           // streamLength = fileSize;
            while (inputStreamLength - 1 > inputStream.Position)
            {
                try
                {
                    Block nextBlock;
                            nextBlock = blockReader.GetNextBlock(((CompressedBlockReader)blockReader).ReadBlockSize());
                        if (fileSize < bytesReaded + Config.BLOCK_SIZE)
                        {
                            nextBlock.Size = (int)(fileSize - bytesReaded);
                        }
                            blocksHandler.AddBlock(nextBlock);
                        bytesReaded += Config.BLOCK_SIZE;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    isAborted = true;
                    OutputConsole.DisplayError(ex);
                    return;
                }
                catch (ThreadAbortException)
                {
                    return;
                }
            }
        }
    }
}
