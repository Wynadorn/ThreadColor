using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadCollor
{
    class FileEntry
    {
        private int entryNumber = -1;
        private string fileName;
        private string filePath;
        private string status = "Waiting";
        private int red = -1;
        private int green = -1;
        private int blue = -1;
        private string hex = String.Empty;

        public FileEntry(string fileName, string filePath)
        {
            this.fileName = fileName;
            this.filePath = filePath;
        }

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
        #endregion

        #region Set
        public void setEntryNumber(int entryNumber)
        {
            this.entryNumber = entryNumber;
        }

        public void setStatus(string status)
        {
            //Update the status if the new status is Waiting or Finished
            if(status == "Waiting" || status == "Finished")
            {
                this.status = status;
                return;
            }
            //If it's not see if it's an percentage (50%)
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
            }
        }

        public void setGreen(int green)
        {
            if (green >= 0 && green <= 255)
            {
                this.green = green;
            }
        }

        public void setBlue(int blue)
        {
            if (blue >= 0 && blue <= 255)
            {
                this.blue = blue;
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
    }
}