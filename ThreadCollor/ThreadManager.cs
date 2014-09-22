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
    /// <summary>
    /// This class has control over all the worker threads
    /// </summary>
    class ThreadManager
    {
        //Create an array of ColorCalculators (workers)
        private ColorCalculator[] workers;
        //A variable that keeps track of the number of workers running
        private int tasksRunning = 0;
        //A flag to provide information if threads are running
        private bool threadsRunning = false;
        //A public reference to threads running
        public bool ThreadsRunning
        {
            get { return threadsRunning; } 
        }

        //An event handler to tell the MainForm all the threads have finished working
        public event AllThreadsDone allThreadsDone;
        public delegate void AllThreadsDone();
        
        public void startThreads(FileManager fileManager, ListView listView_overview, int numberOfThreads, int numberOfCores)
        {
            //Give the workers array a lenght
            workers = new ColorCalculator[numberOfThreads];

            //Set threads running to true
            threadsRunning = true;

            //For every worker in workers
            for (int i = 0; i < workers.Length; i++)
            {
                //Fill the workers array with ColorCalculators
                workers[i] = new ColorCalculator(listView_overview, fileManager, i%numberOfCores);
                //Add a listener that's called when the thread is done working
                workers[i].Done += new ColorCalculator.DoneHandler(threadFinished);

                //Start the worker
                workers[i].RunWorkerAsync();
                //Iterate the number of running workers
                tasksRunning++;
            }
        }

        /// <summary>
        /// When all the workers are finished working let the listeners (MainForm) know
        /// </summary>
        public void threadFinished(object sender, EventArgs e)
        {
            tasksRunning--;
            //If there are no tasks working they're all done
            if(tasksRunning <= 0)
            {
                //Set threadsRunning to false
                threadsRunning = false;

                //Signal the event
                allThreadsDone();
            }
        }
    }
}