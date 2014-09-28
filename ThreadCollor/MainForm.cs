/**
 *  Author:         János de Vries
 *  Date:           Sep. 2014
 *  Student Number: 208418
 **/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace ThreadCollor
{
    /// <summary>
    /// The MainForm class which extends the basic Form
    /// </summary>
    public partial class MainForm : Form
    {
        /**
         *  The Variable declarations for this class. 
        **/
        #region Variable declarations
            //The thread manager which has control over all the ColorCalculators (workers)
            ThreadManager threadManager;
            //The file manager which has control over all the (image) files
            FileManager fileManager;
        
            //The current time when the threads are activated
            DateTime startingTime;

            //A flag which keeps track remembers if ListView column width changes should be saved
            private bool widthChangeFlag = false;

            //The number of tasks when starting
            double imagesWaitingBefore = -1;
        #endregion


        /// <summary>
        /// The constructor from the MainForm class
        /// </summary>
        public MainForm()
        {
            //Initialize the FileManager
            fileManager = new FileManager();
            //Initialize the ThreadManager
            threadManager = new ThreadManager();
            //Add a AllThreadsDone listener to the ThreadManager
            threadManager.allThreadsDone += new ThreadManager.AllThreadsDone(threadsDone);

            //Initialize the form
            InitializeComponent();

            ////Prioritize the WinForm thread above the worker threads
            //Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
        }


        /// <summary>
        /// This method updates the listView_overview with data contained in the FileManager
        /// </summary>
        private void updateListView()
        {
            //Signal the listView that the update has launched
            listView_overview.BeginUpdate();
            //Clear the old data
            listView_overview.Items.Clear();
            
            //For every file in the FileManager
            for(int i = 0; i < fileManager.Count; i++)
            {
                //Grab the current entry
                FileEntry entry = fileManager[i];

                //Create a new ListViewItem (a row)
                ListViewItem newEntry = new ListViewItem(new String[listView_overview.Columns.Count]);
                //Allow individual cells to apply their own style, This is used for the Color cell
                newEntry.UseItemStyleForSubItems = false;

                //Fill all known information into the cells
                newEntry.SubItems[0].Text = entry.getFileName();
                newEntry.SubItems[1].Text = entry.getFilePath();
                newEntry.SubItems[2].Text = entry.getFileSize();
                newEntry.SubItems[3].Text = entry.getStatus();
                newEntry.SubItems[4].Text = entry.getRed();
                newEntry.SubItems[5].Text = entry.getGreen();
                newEntry.SubItems[6].Text = entry.getBlue();

                //Grab the hex value from the entry
                string hexValue = entry.getHex();
                newEntry.SubItems[7].Text = hexValue;
                //If the hexValue is not "-" (representing null)
                if(hexValue != "-")
                {
                    //Color in the color cell
                    newEntry.SubItems[8].BackColor = ColorTranslator.FromHtml("#" + hexValue);
                    //Remove the placeholder text ("-")
                    newEntry.SubItems[8].Text = String.Empty;
                }
                //If the hex value is unknown
                else
                {
                    //Fill the cell with placeholder text
                    newEntry.SubItems[8].Text = "-";
                }

                //Add the entry to the ListView
                listView_overview.Items.Add(newEntry);
                //Pass the position in the ListView to the individual entry
                entry.setEntryNumber(i);
            }

            //Signal that all the new data has been added
            listView_overview.EndUpdate();
        }


        /// <summary>
        /// Opens a FileDialog in which the user can select their image files
        /// </summary>
        /// <returns>A list of selected files</returns>
        private List<String> askForFiles()
        {
            //Create an instance of OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            //Filter to these file extensions
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png | All Files (*.*)|*.*";
            //Select Image Files on default
            openFileDialog.FilterIndex = 1;

            //Allows the user to select multiple files
            openFileDialog.Multiselect = true;

            //Show the OpenFileDialog so the user can select files
            openFileDialog.ShowDialog();

            //Put the dialog results into a list
            //This list will contain the selected file paths
            return openFileDialog.FileNames.ToList();
        }

        /// <summary>
        /// This method saves the time at which the threads start running
        /// </summary>
        private void setStartTime()
        {
            //Set the time at which the threads start running
            startingTime = DateTime.Now;
            //Set the number of images waiting at the moment the threads start
            this.imagesWaitingBefore = fileManager.FilesWaiting;
        }


        /// <summary>
        /// This method returns the total runtime since setTime has been called
        /// If setTime hasn't been called it will return zero
        /// </summary>
        /// <returns>The total run time of all the threads</returns>
        private TimeSpan reportTimeSpent()
        {
            //If the starting time has been set
            if(startingTime != DateTime.MinValue)
            {
                //Calculate the total run time
                TimeSpan total = DateTime.Now - startingTime;
                //Return it
                return total;
            }
            //If not return zero
            return TimeSpan.Zero;
        }


        /// <summary>
        /// Start the calculations
        /// </summary>
        private void start()
        {
            //If there are files waiting to be calculated
            if(fileManager.FilesWaiting > 0)
            {
                //Lock the controls 
                button_start.Text = "Stop";
                button_add.Enabled = false;
                button_remove.Enabled = false;
                comboBox_cores.Enabled = false;
                numericUpDown_threads.Enabled = false;
                
                //Set the time at which the threads have started running
                setStartTime();

                //Send the threads per image to the file manager
                fileManager.setThreadsPerImage((int)numericUpDown_tpi.Value);
                //Generate the task list (queue)
                fileManager.createQueue();

                //Tell the ThreadManager to start the threads
                threadManager.startThreads(fileManager, listView_overview,(int)numericUpDown_threads.Value, Convert.ToInt32(comboBox_cores.Text));
            }
        }


        /// <summary>
        /// Stop the calculations and report statistics
        /// </summary>
        private void stop()
        {
            //If there are still threads running
            if(threadManager.ThreadsRunning)
            {
                //Tell the file manager to stop handing out tasks
                fileManager.setStopFlag();
                //Disable the start/stop button
                button_start.Enabled = false;
            }
            //Else if there are no threads running
            else
            {
                //Get the run time
                double runTime = reportTimeSpent().TotalSeconds;

                //Unlock the controls
                button_start.Text = "Start";
                button_add.Enabled = true;
                button_remove.Enabled = true;
                comboBox_cores.Enabled = true;
                numericUpDown_threads.Enabled = true;

                //Allow the FileManager to hand out tasks
                fileManager.releaseStopFlag();
                
                //If there are files waiting
                if(fileManager.FilesWaiting > 0)
                {
                    //Re-enable the start button
                    button_start.Enabled = true;
                }
                else
                {
                    button_start.Enabled = false;
                }

                //Calculate the number of tasks completed
                double tasksCompleted = (imagesWaitingBefore - fileManager.FilesWaiting);
                //Calculate the time it used, as strings
                string totalTime = Math.Round(runTime, 2).ToString();
                string timePerImage = Math.Round(runTime / tasksCompleted , 2).ToString();
                //Display a MessageBox which shows the total run time, the time per image, number of pixels and pixels per second
                MessageBox.Show(String.Format("Total running time is {0} seconds. \nThat's {1} seconds for each image.\n\nCalculated ~{2} pixels.\n{3} pixels per second.", totalTime, timePerImage, fileManager.getTotalPixelsDone(), (fileManager.getTotalPixelsDone()/totalTime.Length).ToString()), "Run time report");
            }
        }


        /// <summary>
        /// A method that is trigged when all the threads have completed their work
        /// This method is triggered by an event in ThreadManager
        /// </summary>
        private void threadsDone()
        {
            //Call the stop method
            stop();
        }


        /**
         *  All the click events the Windows form uses are within this region.
        **/
        #region Click events
            /// <summary>
            /// This method is bound to a click event on the add button
            /// </summary
            private void button_add_Click(object sender, EventArgs e)
            {
                //Ask the user which files to add
                List<string> selectedFiles = askForFiles();

                //If there are files selected
                if(selectedFiles.Count > 0)
                {
                    //Send the files to the FileManager
                    fileManager.add(selectedFiles);
                    
                    //Enable the start button
                    button_start.Enabled = true;
    
                    //Update the ListView
                    updateListView();
                }
            }


            /// <summary>
            /// When the remove button has been clicked the highlighted files will be removed
            /// </summary>
            private void button_remove_Click(object sender, EventArgs e)
            {
                //For every item in the ListView
                for (int i = listView_overview.Items.Count-1; i >= 0; i--)
                {
                    //If the item has been selected
                    if (listView_overview.Items[i].Selected)
                    {
                        //Remove the item at location i in the FileManager
                        fileManager.removeAt(i);
                    }
                    //Re-create the queue
                    fileManager.createQueue();
                }

                //Update the ListView to remove the items
                updateListView();

                //If every item has been deleted
                if(fileManager.FilesWaiting < 1)
                {
                    //Disable the start button
                    button_start.Enabled = false;
                }
            }


            /// <summary>
            /// Turn the start button in to a start/stop toggle
            /// </summary>
            private void button_start_Click(object sender, EventArgs e)
            {
                if (button_start.Text == "Start")
                {
                    start();
                }
                else
                {
                    stop();
                }
            }
            
            
            /// <summary>
            /// When one of the ListView columns is clicked sort it
            /// </summary>
            private void listView_overview_ColumnClick(object sender, ColumnClickEventArgs e)
            {
                //Grab the index of the selected column
                Int32 colIndex = Convert.ToInt32(e.Column.ToString());

                //Call the sort method within the FileManager
                fileManager.sort(listView_overview.Columns[colIndex].Text);
                //Update the ListView with the sorted information
                updateListView();
            }
        #endregion


        /**
         *  These methods preserve the user settings. For example column width and form size.
        **/
        #region Preserving user settings
            /// <summary>
            /// This method is called on load and runs start-up tasks.
            /// </summary>
            private void MainForm_Load(object sender, EventArgs e)
            {
                // If the user previously changed the window size
                if (Properties.Settings.Default.WindowSize != null)
                {
                    //Restore it to what it used to be
                    this.Size = Properties.Settings.Default.WindowSize;
                }
                //If the user previously changed to column widths
                if (Properties.Settings.Default.ColumnWidths != null)
                {
                    //retrieve the old settings
                    int[] columnWidths = Properties.Settings.Default.ColumnWidths.Split(',').Select(s => Int32.Parse(s)).ToArray();
                    int i = 0;
                    //and for each column
                    foreach (ColumnHeader column in listView_overview.Columns)
                    {
                        //restore it
                        column.Width = columnWidths[i];
                        i++;
                    }
                }

                //Set the max number of (logical) processors
                comboBox_cores.Items.Clear();
                for (int i = 1; i <= Environment.ProcessorCount; i++)
                {
                    //Add an option for each number processors
                    comboBox_cores.SelectedIndex = comboBox_cores.Items.Add(i);
                }
                
                //Restore the number of threads
                numericUpDown_threads.Value = Properties.Settings.Default.Threads;

                //Only column width changes after the form load event should be saved
                this.widthChangeFlag = true;
            }


            /// <summary>
            /// When the form's size changes, save it
            /// </summary>
            private void MainForm_SizeChanged(object sender, EventArgs e)
            {
                Properties.Settings.Default.WindowSize = this.Size;
                Properties.Settings.Default.Save();
            }


            /// <summary>
            /// When the listView's column with changes, save it
            /// </summary>
            private void listView_overview_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
            {
                //If the form has loaded start recording changes in width
                if(this.widthChangeFlag)
                {
                    int[] columnWidths = new int[listView_overview.Columns.Count];
                    int i = 0;
                    //for each column header
                    foreach (ColumnHeader column in listView_overview.Columns)
                    {
                        //save the width in an int array
                        columnWidths[i] = column.Width;
                        i++;
                    }
                    //convert the int array into a string
                    Properties.Settings.Default.ColumnWidths = String.Join(",", columnWidths);
                    //save it
                    Properties.Settings.Default.Save();
                }
            }


            /// <summary>
            /// Save the number of threads if the user changes it's value.
            /// </summary>
            private void numericUpDown_threads_ValueChanged(object sender, EventArgs e)
            {
                numericUpDown_tpi.Maximum = numericUpDown_threads.Value;
                Properties.Settings.Default.Threads = (int)numericUpDown_threads.Value;
                Properties.Settings.Default.Save();
            }


            /// <summary>
            /// Save the number of cores if the user changes it's value.
            /// </summary>
            private void comboBox_cores_TextChanged(object sender, EventArgs e)
            {
                //Properties.Settings.Default.Cores = Convert.ToInt32(comboBox_cores.Items[0]);
                //Properties.Settings.Default.Save();
            }
        #endregion
    }
}