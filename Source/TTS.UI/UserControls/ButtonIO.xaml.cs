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
using System.Windows.Navigation;
using System.Windows.Shapes;

using TTS.UI.Forms;
using TTS.Core.Abstract.Model;
using TTS.Core.Concrete;

namespace TTS.UI.UserControls
{
    /// <summary>
    /// Interaction logic for buttonIO.xaml
    /// </summary>
    public partial class buttonIO : UserControl
    {
        #region Members
        private IOEditDialog ioEditInputOutput;
        private string content;
        public Action<buttonIO> Delete;
        private ITestInfo testInfo;
        private string input;
        private string output;
        #endregion

        public ITestInfo TestInfo
        {
            get
            {
                return testInfo;
            }
        }

        #region Constructors
        public buttonIO()
        {
            this.InitializeComponent();
            this.input = "";
            this.output = "";
        }

        public buttonIO(string input, string output)
            : this()
        {
            this.input = input;
            this.output = output;
            this.InputTextBox.Text = input;
            this.OutputTextBox.Text = output;
        }
        #endregion

        #region Events
        private void InputFile_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            content = InputTextBox.Text;
            ioEditInputOutput = new IOEditDialog(content);
            ioEditInputOutput.ShowDialog();
            input = ioEditInputOutput.Text;
            InputTextBox.Text = input;
        }

        private void OutputFile_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            content = OutputTextBox.Text;
            ioEditInputOutput = new IOEditDialog(content);
            ioEditInputOutput.ShowDialog();
            output = ioEditInputOutput.Text;
            OutputTextBox.Text = output;
        }

        public void SaveTestInfo()
        {
            if (!String.IsNullOrWhiteSpace(input) || !String.IsNullOrWhiteSpace(output))
            {
                testInfo = CoreAccessor.CreateTest();
                testInfo.Input = this.input;
                testInfo.Output = this.output;
            }
        }

        private void DeleteFiles_OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (Delete != null)
            {
                Delete(this);
            }
        }
        #endregion
    }
}