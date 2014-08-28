using System.Windows;
using System.IO;
using System;

using Microsoft.Win32;

using TTS.Core.Abstract.Model;



namespace TTS.UI.Forms
{
    public partial class TaskCheckWindow : Window
    {
        #region Members
        private ITask task;
        #endregion

        #region Properties
        public ITask Task
        {
            get { return this.task; }
        }
        #endregion

        #region Constructors
        public TaskCheckWindow()
        {
            this.InitializeComponent();
        }

        public TaskCheckWindow(ITask task)
            : this()
        {
            int i = 1;
            this.task = task;
            foreach (ITestInfo testInfo in task.Tests)
            {
                Test test = new Test(testInfo, i);
                TestsList.Items.Add(test);
                i++;
            }
        }
        #endregion

        #region Events
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "exe files (*.exe)|*.exe";
            if (openFileDialog.ShowDialog() == DialogResult.Equals(true))
            {
                
            }
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void CheckCurrentButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void CheckAllButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}