using System.Collections.Generic;
using System.Windows.Controls;


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
            List<string> files = new List<string>();
            foreach (TestingFileControl control in this.FilesPanel.Children)
            {
                files.Add(control.FilePath);
            }
            return files;
        } 
        #endregion
    }
}
