/**
 *  Author:         János de Vries
 *  Date:           Sep. 2014
 *  Student Number: 208418
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadCollor
{
    /// <summary>
    /// This class is used to manage all the FileEntries (images)
    /// </summary>
    class FileManager
    {
        //A list of all the files contained in the FileManager
        private List<FileEntry> files = new List<FileEntry>();
        //A queue containing all the work that has to be done
        private Queue<KeyValuePair<FileEntry, Point>> tasklist = new Queue<KeyValuePair<FileEntry, Point>>();

        //A flag that when set stops the FileManager from handing out new tasks
        //Which in turn will stop all the workers because there is no more work
        private bool stopflag = false;

        //Current sort, used to toggle between ascending and descending
        private string curSort = "none";

        //The number of threads on one image
        private int threadsPerImage = 1;

        //The number of tasks or files waiting
        private double filesWaiting = 0;

        /// <summary>
        /// A public reference to Count
        /// </summary>
        public int Count
        {
            //Return the number of files in the FileManager
            get{ return files.Count; }
        }

        /// <summary>
        /// Public reference to the queue length
        /// </summary>
        public double FilesWaiting
        {
            get
            {
                //If the stop flag is not set
                if(!stopflag || tasklist.Count%threadsPerImage > 0)
                {
                    //Return the actual number of tasks
                    return Math.Ceiling(Math.Round(filesWaiting, 1));
                }
                //If the stop flag has been set
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// Returns the FileEntry at index i
        /// </summary>
        /// <param name="i">The index of the FileEntry requested</param>
        /// <returns>The i'th FileEntry</returns>
        public FileEntry this[int i]
        {
            get{ return files[i]; }
        }


        /// <summary>
        /// Add a single FileEntry to the FileManager
        /// </summary>
        /// <param name="fileName">The name of the file being added</param>
        /// <param name="filePath">The path of the file being added</param>
        private void add(string fileName, string filePath)
        {
            //Local reference to the entry
            FileEntry entry = new FileEntry(fileName, filePath, threadsPerImage);
            //Add the entry to the files list
            files.Add(entry);

            filesWaiting++;
        }
        
        /// <summary>
        /// Add a list of file paths to the FileManager
        /// </summary>
        /// <param name="selectedFiles">A list of FilePaths + FileNames</param>
        public void add(List<string> selectedFiles)
        {
            foreach(string filepath in selectedFiles)
            {
                //Try to get the FileName
                try
                {
                    //Add the file to the FileManager
                    this.add(System.IO.Path.GetFileName(filepath), filepath);
                }
                catch(ArgumentException)
                {} //Not a valid path don't add
            }
        }

        /// <summary>
        /// Returns the files list containing all known FileEntries
        /// </summary>
        /// <returns>A list of all FileEntries</returns>
        public List<FileEntry> getFiles()
        {
            return files;
        }

            /// <summary>
        /// This method returns a task, which can be completed by workers
        /// </summary>
        /// <returns>The task that has to be completed</returns>
        public KeyValuePair<FileEntry, Point> getTask()
        {
            //If there are any files waiting and (the FileManager is allowed to hand out tasks or has unfinished tasks)
            if(FilesWaiting > 0 && (!stopflag || tasklist.Count%threadsPerImage > 0))
            {
                //Decrease files waiting
                filesWaiting -= (1/(double)threadsPerImage);
                //Return a task
                return tasklist.Dequeue();
            }
            //If there are no tasks or the FileManager isn't allowed to release tasks
            else
            {
                //Return an empty task
                return new KeyValuePair<FileEntry, Point>(null, Point.Empty);
            }
        }

        /// <summary>
        /// Create the taskList
        /// </summary>
        public void createQueue()
        {
            //Clear the old taskList
            tasklist.Clear();
            //For each entry in files
            foreach(FileEntry entry in files)
            {
                //If the status is waiting
                if(entry.getStatus() == "Waiting")
                {
                    //Grab the image height
                    int entryHeight = entry.Height;
                    //For each thread per image
                    for(int i = 0; i<threadsPerImage; i++)
                    {
                        //Divide the image height over the number of threads allowed on each image
                        //
                        //Example:
                        //An image has a height 100px, threasPerImage = 4;
                        //This will generate the points & tasks, (0,24);(25,49);(51,74);(74,99)
                        Point range = new Point((entryHeight/threadsPerImage)*i, ((entryHeight/threadsPerImage)*(i+1)-1));
                        //Add the task to the queue
                        tasklist.Enqueue(new KeyValuePair<FileEntry, Point>(entry, range));
                    }
                }
            }
        }


        /// <summary>
        /// Sets the number of threads per image
        /// </summary>
        /// <param name="threadsPerImage">the number of threads per image</param>
        public void setThreadsPerImage(int threadsPerImage)
        {
            //Save the new value
            this.threadsPerImage = threadsPerImage;

            //Send the new threads per image value to each entry
            foreach(FileEntry entry in files)
            {
                entry.setThreadsPerImage(threadsPerImage);
            }
        }

        /// <summary>
        /// Remove a FileEntry at index i
        /// </summary>
        /// <param name="i">The index at which the FileEntry will be removed</param>
        public void removeAt(int i)
        {
            //Get the file's status
            string entryStatus = files[i].getStatus();
            if(entryStatus == "Waiting")
            {
                //Decrease files waiting
                filesWaiting --;
            }
            //If the status length is under 4 it's not "Waiting" or "Finished"
            else if(entryStatus.Length < 4)
            {
                //Calculate the percentage
                double statusPercent = Convert.ToDouble(entryStatus.Remove(entryStatus.Length - 1));
                //Reduce files waiting by 1*statusPercent
                filesWaiting -= 1-(statusPercent / 100);
            }
            
            //Remove the file located at i
            files.RemoveAt(i);
        }


        /// <summary>
        /// Sets the stop flag to true, which causes the FileManager to no longer hand out tasks
        /// </summary>
        public void setStopFlag()
        {
            stopflag = true;
        }


        /// <summary>
        /// Releases the StopFlag so the FileManager will hand out tasks again
        /// </summary>
        public void releaseStopFlag()
        {
            stopflag = false;
        }


        /// <summary>
        /// Get the total number of pixels calculated within all images
        /// </summary>
        /// <returns>The total number of pixels calculated</returns>
        public long getTotalPixelsDone()
        {
            long pixels = 0;

            //For each FileEntry
            foreach(FileEntry entry in files)
            {
                //Get the progress and add it to the total
                pixels += entry.getProgress();
            }

            return pixels;
        }


        /// <summary>
        /// Sort the files within the FileManager, ascending and descending is toggled
        /// </summary>
        /// <param name="Type">The property on which to sort, for possible settings see code</param>
        public void sort(string Type)
        {
            //A case switch which determines which sort method needs to be called
            switch(Type)
            {
                case "Filename":
                {
                    if(curSort == "FilenameA")
                    {
                        curSort = "FilenameD";
                        files.Sort((x, y) => -1*String.Compare(x.getFileName(), y.getFileName()));
                    }
                    else
                    {
                        curSort = "FilenameA";
                        files.Sort((x, y) => String.Compare(x.getFileName(), y.getFileName()));
                    }
                    break;
                }
                case "File path":
                {
                    if(curSort == "FilePathA")
                    {
                        curSort = "FilePathD";
                        files.Sort((x, y) => -1*String.Compare(x.getFilePath(), y.getFilePath()));
                    }
                    else
                    {
                        curSort = "FilePathA";
                        files.Sort((x, y) => String.Compare(x.getFilePath(), y.getFilePath()));
                    }
                    break;
                }
                case "Size":
                {
                    if(curSort == "FileSizeA")
                    {
                        curSort = "FileSizeD";
                        files.Sort((x, y) => -1*x.getByteLenght().CompareTo(y.getByteLenght()));
                    }
                    else
                    {
                        curSort = "FileSizeA";
                        files.Sort((x, y) => x.getByteLenght().CompareTo(y.getByteLenght()));
                    }
                    break;
                }
                case "Status":
                {
                    if(curSort == "StatusA")
                    {
                        curSort = "StatusD";
                        files.Sort((x, y) => -1*String.Compare(x.getStatus(), y.getStatus()));
                    }
                    else
                    {
                        curSort = "StatusA";
                        files.Sort((x, y) => String.Compare(x.getStatus(), y.getStatus()));
                    }
                    break;
                }
                case "Red":
                {
                    if(curSort == "RedA")
                    {
                        curSort = "RedD";
                        
                        files.Sort((x, y) => -1*Convert.ToInt32(x.getRed()).CompareTo(Convert.ToInt32(y.getRed())));
                    }
                    else
                    {
                        curSort = "RedA";
                        files.Sort((x, y) => Convert.ToInt32(x.getRed()).CompareTo(Convert.ToInt32(y.getRed())));
                    }
                    break;
                }
                case "Green":
                {
                    if(curSort == "GreenA")
                    {
                        curSort = "GreenD";
                        files.Sort((x, y) => -1 * Convert.ToInt32(x.getGreen()).CompareTo(Convert.ToInt32(y.getGreen())));
                    }
                    else
                    {
                        curSort = "GreenA";
                        files.Sort((x, y) => Convert.ToInt32(x.getGreen()).CompareTo(Convert.ToInt32(y.getGreen())));
                    }
                    break;
                }
                case "Blue":
                {
                    if(curSort == "BlueA")
                    {
                        curSort = "BlueD";
                        files.Sort((x, y) => -1 * Convert.ToInt32(x.getBlue()).CompareTo(Convert.ToInt32(y.getBlue())));
                    }
                    else
                    {
                        curSort = "BlueA";
                        files.Sort((x, y) => Convert.ToInt32(x.getBlue()).CompareTo(Convert.ToInt32(y.getBlue())));
                    }
                    break;
                }
                case "Hex":
                case "Color":
                {
                    if(curSort == "HexA")
                    {
                        curSort = "HexD";
                        files.Sort((x, y) => -1*String.Compare(x.getHex(), y.getHex()));
                    }
                    else
                    {
                        curSort = "HexA";
                        files.Sort((x, y) => String.Compare(x.getHex(), y.getHex()));
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }
}