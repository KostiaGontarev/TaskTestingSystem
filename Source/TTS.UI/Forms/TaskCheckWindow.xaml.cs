using System.Linq;
using System.Windows;
using System.Collections.Generic;

using Microsoft.Win32;

using TTS.Core.Abstract.Model;

using TTS.Core.Abstract.Controllers;
using TTS.Core.Abstract.Processing;
using TTS.Core.Concrete;

using TTS.UI.UserControls;


namespace TTS.UI.Forms
{
    public partial class TaskCheckWindow : Window
    {
        #region Data Members
        private readonly ITestController controller;
        private readonly TestingFilesPanel testingFilesPanel;
        #endregion

        #region Constructors
        private TaskCheckWindow()
        {
            this.InitializeComponent();
            this.testingFilesPanel = new TestingFilesPanel();
            this.TestingFilesPanel.Children.Add(testingFilesPanel);
            this.controller = CoreAccessor.GetTestController();
        }
        public TaskCheckWindow(ITask task)
            : this()
        {
            this.controller.Task = task;
            this.SetupTests();
        }
        #endregion

        #region Event Handlers
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "executable files (*.exe)|*.exe"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                this.testingFilesPanel.AddItem(openFileDialog.FileName);
            }
        }

        private void CheckSelectedButton_OnClick(object sender, RoutedEventArgs e)
        {
            List<ITest> selected =
                    this.TestsPanel.Children.OfType<TestIndicator>()
                    .Where(element => element.TestCheckBox.IsChecked == true)
                    .Select(element => element.Test)
                    .ToList();
            List<string> files = this.testingFilesPanel.GetCheckedFiles();
            if (selected.Count != 0 && files.Count != 0)
                this.Run(selected, files);
        }

        private void CheckAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            List<ITest> selected =
                    this.TestsPanel.Children.OfType<TestIndicator>()
                    .Select(element => element.Test)
                    .ToList();
            List<string> files = this.testingFilesPanel.GetCheckedFiles();
            if (files.Count != 0)
                this.Run(selected, files);
        }

        private void StopCheckButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void SelectAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.testingFilesPanel.SelectAllFiles();
        }
        #endregion

        #region Assistance
        private void SetupTests()
        {
            const string title = "Тест №";
            int index = 1;
            foreach (ITest test in this.controller.Tests)
            {
                TestIndicator testIndicator = new TestIndicator(test, title + index);
                this.TestsPanel.Children.Add(testIndicator);
                index++;
            }
        }

        private void DisableFunctionality()
        {
            CheckAllButton.Visibility = Visibility.Hidden;
            CheckSelectedButton.Visibility = Visibility.Hidden;
            TestsPanel.IsEnabled = false;
            AddButton.IsEnabled = false;
            testingFilesPanel.IsEnabled = false;
        }
        private void EnableFunctionality()
        {
            CheckAllButton.Visibility = Visibility.Visible;
            CheckSelectedButton.Visibility = Visibility.Visible;
            TestsPanel.IsEnabled = true;
            AddButton.IsEnabled = true;
            testingFilesPanel.IsEnabled = true;
        }

        private void Run(IList<ITest> tests, IList<string> files)
        {
            this.DisableFunctionality();
            this.controller.Run(tests, files);
            this.EnableFunctionality();
        }
        #endregion



    }
}