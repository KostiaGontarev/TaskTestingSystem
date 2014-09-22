using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

using TTS.Core;
using TTS.Core.Declarations;
using TTS.Core.Interfaces.Model;
using TTS.Core.Interfaces.Storage;

using TTS.UI.UserControls;


namespace TTS.UI.Windows
{
    public partial class TaskEditWindow : Window
    {
        #region Data Members
        private readonly List<string> errorsList;
        private readonly IOPanel ioPanel;
        private ITask task;
        #endregion

        #region Constructors
        public TaskEditWindow()
        {
            this.InitializeComponent();
            this.errorsList = new List<string>();
            this.ioPanel = new IOPanel();
            this.ContentIOBorder.Child = this.ioPanel;
        }
        public TaskEditWindow(ITask task)
            : this()
        {
            if (task != null)
                this.task = task;
            else
                throw new ArgumentNullException("task");

            this.DisplayCurrentTask();
        }
        #endregion

        #region Event Handlers
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.errorsList.Clear();
            this.CheckTask();
            if (this.errorsList.Count > 0)
                this.CheckErrors();
            else
            {
                this.SaveTask();
                this.Close();
            }
        }
        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.ioPanel.AddItem();
        }
        #endregion

        #region Assistance
        private void CheckTask()
        {
            this.CheckName();
            this.CheckDescription();
            this.CheckIO();
        }
        private void CheckName()
        {
            if (String.IsNullOrWhiteSpace(this.NameTextBox.Text))
                this.errorsList.Add("Название");
        }
        private void CheckDescription()
        {
            TextRange textRange = new TextRange(this.DescriptionTextBox.Document.ContentStart, this.DescriptionTextBox.Document.ContentEnd);
            string str = textRange.Text.Replace("\r\n", "");
            if (String.IsNullOrWhiteSpace(str))
                this.errorsList.Add("Условие");
        }
        private void CheckIO()
        {
            List<ITestInfo> testsInfo = this.ioPanel.GetTestsInfo();
            if (testsInfo.Count == 0)
                this.errorsList.Add("Тесты");

            foreach (ITestInfo testInfo in testsInfo)
            {
                if (String.IsNullOrWhiteSpace(testInfo.Input) || String.IsNullOrWhiteSpace(testInfo.Output))
                {
                    this.errorsList.Add("Тесты");
                }
            }
        }

        private void SaveTask()
        {
            if (this.task == null)
                this.task = CoreAccessor.CreateTask();
            this.SaveName();
            this.SaveDescription();
            this.SaveIO();
            this.SaveRequirements();

            IDataStorage storage = CoreAccessor.GetStorage();
            ITask toDelete = storage.Tasks.SingleOrDefault(task => task.ID == this.task.ID);
            if (toDelete == null)
                storage.Tasks.Add(this.task);
            MessageBox.Show("Сохранение прошло успешно!", "Сохранение задачи");
        }
        private void SaveName()
        {
            this.task.Name = this.NameTextBox.Text;
        }
        private void SaveDescription()
        {
            TextRange textRange = new TextRange(this.DescriptionTextBox.Document.ContentStart, this.DescriptionTextBox.Document.ContentEnd);
            string str = textRange.Text.Replace("\r\n", "");
            this.task.Description = str;
        }
        private void SaveIO()
        {
            List<ITestInfo> testsInfo = this.ioPanel.GetTestsInfo();
            this.UpdateIO(testsInfo);
            this.task.Tests.Clear();
            foreach (ITestInfo testInfo in testsInfo)
            {
                this.task.Tests.Add(testInfo.ID);
            }
        }
        private void UpdateIO(IList<ITestInfo> newTests)
        {
            IDataStorage storage = CoreAccessor.GetStorage();
            List<ITestInfo> current = storage.Tests.Where(test => this.task.Tests.Contains(test.ID)).ToList();
            this.AddTests(newTests, current);
            this.DeleteTests(newTests, current);
            this.UpdateTests(newTests, current);
        }
        private void AddTests(IEnumerable<ITestInfo> newTests, IEnumerable<ITestInfo> currentTests)
        {
            IDataStorage storage = CoreAccessor.GetStorage();
            List<ITestInfo> toAdd = newTests.Where(newTest => currentTests.All(oldTest => oldTest.ID != newTest.ID)).ToList();
            foreach (ITestInfo testInfo in toAdd)
            {
                storage.Tests.Add(testInfo);
            }
        }
        private void DeleteTests(IEnumerable<ITestInfo> newTests, IEnumerable<ITestInfo> currentTests)
        {
            IDataStorage storage = CoreAccessor.GetStorage();
            List<ITestInfo> toDelete = currentTests.Where(oldTest => newTests.All(newTest => newTest.ID != oldTest.ID)).ToList();
            foreach (ITestInfo testInfo in toDelete)
            {
                storage.Tests.Remove(testInfo);
            }
        }
        private void UpdateTests(IEnumerable<ITestInfo> newTests, IEnumerable<ITestInfo> currentTests)
        {
            List<ITestInfo> toUpdate = currentTests.Where(oldTest => newTests.Any(test => test.ID == oldTest.ID)).ToList();
            foreach (ITestInfo testInfo in newTests)
            {
                ITestInfo newTestInfo = toUpdate.SingleOrDefault(test => test.ID == testInfo.ID);
                if (newTestInfo == null)
                    return;
                newTestInfo.Input = testInfo.Input;
                newTestInfo.Output = testInfo.Output;
            }
        }

        private void SaveRequirements()
        {
            this.task.Requirements.Clear();
            Characteristic characteristic = new Characteristic
            {
                Type = CharacteristicType.InputOutputCompliance,
                Value = true
            };
            this.task.Requirements.Add(characteristic);
        }

        private void DisplayCurrentTask()
        {
            this.DisplayNameAndDescription();
            this.DisplayIO();
        }
        private void DisplayNameAndDescription()
        {
            this.NameTextBox.Text = this.task.Name ?? String.Empty;
            this.DescriptionTextBox.AppendText(this.task.Description ?? String.Empty);
        }
        private void DisplayIO()
        {
            IDataStorage storage = CoreAccessor.GetStorage();
            List<ITestInfo> tests = storage.Tests.Where(test => this.task.Tests.Contains(test.ID)).ToList();
            foreach (ITestInfo testInfo in tests)
            {
                this.ioPanel.AddItem(testInfo);
            }
        }

        private void CheckErrors()
        {
            string result = String.Empty;
            result += "Во время сохранения возникли ошибки. Обратите внимание на следующие пункты: " +
                      Environment.NewLine;
            result = this.errorsList.Aggregate(result,
                (current, error) => current + ("* " + error + ";" + Environment.NewLine));
            MessageBox.Show(result, "Внимание!");
        }
        #endregion
    }
}