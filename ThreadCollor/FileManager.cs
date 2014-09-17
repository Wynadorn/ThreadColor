using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadCollor
{
    class FileManager
    {
        private List<FileEntry> files = new List<FileEntry>();
        private Queue<FileEntry> tasklist = new Queue<FileEntry>();
        private bool stopflag = false;

        public int Count
        {
            get{ return files.Count; }
        }

        public int FilesWaiting
        {
            get
            {
                if(!stopflag)
                {
                    return tasklist.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        public FileEntry this[int i]
        {
            get{ return files[i]; }
            //set { InnerList[i] = value; }
        }

        public void add(string fileName, string filePath)
        {
            FileEntry entry = new FileEntry(fileName, filePath);
            files.Add(entry);
            tasklist.Enqueue(entry);
        }

        public List<FileEntry> getFiles()
        {
            return files;
        }

        public void sortFileName(bool acending = true)
        {
            //Do sorting
        }

        public void sortFilePath(bool acending = true)
        {

        }

        public void sortStatus(bool acending = true)
        {

        }

        public void sortRed(bool acending = true)
        {

        }

        public void sortGreen(bool acending = true)
        {

        }

        public void sortBlue(bool acending = true)
        {

        }

        public KeyValuePair<FileEntry, Point> getTask()
        {
            if(FilesWaiting > 0 && !stopflag)
            {
                FileEntry entry = tasklist.Dequeue();
                Point range = new Point(50,100);

                return new KeyValuePair<FileEntry, Point>(entry, range);
            }
            else
            {
                return new KeyValuePair<FileEntry, Point>(null, Point.Empty);
            }
        }

        public void removeAt(int i)
        {
            files.RemoveAt(i);
        }

        public void setStopFlag()
        {
            stopflag = true;
        }

        public void releaseStopFlag()
        {
            stopflag = false;
        }
    }
}
