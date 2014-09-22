using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using TTS.Core;
using TTS.Core.Interfaces.Model;
using TTS.UI.Windows;


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
                this.testInfo.Input = this.InputTextBox.Text;
                this.testInfo.Output = this.OutputTextBox.Text;
                return this.testInfo;
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
            this.testInfo = CoreAccessor.CreateTestInfo();
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
            this.InputTextBox.Text = this.ioEditDialog.Text;
        }
        private void OutputTextBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ioEditDialog = new TextEditDialog(this.OutputTextBox.Text);
            this.ioEditDialog.ShowDialog();
            this.OutputTextBox.Text = this.ioEditDialog.Text;
        }
        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OnDeleteButtonClick();
        }
        #endregion

        #region Event Invokers
        private void OnDeleteButtonClick()
        {
            EventHandler handler = this.DeleteButtonClick;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion
    }
}