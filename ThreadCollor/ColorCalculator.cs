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
        //A reference to the current FileEntry
        private FileEntry entry;
        //The range of pixel's in the FileEntry that have to be calculated
        private Point range;
        
        //An event handler to tell the ThreadManager there is no work left to be done
        public event DoneHandler Done;
        public delegate void DoneHandler(ColorCalculator c, EventArgs e);

        /// <summary>
        /// Constructor of the ColorCalculater
        /// </summary>
        /// <param name="listView_overview">The ListView in which progress will be writen</param>
        /// <param name="fileManager">The FileManager from which tasks will be gotten</param>
        public ColorCalculator(ListView listView_overview, FileManager fileManager, int core)
        {
            //Set the FileManager
            this.fileManager = fileManager;
            //Set the ListView
            this.listView_overview = listView_overview;

            //Enable progress reporting
            WorkerReportsProgress = true;
            
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
            KeyValuePair<FileEntry, Point> kvp = fileManager.getTask();
            
            //If botht the FileEntry and the range have been set
            if(kvp.Key != null && kvp.Value != Point.Empty)
            {
                //Save them
                entry = kvp.Key;
                range = kvp.Value;

                //Create variables to store the avg colors
                int avgRed = -1,
                    avgGreen = -1,
                    avgBlue = -1;
            
                //Try incase entry is not a valid Bitmap
                try
                {
                    //Load the image into memory
                    Bitmap image = new Bitmap(entry.getFilePath());

                    //Calculate the number of pixels in the image
                    int numberOfPixels = image.Height * image.Width;

                    //Set the progress to 0
                    ReportProgress(0);

                    //For every row of pixels
                    for (int y = 0; y < image.Height; y++)
                    {
                        //For every colum of pixels
                        for (int x = 0; x < image.Width; x++)
                        {
                            //Get the pixel
                            Color pixel = image.GetPixel(x, y);

                            //Add the colors to the average
                            avgRed += pixel.R;
                            avgGreen += pixel.G;
                            avgBlue += pixel.B;
                        }

                        //Every 25th row report progress
                        if (y % 25 == 0)
                        {
                            //Calculate the progress
                            double progress = (y * image.Width / (double)numberOfPixels) * 100;
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
                listView_overview.Items[entry.getEntryNumber()].SubItems[3].Text = e.ProgressPercentage.ToString() + "%";
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
                //Get a local reference to SubItems
                System.Windows.Forms.ListViewItem.ListViewSubItemCollection subItems = listView_overview.Items[entry.getEntryNumber()].SubItems;
                //Set the entry status to finished
                entry.setStatus("Finished");
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

            ////If there is still work left to be done
            if(fileManager.FilesWaiting> 0)
            {
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
    }
}