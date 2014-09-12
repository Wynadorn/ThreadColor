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
        Queue<FileEntry> taskList;
        ColorCalculator[] workers;
        ListView listView_overview;

        public ThreadManager(ListView listView_overview)
        {
            this.listView_overview = listView_overview;
        }

        public void startThreads(Queue<FileEntry> taskList, int numberOfThreads)
        {
            //Create a local copy of the task list
            this.taskList = taskList;
            
            //Give the workers array a lenght
            workers = new ColorCalculator[numberOfThreads];

            //Fill the workers array with ColorCalculators
            for (int i = 0; i < workers.Length; i++)
            {
                //
                workers[i] = new ColorCalculator(listView_overview, taskList);
            }

            //Set all the workers to work
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