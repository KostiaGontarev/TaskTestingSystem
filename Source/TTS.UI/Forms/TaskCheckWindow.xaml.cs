using System.Windows;
using System.Collections.Generic;

using Microsoft.Win32;

using TTS.Core.Abstract.Model;

using TTS.Core.Abstract.Controllers;
using TTS.Core.Concrete;

using TTS.UI.UserControls;


namespace TTS.UI.Forms
{
    public partial class TaskCheckWindow : Window
    {
        #region Data Members
        private readonly ITask task;
        private ITestController testController;
        private IList<string> filesInfo;
        private IList<ITestInfo> currentTests;
        private TestingFilesPanel testingFilesPanel;
        #endregion

        #region Constructors
        private TaskCheckWindow()
        {
            this.InitializeComponent();
            this.testingFilesPanel = new TestingFilesPanel();
            this.TestingFilesStackPanel.Children.Add(testingFilesPanel);
            this.currentTests = new List<ITestInfo>();
            this.testController = CoreAccessor.GetTestController();
        }
        public TaskCheckWindow(ITask task)
            : this()
        {
            this.task = task;
            this.SetupTests();
        }
        #endregion

        #region Event Handlers
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "executable files (*.exe)|*.exe";
            if (openFileDialog.ShowDialog() == true)
            {
                testingFilesPanel.AddItem(openFileDialog.FileName);
            }
        }

        private void CheckCurrentButton_OnClick(object sender, RoutedEventArgs e)
        {

            TestIndicator testIndicator;
            foreach (UIElement test in TestsStackPanel.Children)
            {
                testIndicator = (TestIndicator)test;
                if (testIndicator.TestCheckBox.IsChecked == true)
                {
                    this.currentTests.Add(testIndicator.TestInfo);
                }
            }
            if (currentTests.Count != 0)
            {
                RunTests(currentTests);
            }
            currentTests.Clear();
        }

        private void CheckAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            currentTests = task.Tests;
            RunTests(currentTests);
        }

        private void StopCheckButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Assistance
        private void SetupTests()
        {
            const string title = "Тест №";
            int index = 1;
            foreach (ITestInfo testInfo in this.task.Tests)
            {
                TestIndicator testIndicator = new TestIndicator(testInfo, title + index);
                this.TestsStackPanel.Children.Add(testIndicator);
                index++;
            }
        }

        private void DisableFunctionality()
        {
            CheckAllButton.Visibility = System.Windows.Visibility.Hidden;
            CheckCurrentButton.Visibility = System.Windows.Visibility.Hidden;
            TestsStackPanel.IsEnabled = false;
            AddButton.IsEnabled = false;
            testingFilesPanel.IsEnabled = false;
        }

        private void EnableFunctionality()
        {
            CheckAllButton.Visibility = System.Windows.Visibility.Visible;
            CheckCurrentButton.Visibility = System.Windows.Visibility.Visible;
            TestsStackPanel.IsEnabled = true;
            AddButton.IsEnabled = true;
            testingFilesPanel.IsEnabled = true;
        }

        private void RunTests(IList<ITestInfo> tests)
        {
            DisableFunctionality();
            this.testController.Run(tests);
            EnableFunctionality();
        }
        #endregion
    }
}