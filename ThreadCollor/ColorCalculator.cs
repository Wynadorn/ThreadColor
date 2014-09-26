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
        //The range of pixel's in the FileEntry that have to be calculated
        private Point range;
        //A lock for the getTask() method
        private object taskLock;
        
        //An event handler to tell the ThreadManager there is no work left to be done
        public event DoneHandler Done;
        public delegate void DoneHandler(ColorCalculator c, EventArgs e);

        /// <summary>
        /// Constructor of the ColorCalculater
        /// </summary>
        /// <param name="listView_overview">The ListView in which progress will be writen</param>
        /// <param name="fileManager">The FileManager from which tasks will be gotten</param>
        public ColorCalculator(ListView listView_overview, FileManager fileManager, int core, object taskLock)
        {
            //Set the FileManager
            this.fileManager = fileManager;
            //Set the ListView
            this.listView_overview = listView_overview;
            //
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

            ////Suport Cacellation
            //WorkerSupportsCancellation = true;
        }

        /// <summary>
        /// The method that runs when the thread has started
        /// </summary>
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            //Grab task from queue
            //Thread local reference to the task that has to be done
            // = fileManager.getTask();
            
            //If botht the FileEntry and the range have been set
            if(task.Key != null)
            {
                //Save them
                entry = task.Key;
                range = task.Value;

                //Create variables to store the avg colors
                double avgRed = -1,
                       avgGreen = -1,
                       avgBlue = -1;
            
                //Try incase entry is not a valid Bitmap
                try
                {
                    //Load the image into memory
                    Bitmap image = new Bitmap(entry.getFilePath());
                    
                    LockBitmap lbm = new LockBitmap(image);
                    lbm.LockBits();

                    if(range == Point.Empty)
                    {
                        range = new Point(0, image.Height);
                    }

                    //Calculate the number of pixels in the image
                    int numberOfPixels = (range.Y-range.X) * image.Width;
                    int rowsDone = 0;

                    //Set the progress to 0
                    ReportProgress(0);

                    //For every row of pixels
                    for (int y = range.X; y < range.Y; y++)
                    {
                        //For every colum of pixels
                        for (int x = 0; x < image.Width; x++)
                        {
                            //Get the pixel
                            Color pixel = lbm.GetPixel(x, y);

                            //Add the colors to the average
                            avgRed += pixel.R;
                            avgGreen += pixel.G;
                            avgBlue += pixel.B;
                        }

                        rowsDone++;
                        //Every 25th row report progress
                        if (y % 50 == 0 || y==range.Y-1)
                        {
                            //Calculate the progress
                            int progress = rowsDone * image.Width;
                            entry.addProgress(progress);
                            rowsDone = 0;
                            //Report it
                            ReportProgress((int)progress);
                        }
                    }

                    //Calculate the avg values
                    if (avgRed != -1 && avgGreen != -1 && avgBlue != -1)
                    {
                        entry.setRed((avgRed + 1) / numberOfPixels);
                        entry.setGreen((avgGreen + 1) / numberOfPixels);
                        entry.setBlue((avgBlue + 1) / numberOfPixels);
                    }
                }
                catch(System.ArgumentException) //not an image, do nothing
                {}
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

            Monitor.Enter(this.taskLock);
            try
            {
                ///If there is still work left to be done
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
            finally { Monitor.Exit(taskLock); }
        }
    }
}