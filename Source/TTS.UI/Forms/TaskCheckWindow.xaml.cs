using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Win32;

using TTS.Core.Abstract.Model;

using TTS.Core.Abstract.Controllers;
using TTS.Core.Concrete;
using TTS.Core.Concrete.Processing;
using TTS.UI.UserControls;


namespace TTS.UI.Forms
{
    public partial class TaskCheckWindow : Window
    {
        #region Data Members
        private readonly ITestController controller;
        private readonly TestingFilesPanel filesPanel;
        private readonly List<TestIndicator> indicators = new List<TestIndicator>();
        #endregion

        #region Constructors
        private TaskCheckWindow()
        {
            this.InitializeComponent();

            this.filesPanel = new TestingFilesPanel();
            this.filesPanel.SelectionChanged += FilesPanel_OnSelectionChanged;
            this.TestingFilesPanel.Children.Add(filesPanel);

            this.controller = CoreAccessor.GetTestController();
            this.controller.TestChanged += controller_TestChanged;
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
                this.filesPanel.AddItem(openFileDialog.FileName);
            }
        }
        private void CheckSelectedButton_OnClick(object sender, RoutedEventArgs e)
        {
            List<ITestInfo> selected =
                    this.indicators.Where(element => element.TestCheckBox.IsChecked == true)
                    .Select(element => element.TestInfo)
                    .ToList();
            List<string> files = this.filesPanel.GetSelectedFiles();
            this.Run(selected, files);
        }
        private void CheckAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            List<ITestInfo> selected = this.indicators.Select(element => element.TestInfo).ToList();
            List<string> files = this.filesPanel.GetFiles();
            this.Run(selected, files);
        }
        private void StopCheckButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
        private void FilesPanel_OnSelectionChanged(object sender, EventArgs e)
        {
            string current = this.filesPanel.CurrentFile;
            ITaskTestResult result = this.controller.Task.Results.SingleOrDefault(element => element.FilePath == current);
            if (result == null)
                this.ResetIndicators();
            else
                this.SetupIndicators(result);
        }
        private void controller_TestChanged(object sender, EventArgs e)
        {
            TestEventArgs args = e as TestEventArgs;
            if (args == null)
                return;

            if (this.filesPanel.CurrentFile == args.FileName)
            {
                TestIndicator current = this.indicators.SingleOrDefault(element => element.TestInfo == args.TestInfo);
                if (current != null)
                {
                    current.SubscribeToController();
                    foreach (TestIndicator indicator in this.indicators)
                    {
                        if (indicator != current)
                            indicator.UnsubscribeFromController();
                    }
                }
            }
        }
        #endregion

        #region Assistance
        private void SetupTests()
        {
            const string title = "Тест №";
            int number = 1;
            foreach (ITestInfo testInfo in this.controller.Task.Tests)
            {
                TestIndicator testIndicator = new TestIndicator(testInfo, title + number);
                this.indicators.Add(testIndicator);
                this.TestsPanel.Children.Add(testIndicator);
                number++;
            }
        }
        private void DisableFunctionality()
        {
            CheckAllButton.Visibility = Visibility.Hidden;
            CheckSelectedButton.Visibility = Visibility.Hidden;
            TestsPanel.IsEnabled = false;
            AddButton.IsEnabled = false;
            filesPanel.IsEnabled = false;
        }
        private void EnableFunctionality()
        {
            CheckAllButton.Visibility = Visibility.Visible;
            CheckSelectedButton.Visibility = Visibility.Visible;
            TestsPanel.IsEnabled = true;
            AddButton.IsEnabled = true;
            filesPanel.IsEnabled = true;
        }
        private void Run(IList<ITestInfo> tests, IList<string> files)
        {
            if (tests.Count != 0 && files.Count != 0)
            {
                this.DisableFunctionality();
                this.controller.Run(tests, files);
                this.EnableFunctionality();
            }
            else
                MessageBox.Show("Выберите тесты и файлы!", "Ошибка!");
        }
        private void ResetIndicators()
        {
            foreach (TestIndicator indicator in this.indicators)
            {
                indicator.Reset();
            }
        }
        private void SetupIndicators(ITaskTestResult result)
        {
            foreach (TestIndicator indicator in this.indicators)
            {
                ITestResult testResult = result.Results.SingleOrDefault(element => element.Test == indicator.TestInfo);
                if (testResult != null)
                {
                    bool success = this.controller.IsTestPassed(this.controller.Task.Requirements, testResult.Requirements);
                    indicator.IndicatorState = success;
                }
            }
        }
        #endregion
    }
}