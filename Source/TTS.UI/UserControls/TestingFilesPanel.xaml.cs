using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using System.Windows;


namespace TTS.UI.UserControls
{
    /// <summary>
    /// Interaction logic for TestingFilesPanel.xaml
    /// </summary>
    public partial class TestingFilesPanel : UserControl
    {
        #region Constructors
        public TestingFilesPanel()
        {
            InitializeComponent();
        }
        #endregion

        #region EventHandlers
        private void TestingFileControl_DeleteButtonClick(object sender, System.EventArgs e)
        {
            TestingFileControl control = sender as TestingFileControl;
            if (control != null)
            {
                control.DeleteButtonClick -= TestingFileControl_DeleteButtonClick;
                this.FilesPanel.Children.Remove(control);
            }
        }
        #endregion

        #region Assistance
        public void AddItem(string filePath)
        {
            TestingFileControl testingFileControl = new TestingFileControl(filePath);
            testingFileControl.DeleteButtonClick += TestingFileControl_DeleteButtonClick;
            this.FilesPanel.Children.Add(testingFileControl);
        }

        public List<string> GetSelectedFiles()
        {
            return this.FilesPanel.Children.OfType<TestingFileControl>()
                                           .Where(element => element.FileCheckBox.IsChecked == true)
                                           .Select(element => element.FilePath)
                                           .ToList();
        }

        public void SelectAllFiles()
        {
            foreach (TestingFileControl control in this.FilesPanel.Children)
            {
                control.FileCheckBox.IsChecked = true;
            }
        }
        #endregion
    }
}
