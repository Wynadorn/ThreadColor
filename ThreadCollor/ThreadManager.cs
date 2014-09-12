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
        Queue<FileEntry> _taskList;
        ColorCalculator[] workers;
        ListView listView_overview;
        int tasksRunning = 0;

        //An event handler to tell the form all the threads have finished working
        public event AllThreadsDone allThreadsDone;
        public delegate void AllThreadsDone();

        public Queue<FileEntry> taskList
        {
            get { return _taskList; }
            set
            {
                _taskList = value;
                if(_taskList.Count <= 0)
                {
                    MessageBox.Show("all done");
                }
            }
        }
        
        //public ThreadManager()
        //{}

        public void setListView(ListView listView_overview)
        {
            this.listView_overview = listView_overview;
        }

        public void startThreads(Queue<FileEntry> taskList, int numberOfThreads)
        {
            //Create a local copy of the task list
            this._taskList = taskList;
            
            //Give the workers array a lenght
            workers = new ColorCalculator[numberOfThreads];

            //Fill the workers array with ColorCalculators
            for (int i = 0; i < workers.Length; i++)
            {
                //
                workers[i] = new ColorCalculator(listView_overview, this.taskList);
                workers[i].Done += new ColorCalculator.DoneHandler(threadFinished);
            }

            //Set all the workers to work
            foreach(ColorCalculator worker in workers)
            {
                worker.RunWorkerAsync();
                //Keep track of the number of tasks running
                tasksRunning++;
            }
        }

        public void threadFinished(object sender, EventArgs e)
        {
            tasksRunning--;
            //If there are no tasks working they're all done
            if(tasksRunning <= 0)
            {
                allThreadsDone();
            }
        }
    }
}