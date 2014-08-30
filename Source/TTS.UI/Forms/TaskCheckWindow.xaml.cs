using System.Windows;

using Microsoft.Win32;

using TTS.Core.Abstract.Model;

using TTS.UI.UserControls;


namespace TTS.UI.Forms
{
    public partial class TaskCheckWindow : Window
    {
        #region Data Members
        private readonly ITask task;
        #endregion

        #region Constructors
        private TaskCheckWindow()
        {
            this.InitializeComponent();
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
            openFileDialog.Filter = "exe files (*.exe)|*.exe";
            if (openFileDialog.ShowDialog() == DialogResult.Equals(true))
            {
                
            }
        }
        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
        private void CheckCurrentButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
        private void CheckAllButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Assistants
        private void SetupTests()
        {
            const string title = "Тест №";
            int index = 1;
            foreach (ITestInfo testInfo in this.task.Tests)
            {
                TestIndicator testIndicator = new TestIndicator(testInfo, title + index);
                this.TestsListBox.Items.Add(testIndicator);
                index++;
            }
        } 
        #endregion
    }
}