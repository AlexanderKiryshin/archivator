using System;
using System.Threading;
using System.IO.Compression;

namespace archiver
{
    /// <summary>
    ///архивирует/разархивирует блок данных
    /// </summary>
    public class BlockHandler
    {
        private CompressionMode operType;
        private int threadsCount;
        private readonly Thread[] workers;
        private readonly Buffer<Block> input;
        private readonly Buffer<Block> output;
        Action<Exception> callback;
        public void Start()
        {
            for (int i = 0; i < workers.Length; i++)
            {
                workers[i].IsBackground = true;
                workers[i].Start();
            }
        }
        public void Abort()
        {
            for (int i = 0; i < workers.Length; i++)
            {
                workers[i].Abort();
            }
        }

        /// <param name="threadsCount">количество потоков</param>
        /// <param name="callback">метод который вызывать в случае ошибки</param>
        public BlockHandler(int threadsCount, Action<Exception> callback, CompressionMode operType)
        {
            this.operType = operType;
            input = new Buffer<Block>(threadsCount * 2);
            output = new Buffer<Block>(threadsCount * 2);
            workers = new Thread[threadsCount];
            for (int i = 0; i < threadsCount; i++)
            {
                workers[i] = new Thread(() => Run());
                workers[i].Priority = ThreadPriority.Lowest;
            }
            this.callback = callback;
        }
        protected void Run()
        {
            Zip zip = new Zip();
            while (true)
            {
                Block block = null;
                try
                {
                    block = input.GetItem();
                    if (block != null)
                    {
                        Block processedBlock;
                        if (operType == CompressionMode.Compress)
                        {
                            processedBlock = zip.Compress(block);
                        }
                        else
                        {
                            processedBlock = zip.Decompress(block);
                        }

                        output.AddItem(processedBlock);
                        //OutputConsole.DisplayBlockHash(block.Id + 1, hash);
                    }
                    else
                    {
                        return;
                    }
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    callback?.Invoke(ex);
                    return;
                }
            }
        }
        public void Stop()
        {
            for (int i = 0; i < workers.Length; i++)
                input.AddItem(null);
            foreach (var w in workers)
            {
                w.Join();
            }
        }
        public void AddBlock(Block block)
        {
            input.AddItem(block);
        }
        public Block GetBlock()
        {
            return output.GetItem();
        }
    }
}

