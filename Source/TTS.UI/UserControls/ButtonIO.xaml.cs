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
        #endregion

        #region Constructors
        public buttonIO()
        {
            this.InitializeComponent();
        }

        public buttonIO(string input, string output)
            : this()
        {
            this.Input_TextBox.Text = input;
            this.Output_TextBox.Text = output;
        }
        #endregion

        #region Events
        private void Input_File_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            content = Input_TextBox.Text;
            ioEditInputOutput = new IOEditDialog(content);
            ioEditInputOutput.ShowDialog();
            Input_TextBox.Text = ioEditInputOutput.Text;
        }

        private void Output_File_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            content = Output_TextBox.Text;
            ioEditInputOutput = new IOEditDialog(content);
            ioEditInputOutput.ShowDialog();
            Output_TextBox.Text = ioEditInputOutput.Text;
        }

        private void Delete_Files_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Delete != null)
            {
                Delete(this);
            }
        }
        #endregion
    }
}