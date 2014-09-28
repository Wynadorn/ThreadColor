/**
 *  Author:         János de Vries
 *  Date:           Sep. 2014
 *  Student Number: 208418
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;

namespace ThreadCollor
{
    /// <summary>
    /// This class is used to calculate the RGB values of each task and report it back to a FileEntry
    /// </summary>
    class ColorCalculator : BackgroundWorker
    {
        //A reference to the ListView
        private ListView listView_overview;
        //A reference to the FileManager
        private FileManager fileManager;
        //
        KeyValuePair<FileEntry, Point> task;
        //A reference to the current FileEntry
        private FileEntry entry;
        //The range of pixel's in the FileEntry that have to be calculated (in height)
        private Point range;
        //A lock for the getTask() method
        private object taskLock;
        
        //An event handler to tell the ThreadManager there is no work left to be done
        public event DoneHandler Done;
        public delegate void DoneHandler(ColorCalculator c, EventArgs e);

        /// <summary>
        /// Constructor of the ColorCalculater
        /// </summary>
        /// <param name="listView_overview">The ListView in which progress will be written</param>
        /// <param name="fileManager">The FileManager from which tasks will be gotten</param>
        public ColorCalculator(ListView listView_overview, FileManager fileManager, int core, object taskLock)
        {
            //Set the FileManager
            this.fileManager = fileManager;
            //Set the ListView
            this.listView_overview = listView_overview;
            //Set the taskLock
            this.taskLock = taskLock;

            //Enable progress reporting
            WorkerReportsProgress = true;

            Monitor.Enter(taskLock);
            try
            {
                if (fileManager.FilesWaiting > 0)
                {
                    task = fileManager.getTask();
                }
            }
            finally { Monitor.Exit(taskLock); }

            ////Support Cancellation
            //WorkerSupportsCancellation = true;
        }

        /// <summary>
        /// The method that runs when the thread has started
        /// </summary>
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            //If a FileEntry has been set
            if(task.Key != null)
            {
                //Variable for the Bitmap image
                Bitmap image;

                //Save the entry and ranged
                entry = task.Key;
                range = task.Value;

                //Create variables to store the average colors
                double avgRed = -1,
                       avgGreen = -1,
                       avgBlue = -1;
            
                try
                {
                    //Try to load the image
                    image = new Bitmap(entry.getFilePath());
                }
                catch(System.ArgumentException) //not an image
                {
                    entry.setStatus("Finished");
                    return;
                }

                //Create an instance of the LockBitmap Class
                LockBitmap lbm = new LockBitmap(image);
                //Lock the image into memory
                lbm.LockBits();

                //If the pixel range is empty
                if (range == Point.Empty)
                {
                    //Calculate all rows within the image
                    range = new Point(0, image.Height);
                }

                //Calculate the number of pixels in the image
                int numberOfPixels = (range.Y - range.X) * image.Width;
                //Set the number of rows calculated to 0
                int rowsDone = 0;
                //Set the progress to 0
                ReportProgress(0);

                //For every row of pixels
                for (int y = range.X; y < range.Y; y++)
                {
                    //For every column of pixels
                    for (int x = 0; x < image.Width; x++)
                    {
                        //Get the pixel data
                        Color pixel = lbm.GetPixel(x, y);

                        //Add the colors to the average
                        avgRed += pixel.R;
                        avgGreen += pixel.G;
                        avgBlue += pixel.B;
                    }

                    //Increase the rows done
                    rowsDone++;

                    //Every 50th row report progress
                    if (y % 50 == 0 || y == range.Y - 1)
                    {
                        //Calculate the progress
                        int progress = rowsDone * image.Width;
                        //Add the progress to the entry
                        entry.addProgress(progress);
                        //Reset the rows calculated back to 0
                        rowsDone = 0;
                        //Report the entry's progress to the ListView
                        ReportProgress((int)progress);
                    }
                }

                //Calculate the average values and send them to the FileEntry
                if (avgRed != -1 && avgGreen != -1 && avgBlue != -1)
                {
                    entry.setRed((avgRed + 1) / numberOfPixels);
                    entry.setGreen((avgGreen + 1) / numberOfPixels);
                    entry.setBlue((avgBlue + 1) / numberOfPixels);
                }
            }
        }

        /// <summary>
        /// This method is called when the progress has been changed
        /// </summary>
        protected override void OnProgressChanged(ProgressChangedEventArgs e)
        {
            //If there is an active FileEntry
            if(entry != null)
            {
                //Write the progress to the ListView
                listView_overview.Items[entry.getEntryNumber()].SubItems[3].Text = entry.getStatus();
            }
        }

        /// <summary>
        /// This method is called when the thread reaches the end of OnDoWork()
        /// </summary>
        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            //If the thread has a current FileEntry
            if(entry != null)
            {
                //If it's status is Finished
                if(entry.getStatus() == "Finished")
                {
                    //Get a local reference to SubItems
                    System.Windows.Forms.ListViewItem.ListViewSubItemCollection subItems = listView_overview.Items[entry.getEntryNumber()].SubItems;
                    //Set all the information in the ListView
                    subItems[3].Text = "Finished";
                    subItems[4].Text = entry.getRed();
                    subItems[5].Text = entry.getGreen();
                    subItems[6].Text = entry.getBlue();
                
                    //Grab the hex value from the entry
                    string hexValue = entry.getHex();
                    subItems[7].Text = hexValue;
                    //If the hex value is not null (visually represented by "-")
                    if (hexValue != "-")
                    {
                        //Color the background of the cell
                        subItems[8].BackColor = ColorTranslator.FromHtml("#" + hexValue);
                        //Remove the text placeholder
                        subItems[8].Text = String.Empty;
                    }
                
                    //Set the entry to null
                    entry = null;
                }
            }

            //Enter the critical section, lock the lock
            Monitor.Enter(this.taskLock);
            try
            {
                //If there is still work left to be done
                if (fileManager.FilesWaiting > 0)
                {
                    //Grab a new task
                    task = fileManager.getTask();
                    //Start again
                    this.RunWorkerAsync();
                }
                //Else all the work is done
                else
                {
                    //Report to the thread manager this thread has nothing left to do
                    Done(this, e);
                }
            }
            finally { Monitor.Exit(taskLock); } //Leave the critical section
        }
    }
}