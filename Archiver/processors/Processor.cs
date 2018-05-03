using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace archiver.processors
{
    public abstract class Processor
    {

        protected Stream inputStream;
        protected Stream outputStream;
        private bool isRun;
        protected bool isAborted;
        protected BlockReader blockReader;
        protected BlockWriter blockWriter;
        protected BlockHandler blocksHandler;
       // private CompressionMode operationType;
        private Thread producerThread;
        private Thread consumerThread;
        protected int currentBlockIndex;
        protected SortedList<int, Block> buffer;
        protected IZipOption option;
        protected long inputStreamLength;
        protected long outputStreamLength;
        protected long bytesReaded = 0;
        public Processor(IZipOption option)
        {
            this.option = option;
            buffer = new SortedList<int, Block>();
        }

        private void Process()
        {
            if (isAborted)
            {
                return;
            }
            try
            {
                producerThread = new Thread(Produce);
                producerThread.Priority = ThreadPriority.AboveNormal;
                consumerThread = new Thread(Consume);
                consumerThread.Priority = ThreadPriority.AboveNormal;
                producerThread.Start();
                blocksHandler.Start();
                consumerThread.Start();
                producerThread.Join();
                consumerThread.Join();
                if (!isAborted) blocksHandler.Stop();
            }
            catch (Exception ex)
            {
                OutputConsole.DisplayError(ex);
            }
        }
        protected void Callback(Exception ex)
        {
            
            OutputConsole.DisplayError(ex);
            Abort();
        }

       
        public bool Run()
        {
            Initialization();

            Process();

            return !isAborted;
        }
        protected abstract void Initialization();

        public void Abort()
        {
            isAborted = true;

            if (producerThread != null)
            {
                producerThread.Abort();
            }
            if (blocksHandler != null)
            {
                blocksHandler.Abort();
            }
            if (consumerThread != null)
            {
                consumerThread.Abort();
            }
        }
        protected abstract void ConsumeAction(); 

        protected void Consume()
        {          
            long processedBytes = 0;
            ConsumeAction();
            while (outputStreamLength - 1 > processedBytes)
            {
                try
                {
            Block nextBlock;
                    if (!buffer.TryGetValue(currentBlockIndex, out nextBlock))
                    {                       
                        nextBlock = blocksHandler.GetBlock();
                        if (currentBlockIndex == nextBlock.Id)
                        {
                            OutputConsole.ShowMessage("Обработано " + processedBytes / 1024 / 1024 + "Мб");
                            blockWriter.WriteNextBlock(nextBlock);
                            processedBytes += Config.BLOCK_SIZE;
                            currentBlockIndex++;
                        }
                        else
                        {
                            buffer.Add(nextBlock.Id, nextBlock);
                        }
                    }
                    else
                    {
                        buffer.Remove(currentBlockIndex);
                        OutputConsole.ShowMessage("Обработано " + processedBytes / 1024 / 1024 + "Мб");
                        blockWriter.WriteNextBlock(nextBlock);
                        processedBytes += Config.BLOCK_SIZE;
                        currentBlockIndex++;
                    }
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
        public abstract void Produce();             
    }
}
