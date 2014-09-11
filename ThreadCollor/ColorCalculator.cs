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
        private Queue<FileEntry> taskList;
        FileEntry entry;

        public ColorCalculator(ListView listView_overview, Queue<FileEntry> taskList)
        {
            this.taskList = taskList;
            this.listView_overview = listView_overview;
            result = new Dictionary<string,int>();

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            int avgRed = -1,
                avgGreen = -1,
                avgBlue = -1;
            
            //Grab task from queue
            entry = taskList.Dequeue();

            //Load the image into memory
            Bitmap image = new Bitmap(entry.getFilePath());

            //Calculate the number of pixels in the image
            int numberOfPixels = image.Height*image.Width;

            //BitmapData imageData = image.LockBits(new Rectangle(0, 0, 10, 10), System.Drawing.Imaging.ImageLockMode.ReadOnly, image.PixelFormat);

            //Set the progress to 0
            ReportProgress(0);

            //For every row of pixels
            for(int y=0; y<image.Height; y++)
            {
                //For every colum of pixels
                for(int x=0; x<image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);

                    avgRed += pixel.R;
                    avgGreen += pixel.G;
                    avgBlue += pixel.B;
                }

                //Report progress after every row
                double progress = (y*image.Width / (double)numberOfPixels) * 100;
                ReportProgress((int)progress);
            }

            //Calculate the avg values
            if (avgRed != -1 && avgGreen != -1 && avgBlue != -1)
            {
                entry.setRed((avgRed+1) / numberOfPixels);
                entry.setGreen((avgGreen+1) / numberOfPixels);
                entry.setBlue((avgBlue+1) / numberOfPixels);
                //Calculate the hex
            }


            //Store the results
            //result.Add("Red", avgRed);
            //result.Add("green", avgGreen);
            //result.Add("blue", avgBlue);
            //result.Add("hex", avgHex);
        }

        protected override void OnProgressChanged(ProgressChangedEventArgs e)
        {
            Console.WriteLine("{0}%", e.ProgressPercentage);
            listView_overview.Items[entry.getEntryNumber()].SubItems[2].Text = e.ProgressPercentage.ToString()+"%";
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            System.Windows.Forms.ListViewItem.ListViewSubItemCollection subItems = listView_overview.Items[entry.getEntryNumber()].SubItems;
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
            //listView_overview.Items[entry.getEntryNumber()].SubItems[7].Text = entry.getHex();

            //MessageBox.Show("done");
        }
    }
}