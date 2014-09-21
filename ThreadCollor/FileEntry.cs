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
        private int red = -1;
        private int green = -1;
        private int blue = -1;
        private string hex = String.Empty;
        private long fileSize = -1;

        /// <summary>
        /// Constructor of the FileEntry class
        /// </summary>
        /// <param name="fileName">The fileName with extention (image name)</param>
        /// <param name="filePath">The path to the file</param>
        public FileEntry(string fileName, string filePath)
        {
            //Save both values
            this.fileName = fileName;
            this.filePath = filePath;
            FileInfo f = new FileInfo(filePath);
            this.fileSize = f.Length;
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
            return status;
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
                return red.ToString();
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
                return green.ToString();
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
                return blue.ToString();
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
            //If it's not see if it's an percentage between 0 and 100
            else
            {
                try
                {
                    //Convert the string percentage to an int if possible.
                    int statusPercent = Convert.ToInt32(status.Remove(status.Length - 1));

                    //If it's a valid number
                    if (statusPercent >= 0 && statusPercent <= 100)
                    {
                        //update it
                        this.status = status;
                        return;
                    }
                }
                //Do nothing if it isn't a valid number
                catch(FormatException){}
            }
        }

        public void setRed(int red)
        {
            if(red >= 0 && red <= 255)
            { 
                this.red = red;
                //Try to calculate the hex value
                setHex();
            }
        }

        public void setGreen(int green)
        {
            if (green >= 0 && green <= 255)
            {
                this.green = green;
                //Try to calculate the hex value
                setHex();
            }
        }

        public void setBlue(int blue)
        {
            if (blue >= 0 && blue <= 255)
            {
                this.blue = blue;
                //Try to calculate the hex value
                setHex();
            }
        }

        private void setHex()
        {
            if (red != -1 && green != -1 && blue != -1)
            {
                Color myColor = Color.FromArgb(red, green, blue);

                this.hex = myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");
            }
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