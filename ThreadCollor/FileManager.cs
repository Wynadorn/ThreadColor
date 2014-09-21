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
        private Queue<FileEntry> tasklist = new Queue<FileEntry>();

        //A flag that when set stops the FileManager fram handing out new tasks
        //Which in turn will stop all the workers because there is no more work
        private bool stopflag = false;

        //Current sort, used to toggle between acending and decending
        private string curSort = "none";

        /// <summary>
        /// A public reference to Count
        /// </summary>
        public int Count
        {
            //Return the number of files in the FileManager
            get{ return files.Count; }
        }

        /// <summary>
        /// Public reference to the queue lenght
        /// </summary>
        public int FilesWaiting
        {
            get
            {
                //If the stop flag is not set
                if(!stopflag)
                {
                    //Return the actual number of tasks
                    return tasklist.Count;
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
            //set { InnerList[i] = value; }
        }

        /// <summary>
        /// Add a single FileEntry to the FileManager
        /// </summary>
        /// <param name="fileName">The name of the file being added</param>
        /// <param name="filePath">The path of the file being added</param>
        private void add(string fileName, string filePath)
        {
            //Local reference to the entry
            FileEntry entry = new FileEntry(fileName, filePath);
            //Add the entry to the files list
            files.Add(entry);
            //Add the entry to the queue, because new tasks are never compled
            tasklist.Enqueue(entry);
        }

        /// <summary>
        /// Add a list of filepaths to the FileManager
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
        /// Returns the files list cointaining all known FileEntries
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
            //If there are any files waiting and the filemanager is allowed to hand out tasks
            if(FilesWaiting > 0 && !stopflag)
            {
                //Remove one task from the queue
                FileEntry entry = tasklist.Dequeue();
                //Select a pixel range to complete
                Point range = new Point(50,100);

                //Return the value
                return new KeyValuePair<FileEntry, Point>(entry, range);
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
        private void createQueue()
        {
            tasklist.Clear();
            //For each entry in files
            foreach(FileEntry entry in files)
            {
                //If the status is wating
                if(entry.getStatus() == "Waiting")
                {
                    //Add it to the queue
                    tasklist.Enqueue(entry);
                }
            }
        }

        /// <summary>
        /// Remove  a FileEntry at index i
        /// </summary>
        /// <param name="i">The index at which the FileEntry will be removed</param>
        public void removeAt(int i)
        {
            //Remove the file at i
            files.RemoveAt(i);
            //Delete the tasklist
            tasklist.Clear();
            createQueue();
        }

        /// <summary>
        /// Sets the stop flag to true, which causes the FileManager to nolonger hand out tasks
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

        public void sort(string Type)
        {
            //A case switch which determains which sort method needs to be called
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
                        files.Sort((x, y) => -1*String.Compare(x.getFileSize(), y.getFileSize()));
                    }
                    else
                    {
                        curSort = "FileSizeA";
                        files.Sort((x, y) => String.Compare(x.getFileSize(), y.getFileSize()));
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
                        files.Sort((x, y) => -1*String.Compare(x.getRed(), y.getRed()));
                    }
                    else
                    {
                        curSort = "RedA";
                        files.Sort((x, y) => String.Compare(x.getRed(), y.getRed()));
                    }
                    break;
                }
                case "Green":
                {
                    if(curSort == "GreenA")
                    {
                        curSort = "GreenD";
                        files.Sort((x, y) => -1*String.Compare(x.getGreen(), y.getGreen()));
                    }
                    else
                    {
                        curSort = "GreenA";
                        files.Sort((x, y) => String.Compare(x.getGreen(), y.getGreen()));
                    }
                    break;
                }
                case "Blue":
                {
                    if(curSort == "BlueA")
                    {
                        curSort = "BlueD";
                        files.Sort((x, y) => -1*String.Compare(x.getBlue(), y.getBlue()));
                    }
                    else
                    {
                        curSort = "BlueA";
                        files.Sort((x, y) => String.Compare(x.getBlue(), y.getBlue()));
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
            createQueue();
        }
    }
}

///**
// *  Region that contains all the methods used for sorting the files 
//**/
//#region Sorting
//    /// <summary>
//    /// Sorts the files by name
//    /// </summary>
//    /// <param name="acending">A bool if the list has to be storted aceding or decending</param>
//    public void sortFileName(bool acending = true)
//    {
//        //Do sorting
//    }

//    /// <summary>
//    /// Sorts the files by filepath
//    /// </summary>
//    /// <param name="acending">A bool if the list has to be storted aceding or decending</param>
//    public void sortFilePath(bool acending = true)
//    {

//    }

//    /// <summary>
//    /// Sorts the files by size
//    /// </summary>
//    /// <param name="acending">A bool if the list has to be storted aceding or decending</param>
//    public void sortSize(bool acending = true)
//    {

//    }

//    /// <summary>
//    /// Sorts the files by status
//    /// </summary>
//    /// <param name="acending">A bool if the list has to be storted aceding or decending</param>
//    public void sortStatus(bool acending = true)
//    {

//    }

//    /// <summary>
//    /// Sorts the files by red RGB value
//    /// </summary>
//    /// <param name="acending">A bool if the list has to be storted aceding or decending</param>
//    public void sortRed(bool acending = true)
//    {

//    }

//    /// <summary>
//    /// Sorts the files by green RGB value
//    /// </summary>
//    /// <param name="acending">A bool if the list has to be storted aceding or decending</param>
//    public void sortGreen(bool acending = true)
//    {

//    }

//    /// <summary>
//    /// Sorts the files by blue RGB value
//    /// </summary>
//    /// <param name="acending">A bool if the list has to be storted aceding or decending</param>
//    public void sortBlue(bool acending = true)
//    {

//    }

//    /// <summary>
//    /// Sorts the files by hex value
//    /// </summary>
//    /// <param name="acending">A bool if the list has to be storted aceding or decending</param>
//    public void sortHex(bool acending = true)
//    {

//    }
//#endregion