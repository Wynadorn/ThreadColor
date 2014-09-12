using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Regex.Match(inputColor, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success

namespace ThreadCollor
{
    public partial class MainForm : Form
    {
        /**
         *  The Variable declarations within this region. 
        **/
        #region Variable declarations
            //A list containing references to all the tasks that have to be done
            private static Queue<FileEntry> taskList = new Queue<FileEntry>();
            //Only column width changed after the form load event should be saved
            private static List<FileEntry> backlog_overview = new List<FileEntry>();

            private bool threadsRunning = false;

            ThreadManager threadManager;
        
            //A flag which is set after the form loads
            private bool widthChangeFlag = false;
        #endregion

        public MainForm()
        {
            threadManager = new ThreadManager();
            threadManager.allThreadsDone += new ThreadManager.AllThreadsDone(threadsDone);

            //Initialize the form
            InitializeComponent();
        }

        /// <summary>
        /// This method updates the listView_overview with data contained in the backlog
        /// </summary>
        private void updateOverview()
        {
            listView_overview.Items.Clear();
            for(int i = 0; i < backlog_overview.Count; i++)
            {
                FileEntry backlog_entry = backlog_overview[i];

                ListViewItem newEntry = new ListViewItem(new String[8]);
                newEntry.UseItemStyleForSubItems = false;
                newEntry.SubItems[0].Text = backlog_entry.getFileName();
                newEntry.SubItems[1].Text = backlog_entry.getFilePath();
                newEntry.SubItems[2].Text = backlog_entry.getStatus();
                newEntry.SubItems[3].Text = backlog_entry.getRed();
                newEntry.SubItems[4].Text = backlog_entry.getGreen();
                newEntry.SubItems[5].Text = backlog_entry.getBlue();
                string hexValue = backlog_entry.getHex();
                newEntry.SubItems[6].Text = hexValue;
                if(hexValue != "-")
                {
                    newEntry.SubItems[7].BackColor = ColorTranslator.FromHtml("#" + hexValue);
                    newEntry.SubItems[7].Text = String.Empty;
                }
                else
                {
                    newEntry.SubItems[7].Text = "-";
                }
                listView_overview.Items.Add(newEntry);
                backlog_entry.setEntryNumber(i);
            }
            if(listView_overview.Items.Count > 0)
            {
                listView_overview.Update();
                listView_overview.Refresh();
                listView_overview.EnsureVisible(listView_overview.Items.Count - 1);
            }
        }

        private List<String> askForFiles()
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter options and filter index.
            //openFileDialog.Filter = "png (.png)|*.png|All Files (*.*)|*.*";
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png | All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            //Allows the user to select multiple files
            openFileDialog.Multiselect = true;

            // Call the ShowDialog method to show the dialog box wherein the user can select files.
            openFileDialog.ShowDialog();

            //Put the dialog results into a list
            //This list will contain file paths
            return openFileDialog.FileNames.ToList();
        }

        private void start()
        {
            threadManager.setListView(listView_overview);
            
            //Lock the controls
            button_start.Text = "Stop";
            button_add.Enabled = false;
            button_remove.Enabled = false;
            comboBox_cores.Enabled = false;
            numericUpDown_threads.Enabled = false;
            
            if(backlog_overview.Count > 0)
            {
                threadsRunning = true;
                foreach(FileEntry entry in backlog_overview)
                {
                    if(entry.getStatus() == "Waiting")
                    {
                        taskList.Enqueue(entry);
                    }
                }
            }

            threadManager.startThreads(taskList, (int)numericUpDown_threads.Value);
        }

        private void stop()
        {
            if(button_start.Text == "Stop")
            {
                if(threadsRunning)
                {
                    taskList.Clear();
                    button_start.Enabled = false;
                }
                else
                {
                    //Unlock the controls
                    button_start.Text = "Start";
                    button_add.Enabled = true;
                    button_remove.Enabled = true;
                    comboBox_cores.Enabled = true;
                    numericUpDown_threads.Enabled = true;
                }
            }
        }

        private void threadsDone()
        {
            threadsRunning = false;
            if(button_start.Enabled == false)
            {
                button_start.Enabled = true;
            }
            stop();
        }

        /**
         *  All the click events the Windows form uses are within this region.
        **/
        #region Click events
            private void button_add_Click(object sender, EventArgs e)
            {
                //Ask the user which files to add
                List<string> selectedFiles = askForFiles();
            
                //Process input if the use clicked OK.

                //Add the files to the backlog
                foreach(string filepath in selectedFiles)
                {
                    FileEntry fileEntry = new FileEntry(System.IO.Path.GetFileName(filepath), filepath);

                    backlog_overview.Add(fileEntry);
                }
                if(backlog_overview.Count > 0)
                {
                    button_start.Enabled = true;
                }

                updateOverview();
            }

            private void button_remove_Click(object sender, EventArgs e)
            {
                for (int i = listView_overview.Items.Count-1; i >= 0; i--)
                {
                    if (listView_overview.Items[i].Selected)
                    {
                        backlog_overview.RemoveAt(i);
                        //listView_overview.Items[i].Remove();
                    }
                }
                updateOverview();
                if(backlog_overview.Count <= 0)
                {
                    button_start.Enabled = false;
                }
            }

            //Start or stop calculations
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

            //Import the current overview
            private void button_import_Click(object sender, EventArgs e)
            {

            }

            //Export the current overview
            private void button_export_Click(object sender, EventArgs e)
            {

            }

            //When the filter criteria is changed it will be applied imidiatly
            private void textBox_filter_TextChanged(object sender, EventArgs e)
            {

            }
        #endregion

        /**
         *  These methods preserve the user settings. For example collum with and form size.
        **/
        #region Preserving user settings
            /// <summary>
            /// This method is called on load and runs start-up tasks.
            /// </summary>
            private void MainForm_Load(object sender, EventArgs e)
            {
                // If the user changed the window size
                if (Properties.Settings.Default.WindowSize != null)
                {
                    //Restore it to what it used to be
                    this.Size = Properties.Settings.Default.WindowSize;
                }
                //If the user changed to column widths
                if (Properties.Settings.Default.ColumnWidths != null)
                {
                    //retrieve the old settings
                    int[] columnWidths = Properties.Settings.Default.ColumnWidths.Split(',').Select(s => Int32.Parse(s)).ToArray();
                    int i = 0;
                    //and for each column
                    foreach (ColumnHeader column in listView_overview.Columns)
                    {
                        //retore it
                        column.Width = columnWidths[i];
                        i++;
                    }
                }
                
                //Resotre the number of cores
                comboBox_cores.Text = Properties.Settings.Default.Cores.ToString();
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
                    int[] columnWidths = new int[8];
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
                Properties.Settings.Default.Threads = (int)numericUpDown_threads.Value;
                Properties.Settings.Default.Save();
            }

            /// <summary>
            /// Save the number of cores if the user changes it's value.
            /// </summary>
            private void comboBox_cores_TextChanged(object sender, EventArgs e)
            {
                Properties.Settings.Default.Cores = Convert.ToInt32(comboBox_cores.Text);
                Properties.Settings.Default.Save();
            }
        #endregion
    }
}