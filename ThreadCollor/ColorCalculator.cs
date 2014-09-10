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
    class ColorCalculator : BackgroundWorker
    {
        ListView listView_overview;
        private Dictionary<string, int> result;

        public ColorCalculator(ListView listView_overview)
        {
            this.listView_overview = listView_overview;
            result = new Dictionary<string,int>();

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            int avgRed = -1,
                avgGreen = -1,
                avgBlue = -1,
                avgHex = -1;
            
            //Grab task from queue


            //For every pixel
            int numberOfPixels = 20736;

            for(int i=0; i<=numberOfPixels; i++)
            {
                int red = -1,
                    green = -1,
                    blue = -1;

                Console.WriteLine("what? {0}", i);

                //If all colors are set
                if (red != -1 && green != -1 && blue != -1)
                {
                    avgRed += red;
                    avgGreen += green;
                    avgBlue += blue;
                }

                //Report
                if(i%5000 == 0)
                {
                    double x = (i / (double)numberOfPixels) * 100;
                    ReportProgress((int)x);
                }
            }

            //Calculate the avg values
            if (avgRed != -1 && avgGreen != -1 && avgBlue != -1)
            {
                avgRed = avgRed / numberOfPixels;
                avgGreen = avgGreen / numberOfPixels;
                avgBlue = avgBlue / numberOfPixels;
                //Calculate the hex
            }


            //Store the results
            result.Add("Red", avgRed);
            result.Add("green", avgGreen);
            result.Add("blue", avgBlue);
            result.Add("hex", avgHex);
        }

        protected override void OnProgressChanged(ProgressChangedEventArgs e)
        {
            Console.WriteLine("{0}%", e.ProgressPercentage);
            listView_overview.Items[0].SubItems[1].Text = e.ProgressPercentage.ToString();
        }
    }
}