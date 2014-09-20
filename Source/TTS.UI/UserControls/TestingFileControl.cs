using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace TTS.UI.UserControls
{
    public partial class TestingFileControl : UserControl
    {
        #region ReadOnly Members
        private static readonly SolidColorBrush SelectedFile = new SolidColorBrush(new Color { A = 255, R = 95, G = 158, B = 160, });
        private static readonly SolidColorBrush DefaultFile = new SolidColorBrush(new Color { A = 255, R = 255, G = 255, B = 255 });
        #endregion
        
        #region Data Members
        private readonly string filePath;
        private bool selected;
        #endregion

        #region Properties
        public string FilePath
        {
            get { return this.filePath; }
        }
        public bool Selected
        {
            get { return this.selected; }
            set
            {
                this.selected = value;

                if (this.Selected)
                {
                    this.LayoutRoot.Background = TestingFileControl.SelectedFile;
                    this.OnElementSelected();
                }
                else
                    this.LayoutRoot.Background = TestingFileControl.DefaultFile;
            }
        }
        #endregion

        #region Events
        public event EventHandler DeleteButtonClick;
        public event EventHandler ElementSelected;
        #endregion

        #region Constructors
        private TestingFileControl()
        {
            this.InitializeComponent();
        }

        public TestingFileControl(string filePath)
            : this()
        {
            this.filePath = filePath;
            this.FilePathLabel.Content = this.FilePath;
            this.FilePathLabel.ToolTip = this.FilePath;
        }
        #endregion

        #region Event Handlers
        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnDeleteButtonClick();
        }
        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Selected = true;
        }
        #endregion

        #region Event Invokers
        private void OnDeleteButtonClick()
        {
            EventHandler handler = this.DeleteButtonClick;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        protected virtual void OnElementSelected()
        {
            EventHandler handler = this.ElementSelected;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion
    }
}