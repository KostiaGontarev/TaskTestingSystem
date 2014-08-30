using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using TTS.Core.Abstract.Model;

using TTS.Core.Concrete;

using TTS.UI.Forms;


namespace TTS.UI.UserControls
{
    public partial class IOSetupControl : UserControl
    {
        #region Data Members
        private TextEditDialog ioEditDialog;
        private readonly ITestInfo testInfo;
        #endregion

        #region Properties
        public ITestInfo TestInfo
        {
            get
            {
                return testInfo;
            }
        } 
        #endregion

        #region Events
        public event EventHandler DeleteButtonClick;
        #endregion

        #region Constructors
        public IOSetupControl()
        {
            this.InitializeComponent();
            this.testInfo = CoreAccessor.CreateTest();
        }
        public IOSetupControl(ITestInfo testInfo)
            : this()
        {
            this.testInfo = testInfo;
            this.InputTextBox.Text = this.testInfo.Input;
            this.OutputTextBox.Text = this.testInfo.Output;
        }
        #endregion

        #region Event Handlers
        private void InputTextBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ioEditDialog = new TextEditDialog(this.InputTextBox.Text);
            this.ioEditDialog.ShowDialog();
            this.testInfo.Input = this.ioEditDialog.Text;
            this.InputTextBox.Text = this.testInfo.Input;
        }
        private void OutputTextBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ioEditDialog = new TextEditDialog(this.OutputTextBox.Text);
            this.ioEditDialog.ShowDialog();
            this.testInfo.Output = this.ioEditDialog.Text;
            this.OutputTextBox.Text = this.testInfo.Output;
        }
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