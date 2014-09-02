using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

using Microsoft.Win32;

using TTS.Core.Abstract.Controllers;
using TTS.Core.Abstract.Model;

using TTS.Core.Concrete;


namespace TTS.UI.Forms
{
	public partial class MainWindow : Window
    {
        #region Data Members
	    private readonly ITaskController controller;
        private TaskEditWindow taskEditWindow;
        private TaskCheckWindow taskCheckWindow;
        private string tasksFilePath;
        #endregion

        #region Constructors
        public MainWindow()
		{
			this.InitializeComponent();
            this.controller = CoreAccessor.GetTaskController();
            TasksList.ItemsSource = this.controller.Tasks;
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
                this.TasksList.Items.Remove(this.TasksList.SelectedItem);
            }
        }
        private void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openTasksDialog = new OpenFileDialog();
            openTasksDialog.DefaultExt = ".json";
            openTasksDialog.Filter = "Documents (.json)|*.json";

            if (openTasksDialog.ShowDialog() == true)
            {
                tasksFilePath = openTasksDialog.FileName;
                controller.LoadFrom(tasksFilePath);
                TasksList.Items.Refresh();
            }
        }
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveTasksDialog = new SaveFileDialog();
            saveTasksDialog.FileName = "Document";
            saveTasksDialog.DefaultExt = ".json";
            saveTasksDialog.Filter = "Documents (.json)|*.json";

            if (saveTasksDialog.ShowDialog() == true)
            {
                tasksFilePath = saveTasksDialog.FileName;
                controller.WriteTo(tasksFilePath);
            }
        }
        #endregion

        #region Assistance
        private void OpenTaskEditWindow()
        {
            this.taskEditWindow = new TaskEditWindow();
            this.taskEditWindow.ShowDialog();

            ITask task = this.taskEditWindow.Task;
            if (task != null)
            {
                controller.Tasks.Add(task);
                TasksList.Items.Refresh();
                this.TasksList.SelectedItem = task;
            }
        }
        private void OpenTaskEditWindow(ITask task)
        {
            this.taskEditWindow = new TaskEditWindow(task);
            this.taskEditWindow.ShowDialog();

            this.TasksList.SelectedItem = this.taskEditWindow.Task;
            this.TasksList.Items.Refresh();
        }
        private void OpenTaskCheckWindow()
        {
            ITask task = this.TasksList.SelectedItem as ITask;
            if (task.Tests.Count == 0)
            {
                MessageBox.Show("Пожалуйста, добавьте файлы для проверки", "Нет файлов для проверки");
            }
            else
            {
                this.taskCheckWindow = new TaskCheckWindow(task);
                this.taskCheckWindow.ShowDialog();
            }
        }
        #endregion
    }
}