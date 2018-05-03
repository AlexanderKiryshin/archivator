using archiver.blockReaders;
using archiver.blockWriters;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace archiver.processors
{
    public class CompressProcessor:Processor
    {
        public CompressProcessor(IZipOption option) : base(option)
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
            blockReader = new UncompressedBlockReader(inputStream);
            blockWriter = new CompressedBlockWriter(outputStream);
            blocksHandler = new BlockHandler(Environment.ProcessorCount, ex => Callback(ex), CompressionMode.Compress);
        }
        protected override void ConsumeAction()
        {
            ((CompressedBlockWriter)blockWriter).WriteFileSize(inputStream.Length);
        }
        public override void Produce()
        {
            inputStreamLength = inputStream.Length;
            outputStreamLength = inputStreamLength;
            while (inputStreamLength - 1 > inputStream.Position)
            {
                try
                {
                    Block nextBlock;
                        if (inputStreamLength > bytesReaded + Config.BLOCK_SIZE)
                        {
                            nextBlock = blockReader.GetNextBlock(Config.BLOCK_SIZE);
                        }
                        else
                        {
                        nextBlock=blockReader.GetNextBlock((int)(inputStreamLength - bytesReaded));
                        }
                            blocksHandler.AddBlock(nextBlock);
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
