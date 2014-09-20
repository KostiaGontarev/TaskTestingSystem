using System.Windows;

using Microsoft.Win32;

using TTS.Core;
using TTS.Core.Interfaces.Controllers;
using TTS.Core.Interfaces.Model;


namespace TTS.UI.Forms
{
	public partial class TaskIndexWindow : Window
    {
        #region Data Members
        private TaskEditWindow taskEditWindow;
        private TaskCheckWindow taskCheckWindow;

	    private readonly ITaskController controller;
        #endregion

        #region Constructors
        public TaskIndexWindow()
		{
			this.InitializeComponent();
            this.controller = CoreAccessor.GetTaskController();
            this.TasksList.ItemsSource = this.controller.Tasks;
		}
        #endregion

        #region Event Handlers
        private void CheckButton_OnClick(object sender, RoutedEventArgs e)
        {
            if ((this.controller.Tasks.Count == 0))
            {
                MessageBox.Show("Список задач пуст.", "Ошибка!");
            }
            else if (this.TasksList.SelectedItem == null)
            {
                MessageBox.Show("Вы не выбрали задачу для проверки.", "Ошибка!");
            }
            else
            {
                this.OpenTaskCheckWindow();
            }
        }
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenTaskEditWindow();
        }
        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            if ((this.controller.Tasks.Count == 0))
            {
                MessageBox.Show("Список задач пуст.", "Ошибка!");
            }
            else if (this.TasksList.SelectedItem == null)
            {
                MessageBox.Show("Элемент для редактирования не выбран.", "Ошибка!");
            }
            else
            {
                this.OpenTaskEditWindow(this.TasksList.SelectedItem as ITask);
            }
        }
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if ((this.controller.Tasks.Count == 0))
            {
                MessageBox.Show("Список задач пуст.", "Ошибка!");
            }
            else if (this.TasksList.SelectedItem == null)
            {
                MessageBox.Show("Элемент для удаления не выбран.", "Ошибка!");
            }
            else
            {
                ITask task = this.TasksList.SelectedItem as ITask;
                this.controller.Tasks.Remove(task);
                this.TasksList.Items.Refresh();
            }
        }
        private void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openTasksDialog = new OpenFileDialog
            {
                DefaultExt = ".tts",
                Filter = "StorageFile (.tts)|*.tts"
            };

            if (openTasksDialog.ShowDialog() == true)
            {
                this.controller.LoadFrom(openTasksDialog.FileName);
                this.TasksList.Items.Refresh();
            }
        }
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveTasksDialog = new SaveFileDialog
            {
                FileName = "Storage",
                DefaultExt = ".xml",
                Filter = "StorageFile (.tts)|*.tts"
            };

            if (saveTasksDialog.ShowDialog() == true)
            {
                this.controller.WriteTo(saveTasksDialog.FileName);
                this.TasksList.Items.Refresh();
            }
        }
        private void ExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
        private void AboutButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Assistance
        private void OpenTaskEditWindow()
        {
            this.taskEditWindow = new TaskEditWindow();
            this.taskEditWindow.ShowDialog();
            this.TasksList.Items.Refresh();
        }
        private void OpenTaskEditWindow(ITask task)
        {
            this.taskEditWindow = new TaskEditWindow(task);
            this.taskEditWindow.ShowDialog();
            this.TasksList.Items.Refresh();
        }
        private void OpenTaskCheckWindow()
        {
            ITask task = this.TasksList.SelectedItem as ITask;
            if (task != null)
            {
                this.taskCheckWindow = new TaskCheckWindow(task);
                this.taskCheckWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, добавьте файлы для проверки", "Нет файлов для проверки");
            }
        }
	    #endregion
    }
}