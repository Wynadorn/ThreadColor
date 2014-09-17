﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;

namespace ThreadCollor
{
    class ColorCalculator : BackgroundWorker
    {
        private ListView listView_overview;
        private Dictionary<string, int> result;
        private FileManager fileManager;
        
        FileEntry entry;
        Point range;

        //An event handler to tell the thread manager there is no work left to do
        public event DoneHandler Done;
        public delegate void DoneHandler(ColorCalculator c, EventArgs e);

        public ColorCalculator(ListView listView_overview, FileManager fileManager)
        {
            this.fileManager = fileManager;
            this.listView_overview = listView_overview;
            result = new Dictionary<string,int>();

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            KeyValuePair<FileEntry, Point> kvp = fileManager.getTask();
            
            if(kvp.Key != null && kvp.Value != Point.Empty)
            {
                entry = kvp.Key;
                range = kvp.Value;

                int avgRed = -1,
                    avgGreen = -1,
                    avgBlue = -1;
            
                //Grab task from queue

                //Load the image into memory
                try
                {
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

                        if (y % 25 == 0)
                        {
                            //Report progress after every row
                            double progress = (y * image.Width / (double)numberOfPixels) * 100;
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

        protected override void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if(entry != null)
            {
                listView_overview.Items[entry.getEntryNumber()].SubItems[2].Text = e.ProgressPercentage.ToString() + "%";
            }
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            if(entry != null)
            {
                System.Windows.Forms.ListViewItem.ListViewSubItemCollection subItems = listView_overview.Items[entry.getEntryNumber()].SubItems;
                entry.setStatus("Finished");
                subItems[2].Text = "Finished";
                subItems[3].Text = entry.getRed();
                subItems[4].Text = entry.getGreen();
                subItems[5].Text = entry.getBlue();
            
                string hexValue = entry.getHex();
                subItems[6].Text = hexValue;
                if (hexValue != "-")
                {
                    subItems[7].BackColor = ColorTranslator.FromHtml("#" + hexValue);
                    subItems[7].Text = String.Empty;
                }
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