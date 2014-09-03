using System;
using System.Windows;
using System.Windows.Controls;


namespace TTS.UI.UserControls
{
	/// <summary>
	/// Interaction logic for Exec_file.xaml
	/// </summary>
	public partial class TestingFileControl : UserControl
    {
        #region Data Members
        private readonly string filePath;
        #endregion

        #region Properties
        public string FilePath
        {
            get{ return this.filePath;}
        }
        #endregion

        #region Events
        public event EventHandler DeleteButtonClick;
        #endregion

        #region Constructors
        private TestingFileControl()
		{
			this.InitializeComponent();
        }
        public TestingFileControl(string filePath)
            :this()
        {
            this.filePath = filePath;
            this.FilePathLabel.Content = this.FilePath;
            this.FilePathLabel.ToolTip = this.FilePath;
        }
        #endregion

        #region EventHandlers
        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnDeleteButtonClick();
        }
        #endregion

        #region Event Invokers
        private void OnDeleteButtonClick()
        {
            EventHandler handler = DeleteButtonClick;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion

    }
}