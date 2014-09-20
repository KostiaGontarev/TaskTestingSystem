using System;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;


namespace TTS.UI.UserControls
{
    public partial class TestingFilesPanel : UserControl
    {
        #region Constructors
        public TestingFilesPanel()
        {
            InitializeComponent();
        }
        #endregion

        #region Propeties
        public string CurrentFile
        {
            get
            {
                TestingFileControl control = this.FilesPanel.Children.OfType<TestingFileControl>()
                    .SingleOrDefault(element => element.Selected);
                if (control != null)
                    return control.FilePath;

                return String.Empty;
            }
        }
        #endregion
        
        #region Events
        public event EventHandler SelectionChanged;
        #endregion

        #region Event Invokators
        protected virtual void OnSelectionChanged()
        {
            EventHandler handler = this.SelectionChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion

        #region Event Handlers
        private void TestingFileControl_DeleteButtonClick(object sender, System.EventArgs e)
        {
            TestingFileControl control = sender as TestingFileControl;
            if (control != null)
            {
                control.DeleteButtonClick -= this.TestingFileControl_DeleteButtonClick;
                this.FilesPanel.Children.Remove(control);
            }
        }
        private void testingFileControl_ElementSelected(object sender, System.EventArgs e)
        {
            TestingFileControl selectedControl = sender as TestingFileControl;
            if (selectedControl == null)
                return;

            List<TestingFileControl> resetSelectionList =
                this.FilesPanel.Children.OfType<TestingFileControl>()
                .Where(element => element.Selected)
                .ToList();
            foreach (TestingFileControl control in resetSelectionList)
            {
                if (control.FilePath != selectedControl.FilePath)
                    control.Selected = false;
            }
            this.OnSelectionChanged();
        }
        #endregion

        #region Members
        public void AddItem(string filePath)
        {
            TestingFileControl testingFileControl = new TestingFileControl(filePath);
            testingFileControl.DeleteButtonClick += this.TestingFileControl_DeleteButtonClick;
            testingFileControl.ElementSelected += this.testingFileControl_ElementSelected;
            testingFileControl.Selected = true;

            this.FilesPanel.Children.Add(testingFileControl);
        }
        public List<string> GetSelectedFiles()
        {
            return this.FilesPanel.Children.OfType<TestingFileControl>()
                                           .Where(element => element.FileCheckBox.IsChecked == true)
                                           .Select(element => element.FilePath)
                                           .ToList();
        }
        public List<string> GetFiles()
        {
            return this.FilesPanel.Children.OfType<TestingFileControl>()
                                           .Select(element => element.FilePath)
                                           .ToList();
        }
        public void SelectFile(string fileName)
        {
            TestingFileControl control = this.FilesPanel.Children.OfType<TestingFileControl>()
                .SingleOrDefault(element => element.FilePath == fileName);
            if (control != null)
                control.Selected = true;
        }
        #endregion
    }
}
