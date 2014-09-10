using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

namespace ThreadCollor
{
    class ThreadManager
    {
        Queue<string> taskList;
        ColorCalculator[] workers;
        ListView listView_overview;

        public ThreadManager(int numberOfThreads, ListView listView_overview)
        {
            this.listView_overview = listView_overview;
            workers = new ColorCalculator[numberOfThreads];

            for (int i = 0; i < workers.Length; i++)
            {
                workers[i] = new ColorCalculator(listView_overview);
                //ColorCalculator worker = workers[i];
                //worker = new ColorCalculator();
                //worker.DoWork += work;
                //worker.ProgressChanged += reportProgress;
            }
        }

        public void start(Queue<string> taskList)
        {
            this.taskList = taskList;

            foreach(ColorCalculator worker in workers)
            {
                worker.RunWorkerAsync();
            }
        }

        public void stop()
        {
        }

        static void work(object sender, DoWorkEventArgs e)
        {
        }

        static void reportProgress(object sender, ProgressChangedEventArgs e)
        {
        }
    }
}