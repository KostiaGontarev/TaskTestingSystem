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
            this.task = CoreAccessor.CreateTask();
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
            this.SaveTask();
            this.CheckErrors();
        }
        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PlusButton_OnClick(object sender, RoutedEventArgs e)
        {
            //Здесь будет не совсем это - здесь будет проверка по типам требований. Потом уточню
            RequirementSetupControl requirementSetupControl = new RequirementSetupControl();
            this.RequirementsStackPanel.Children.Add(requirementSetupControl);
        }
        private void MinusButton_OnClick(object sender, RoutedEventArgs e)
        {
            //Надо делать выделяемый элемент и удалять выделенный, но пока и так сойдёт.
            if (this.RequirementsStackPanel.Children.Count != 0)
            {
                this.RequirementsStackPanel.Children.RemoveAt(this.RequirementsStackPanel.Children.Count - 1);
            }
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.ioPanel.AddItem();
        }
        #endregion

        #region Assistance
        private void SaveTask()
        {
            this.SaveName();
            this.SaveDescription();
            this.SaveRequirements();
            this.SaveIO();
        }
        private void SaveName()
        {
            if (!String.IsNullOrWhiteSpace(this.NameTextBox.Text))
                this.Task.Name = this.NameTextBox.Text;
            else
                this.errorsList.Add("Название");
        }
        private void SaveDescription()
        {
            TextRange textRange = new TextRange(DescriptionTask.Document.ContentStart,
                DescriptionTask.Document.ContentEnd);

            string str = textRange.Text.Replace("\r\n", "");

            if (!String.IsNullOrWhiteSpace(str))
            {
                this.Task.Description = str;
            }
            else
            {
                this.errorsList.Add("Условие");
            }
        }
        private void SaveRequirements()
        {
            this.task.Requirements.Clear();
            if (this.RequirementsStackPanel.Children.Count != 0)
            {
                foreach (UIElement element in this.RequirementsStackPanel.Children)
                {
                    if (element is RequirementSetupControl)
                    {
                        this.SaveRequirement(element as RequirementSetupControl);
                    }
                }
            }
        }
        private void SaveRequirement(RequirementSetupControl requirementSetupControl)
        {
            if (!String.IsNullOrWhiteSpace(requirementSetupControl.RequirementValueTextBox.Text))
            {
                ICharacteristic characterictic = null;
                bool converted = this.SetupCharacterictic(requirementSetupControl, out characterictic);

                if (converted)
                {
                    this.task.Requirements.Add(characterictic);
                    return;
                }
            }

            this.errorsList.Add("Значение требования " + requirementSetupControl.RequirementTypeComboBox.Text);
        }
        private bool SetupCharacterictic(RequirementSetupControl requirementSetupControl, out ICharacteristic characterictic)
        {
            characterictic = CoreAccessor.CreateCharacteristic();
            characterictic.Type = (CharacteristicType) requirementSetupControl.RequirementTypeComboBox.SelectedItem;
            double value = 0.0;
            bool converted = double.TryParse(requirementSetupControl.RequirementValueTextBox.Text, out value);
            characterictic.Value = value;
            return converted;
        }

        private void SaveIO()
        {
            List<ITestInfo> testsInfo = this.ioPanel.GetTestsInfo();
            this.task.Tests.Clear();
            foreach (ITestInfo testInfo in testsInfo)
            {
                if (!String.IsNullOrWhiteSpace(testInfo.Input) || !String.IsNullOrWhiteSpace(testInfo.Output))
                {
                    this.task.Tests.Add(testInfo);
                }
            }
        }
        private void CheckErrors()
        {
            string result = String.Empty;
            if (this.errorsList.Count > 0)
            {
                result += "Во время сохранения возникли ошибки. Обратите внимание на следующие пункты: " +
                          Environment.NewLine;
                result = this.errorsList.Aggregate(result,
                    (current, error) => current + ("* " + error + ";" + Environment.NewLine));
                MessageBox.Show(result, "Внимание!");
            }
            else
            {
                MessageBox.Show("Сохранение прошло успешно!", "Успех!");
                this.Close();
            }
        }

        private void DisplayCurrentTask()
        {
            this.DisplayNameAndDescription();
            this.DisplayRequirements();
            this.DisplayIO();
        }
        private void DisplayNameAndDescription()
        {
            this.NameTextBox.Text = this.Task.Name ?? String.Empty;
            this.DescriptionTask.AppendText(this.Task.Description ?? String.Empty);
        }
        private void DisplayRequirements()
        {
            foreach (ICharacteristic characteristic in this.Task.Requirements)
            {
                RequirementSetupControl requirementSetupControl = new RequirementSetupControl
                {
                    RequirementTypeComboBox = { SelectedItem = characteristic.Type },
                    RequirementValueTextBox = { Text = characteristic.Value.ToString() }
                };
                this.RequirementsStackPanel.Children.Add(requirementSetupControl);
            }
        }
        private void DisplayIO()
        {
            foreach (ITestInfo testInfo in this.Task.Tests)
            {
                this.ioPanel.AddItem(testInfo);
            }
        }
        #endregion

    }
}