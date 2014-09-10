using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace TTS.UI.UserControls
{
    /// <summary>
    /// Interaction logic for Exec_file.xaml
    /// </summary>
    public partial class TestingFileControl : UserControl
    {
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
                    this.LayoutRoot.Background = new SolidColorBrush(new Color
                    {
                        A = 255,
                        R = 95,
                        G = 158,
                        B = 160,
                    });
                }
                else
                {
                    this.LayoutRoot.Background = new SolidColorBrush(new Color
                    {
                        A = 255,
                        R = 255,
                        G = 255,
                        B = 255,
                    });
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler DeleteButtonClick;
        public event EventHandler ElementSelected;
        #endregion

        #region Constructors
        public TestingFileControl()
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

        #region EventHandlers
        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnDeleteButtonClick();

        }
        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Selected = true;
            this.OnElementSelected();
        }
        #endregion

        #region Event Invokers
        private void OnDeleteButtonClick()
        {
            EventHandler handler = DeleteButtonClick;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        protected virtual void OnElementSelected()
        {
            EventHandler handler = ElementSelected;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion
    }
}