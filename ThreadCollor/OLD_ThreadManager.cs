using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;

namespace ThreadCollor
{
    class OLD_ThreadManager
    {
        Queue<FileEntry> taskList;
        BackgroundWorker[] workers;
        //static bool runFlag;

        public OLD_ThreadManager(int numberOfThreads)
        {
            workers = new BackgroundWorker[numberOfThreads];
            //runFlag = true;

            for (int i = 0; i < workers.Length; i++)
            {
                BackgroundWorker worker = workers[i];
                worker = new BackgroundWorker()
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true,
                };
                worker.DoWork += work;
                worker.ProgressChanged += reportProgress;
            }
        }

        public void start(Queue<FileEntry> taskList)
        {
            this.taskList = taskList;

            foreach(BackgroundWorker worker in workers)
            {
                worker.RunWorkerAsync();
            }
        }

        public void stop()
        {
            //runFlag = false;
            //workers[1].
        }

        static void work(object sender, DoWorkEventArgs e)
        {
            while(false)
            {
                //Console.WriteLine("Doing nothing");
                
                //Thread.Sleep(1000);
            }
        }

        static void reportProgress(object sender, ProgressChangedEventArgs e)
        {
            //send data to main form
            
        }
    }
}