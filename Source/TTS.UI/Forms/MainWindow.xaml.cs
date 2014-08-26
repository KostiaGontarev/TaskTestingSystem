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
using TTS.Core.Abstract.Model;
using TTS.Core.Concrete.Model;

namespace TTS.UI
{
	public partial class MainWindow : Window
    {
        #region Data Members
        private EditTask wEditTask;
        private TaskCheck wTaskCheck;
        #endregion

        #region Constructors
        public MainWindow()
		{
			this.InitializeComponent();
		}
        #endregion

        #region Event Handlers
        private void CheckButtonClick(object sender, RoutedEventArgs e)
        {
            if ((TasksList.Items.Count == 0))
            {
                MessageBox.Show("Добавьте какое-либо задание перед проверкой.", "Список заданий пуст");
            }
            else if (TasksList.SelectedItem == null)
            {
                MessageBox.Show("Выберите задание для проверки.", "Ни один элемент не выбран");
            }
            else
            {
                TaskCheckOpen((ITask)TasksList.SelectedItem);
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            EditTaskOpen();
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            if ((TasksList.Items.Count == 0))
            {
                MessageBox.Show("Добавьте какое-либо задание перед редактированием.", "Список заданий пуст");
            }
            else if (TasksList.SelectedItem == null)
            {
                MessageBox.Show("Выберите задание для редактирования.", "Ни один элемент не выбран");
            }
            else
            {
                EditTaskOpen((ITask)TasksList.SelectedItem);
            }
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void TasksListDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditTaskOpen((ITask)TasksList.SelectedItem);
        }
        #endregion

        #region Assistance
        private void EditTaskOpen()
        {
            wEditTask = new EditTask();
            wEditTask.ShowDialog();
            if (wEditTask.Task != null)
            {
                TasksList.Items.Add(wEditTask.Task);
            }
        }

        private void EditTaskOpen(ITask task)
        {
            wEditTask = new EditTask(task);
            wEditTask.ShowDialog();
            TasksList.SelectedItem = wEditTask.Task;
            TasksList.Items.Refresh();
        }

        private void TaskCheckOpen(ITask task)
        {
            wTaskCheck = new TaskCheck(task);
            wTaskCheck.ShowDialog();
        }
        #endregion
    }
}