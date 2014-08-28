using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using TTS.Core.Concrete;
using TTS.Core.Abstract.Model;
using TTS.Core.Abstract.Declarations;

using TTS.UI.UserControls;


namespace TTS.UI
{
    public partial class EditTask : Window
    {
        #region Data Member
        private ITask task;
        private IOContent content;
        #endregion

        #region Properties
        public ITask Task
        {
            get { return this.task; }
        }
        #endregion

        #region Constructors
        public EditTask()
        {
            this.InitializeComponent();
            content = new IOContent();
            ContentIOBorder.Child = content;
        }
        public EditTask(ITask task)
            : this()
        {
            try
            {
                this.task = task;
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show("Невозможно открыть задание", e.ToString());
                this.Close();
            }
            this.DisplayCurrentTask();
        }
        #endregion

        #region Event Handlers
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (task == null)
            {
                task = CoreAccessor.CreateTask();
            }

            if (SaveNameDescription(this.task))
            {
                SaveRequirements(this.task);
                SaveIO(this.task);
                this.Close();
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            content.AddButtonIO();
        }

        private void PlusButtonClick(object sender, RoutedEventArgs e)
        {
            //Здесь будет не совсем это - здесь будет проверка по типам требований. Потом уточню
            if (RequirementsStackPanel.Children.Count < 3)
            {
                Requirements requirement = new Requirements();
                RequirementsStackPanel.Children.Add(requirement);

            }
            else
            {
                MessageBox.Show("Нельзя объявить больше, чем 3 требования", "Слишком много требований");
            }
        }

        private void MinusbuttonClick(object sender, RoutedEventArgs e)
        {
            if (RequirementsStackPanel.Children.Count != 0)
            {
                RequirementsStackPanel.Children.RemoveAt(RequirementsStackPanel.Children.Count - 1);
            }
            else
            {
                MessageBox.Show("Отсутствуют требования для удаления", "Требования отсутствуют");
            }
        }
        #endregion

        #region Assistance
        private bool SaveNameDescription(ITask task)
        {

            if (NameTask.Text == "")
            {
                MessageBox.Show("Введите название задания", "Введите название");
                return false;
            }
            else
            {
                task.Name = NameTask.Text;
                if (ConditionTask.Text == "")
                {
                    MessageBox.Show("Введите условие для задания", "Введите условие");
                    return false;
                }
                else
                {
                    task.Description = ConditionTask.Text;
                    return true;
                }
            }
        }
        private void SaveRequirements(ITask task)
        {
            if (RequirementsStackPanel.Children.Count != 0)
            {
                Requirements requirements = new Requirements();

                foreach (UIElement requirement in RequirementsStackPanel.Children)
                {
                    requirements = (Requirements)requirement;

                    if (requirements.RequirementComboBox.Text != "" && requirements.RequirementsTextBox.Text != "")
                    {
                        ICharacteristic characterictic = CoreAccessor.CreateCharacteristic();
                        characterictic.Type = (CharacteristicType)requirements.RequirementComboBox.SelectedItem;
                        characterictic.Value = double.Parse(requirements.RequirementsTextBox.Text);
                        task.Requirements.Add(characterictic);
                    }
                }
            }
        }
        private void SaveIO(ITask task)
        {
            content.SaveContentIO(task);
        }

        private void DisplayCurrentTask()
        {
            this.DisplayRequiements(this.task);
            this.DisplayNameDescription(this.task);
            this.DisplayIO(this.task);
        }
        private void DisplayRequiements(ITask task)
        {
            foreach (ICharacteristic characteristic in task.Requirements)
            {
                Requirements requirements = new Requirements();
                requirements.RequirementComboBox.SelectedItem = characteristic.Type;
                requirements.RequirementsTextBox.Text = characteristic.Value.ToString();
                RequirementsStackPanel.Children.Add(requirements);
            }
        }
        private void DisplayNameDescription(ITask task)
        {
            if (task.Name != null)
            {
                this.NameTask.Text = task.Name;
            }
            if (task.Description != null)
            {
                this.ConditionTask.Text = task.Description;
            }
        }
        private void DisplayIO(ITask task)
        {
            foreach (ITestInfo testInfo in task.Tests)
            {
                content.AddButtonIO(testInfo.Input, testInfo.Output);
            }
        }
        #endregion
    }
}