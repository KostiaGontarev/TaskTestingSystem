using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;

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
            this.controller.AllTestsFinished += controller_AllTestsFinished;
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
                List<string> files = this.filesPanel.GetFiles();
                if (!files.Contains(openFileDialog.FileName))
                    this.filesPanel.AddItem(openFileDialog.FileName);
                else
                    MessageBox.Show("Такой файл уже есть в списке!", "Ошибка!");
            }
        }
        private void CheckSelectedButton_OnClick(object sender, RoutedEventArgs e)
        {
            List<Guid> selected =
                    this.indicators.Where(element => element.TestCheckBox.IsChecked == true)
                    .Select(element => element.TestId)
                    .ToList();
            List<string> files = this.filesPanel.GetSelectedFiles();
            this.Run(selected, files);
        }
        private void CheckAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            List<Guid> selected = this.indicators.Select(element => element.TestId).ToList();
            List<string> files = this.filesPanel.GetFiles();
            this.Run(selected, files);
        }
        private void StopCheckButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.controller.Stop();
        }
        private void FilesPanel_OnSelectionChanged(object sender, EventArgs e)
        {
            string current = this.filesPanel.CurrentFile;
            ITaskTestResult result = this.controller.Results.LastOrDefault(element => element.FilePath == current);
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

            if (this.filesPanel.CurrentFile != args.FileName)
                this.filesPanel.SelectFile(args.FileName);

            TestIndicator current = this.indicators.SingleOrDefault(element => element.TestId == args.TestInfo.ID);
            if (current != null)
            {
                current.SubscribeToController();
                foreach (TestIndicator indicator in this.indicators)
                {
                    if (indicator.TestId != current.TestId)
                        indicator.UnsubscribeFromController();
                }
            }
        }

        private void controller_AllTestsFinished(object sender, EventArgs e)
        {
            this.EnableFunctionality();
            foreach (TestIndicator indicator in this.indicators)
            {
                indicator.UnsubscribeFromController();
            }
        }
        #endregion

        #region Assistance
        private void SetupTests()
        {
            const string title = "Тест №";
            int number = 1;
            foreach (Guid testId in this.controller.Task.Tests)
            {
                TestIndicator testIndicator = new TestIndicator(testId, title + number);
                this.indicators.Add(testIndicator);
                this.TestsPanel.Children.Add(testIndicator);
                number++;
            }
        }
        private void DisableFunctionality()
        {
            CheckAllButton.Visibility = Visibility.Collapsed;
            CheckSelectedButton.Visibility = Visibility.Collapsed;
            CheckAllButton.IsEnabled = false;
            CheckAllButton.IsEnabled = false;
            TestsPanel.IsEnabled = false;
            AddButton.IsEnabled = false;
            filesPanel.IsEnabled = false;

            StopCheckButton.IsEnabled = true;
            StopCheckButton.Visibility = Visibility.Visible;
        }
        private void EnableFunctionality()
        {
            StopCheckButton.IsEnabled = false;
            StopCheckButton.Visibility = Visibility.Collapsed;

            CheckAllButton.Visibility = Visibility.Visible;
            CheckSelectedButton.Visibility = Visibility.Visible;
            CheckAllButton.IsEnabled = true;
            CheckAllButton.IsEnabled = true;
            TestsPanel.IsEnabled = true;
            AddButton.IsEnabled = true;
            filesPanel.IsEnabled = true;
        }
        private void Run(IList<Guid> tests, IList<string> files)
        {
            if (tests.Count != 0 && files.Count != 0)
            {
                this.DisableFunctionality();
                this.controller.Run(tests, files);
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
                ITestResult testResult = result.Results.SingleOrDefault(element => element.TestID == indicator.TestId);
                if (testResult != null)
                {
                    indicator.IndicatorState = testResult.IsPassed;
                }
            }
        }
        #endregion
    }
}