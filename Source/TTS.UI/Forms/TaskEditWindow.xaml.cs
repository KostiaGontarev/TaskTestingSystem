using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using TTS.Core.Abstract.Declarations;
using TTS.Core.Abstract.Model;

using TTS.Core.Concrete;

using TTS.UI.UserControls;


namespace TTS.UI.Forms
{
    public partial class TaskEditWindow : Window
    {
        #region Data Members
        private readonly List<string> errorsList = new List<string>();
        private readonly IOPanel ioPanel;
        private ITask task;
        #endregion

        #region Properties
        public ITask Task
        {
            get { return this.task; }
        }
        #endregion

        #region Constructors
        public TaskEditWindow()
        {
            this.InitializeComponent();
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
            TextRange textRange = new TextRange(DescriptionTextBox.Document.ContentStart, DescriptionTextBox.Document.ContentEnd);
            string str = textRange.Text.Replace("\r\n", "");
            if (String.IsNullOrWhiteSpace(str))
                this.errorsList.Add("Условие");
        }
        private void CheckIO()
        {
            List<ITestInfo> testsInfo = this.ioPanel.GetTestsInfo();
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

            MessageBox.Show("Сохранение прошло успешно!", "Сохранение задачи");
        }

        private void SaveName()
        {
            this.task.Name = NameTextBox.Text;
        }
        private void SaveDescription()
        {
            TextRange textRange = new TextRange(DescriptionTextBox.Document.ContentStart, DescriptionTextBox.Document.ContentEnd);
            string str = textRange.Text.Replace("\r\n", "");
            this.task.Description = str;
        }
        private void SaveIO()
        {
            this.task.Tests.Clear();
            List<ITestInfo> testsInfo = this.ioPanel.GetTestsInfo();
            foreach (ITestInfo testInfo in testsInfo)
            {
                this.task.Tests.Add(testInfo);
            }
        }
        private void SaveRequirements()
        {
            this.task.Requirements.Clear();
            ICharacteristic characteristic = CoreAccessor.CreateCharacteristic();
            characteristic.Type = CharacteristicType.InputOutputCompliance;
            characteristic.Value = true;
            this.task.Requirements.Add(characteristic);
        }

        private void DisplayCurrentTask()
        {
            this.DisplayNameAndDescription();
            this.DisplayIO();
        }
        private void DisplayNameAndDescription()
        {
            this.NameTextBox.Text = this.Task.Name ?? String.Empty;
            this.DescriptionTextBox.AppendText(this.Task.Description ?? String.Empty);
        }
        private void DisplayIO()
        {
            foreach (ITestInfo testInfo in this.Task.Tests)
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