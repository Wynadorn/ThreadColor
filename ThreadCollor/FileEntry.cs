using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadCollor
{
    class FileEntry
    {
        //The FileEntry's location in the ListView
        private int entryNumber = -1;
        //Var for Filename
        private string fileName;
        //Var for FilePath
        private string filePath;
        //Status of completion
        private string status = "Waiting";
        //Variable for the color and hex values
        private double red = -1;
        private double green = -1;
        private double blue = -1;
        private int greenAdditions = 0;
        private int redAdditions = 0;
        private int blueAdditions = 0;
        private string hex = String.Empty;
        private long fileSize = -1;
        private int threadsPerImage;
        private int height;
        private int width;
        private int progress;
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// Constructor of the FileEntry class
        /// </summary>
        /// <param name="fileName">The fileName with extention (image name)</param>
        /// <param name="filePath">The path to the file</param>
        public FileEntry(string fileName, string filePath, int threadsPerImage = 1)
        {
            //Save both values
            this.fileName = fileName;
            this.filePath = filePath;
            FileInfo f = new FileInfo(filePath);
            this.fileSize = f.Length;

            //Get the image height
            Bitmap image = new Bitmap(filePath);
            height = image.Height;
            width = image.Width;
        }

        /**
         *  Region containing all the get method from the FileEntry
        **/
        #region Get
        public int getEntryNumber()
        {
            return entryNumber;
        }

        public string getFileName()
        {
            return fileName;
        }

        public string getFilePath()
        {
            return filePath;
        }

        public string getStatus()
        {
            if(status == "Waiting" || status == "Finished")
            {
                return status;
            }
            else
            {

                return status + "%";
            }
        }

        /// <summary>
        /// Returns the RGB value of red if it's known
        /// </summary>
        /// <returns>The RGB value of red</returns>
        public string getRed()
        {
            if(red == -1)
            {
                return "-";
            }
            else
            {
                return Math.Round(red).ToString();
            }
        }

        /// <summary>
        /// Returns the RGB value of green if it's known
        /// </summary>
        /// <returns>The RGB value of green</returns>
        public string getGreen()
        {
            if (green == -1)
            {
                return "-";
            }
            else
            {
                return Math.Round(green).ToString();
            }
        }

        /// <summary>
        /// Returns the RGB value of blue if it's known
        /// </summary>
        /// <returns>The RGB value of blue</returns>
        public string getBlue()
        {
            if (blue == -1)
            {
                return "-";
            }
            else
            {
                return Math.Round(blue).ToString();
            }
        }

        public string getHex()
        {
            if (hex == String.Empty)
            {
                return "-";
            }
            else
            {
                return hex;
            }
        }

        public string getFileSize()
        {
            return SizeSuffix(fileSize);
        }

        public long getByteLenght()
        {
            return fileSize;
        }
        #endregion

        /**
         *  Region containing all the Set method from the FileEntry
        **/
        #region Set
        public void setEntryNumber(int entryNumber)
        {
            this.entryNumber = entryNumber;
        }

        /// <summary>
        /// Method that sets the FileEntry's status
        /// </summary>
        /// <param name="status">The status to set, can be Waiting, Finished or int between 0 and 100</param>
        public void setStatus(string status)
        {
            //Update the status if the new status is Waiting or Finished
            if(status == "Waiting" || status == "Finished")
            {
                this.status = status;
                return;
            }
            ////If it's not see if it's an percentage between 0 and 100
            //else
            //{
            //    try
            //    {
            //        //Convert the string percentage to an int if possible.
            //        decimal statusPercent = Convert.ToDecimal(status.Remove(status.Length - 1));

            //        //If it's a valid number
            //        if (statusPercent >= 0 && statusPercent <= 100)
            //        {
            //            //update it
            //            this.status = statusPercent * (1/(decimal)threadsPerImage) + "%";
            //            return;
            //        }
            //    }
            //    //Do nothing if it isn't a valid number
            //    catch(FormatException){}
            //}
        }

        public void addProgress(int progress)
        {
            this.progress += progress;
            int percentage = (int)((this.progress / ((double)height * (double)width)) * (double)100);
            status = percentage.ToString();
        }

        public void setRed(double red)
        {
            if(red >= 0 && red <= 255)
            {
                redAdditions++;
                this.red = ((this.red * (1.0-1.0/redAdditions)) + (red *(1.0 / redAdditions)));
                //Try to calculate the hex value
                setHex();
            }
        }

        public void setGreen(double green)
        {
            if (green >= 0 && green <= 255)
            {
                greenAdditions++;
                this.green= ((this.green * (1.0 - 1.0 / greenAdditions)) + (green * (1.0 / greenAdditions)));
                //Try to calculate the hex value
                setHex();
            }
        }

        public void setBlue(double blue)
        {
            if (blue >= 0 && blue <= 255)
            {
                blueAdditions++;
                this.blue = ((this.blue * (1.0 - 1.0 / blueAdditions)) + (blue * (1.0 / blueAdditions)));
                //Try to calculate the hex value
                setHex();
            }
        }

        private void setHex()
        {
            if (red != -1 && green != -1 && blue != -1)
            {
                if(redAdditions == threadsPerImage || greenAdditions == threadsPerImage || blueAdditions == threadsPerImage)
                {
                    Color myColor = Color.FromArgb((int)red, (int)green, (int)blue);

                    this.hex = myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");

                    setStatus("Finished");
                }
            }
        }

        public void setThreadsPerImage(int threadsPerImage)
        {
            this.threadsPerImage = threadsPerImage;
        }
        #endregion

        //http://stackoverflow.com/a/14488941/4022492
        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}