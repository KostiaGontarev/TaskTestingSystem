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
using System.Windows.Shapes;

namespace TTS.UI
{
	/// <summary>
	/// Interaction logic for EditIO.xaml
	/// </summary>
	public partial class EditIO : Window
    {
        #region Members
        public string Text;
        #endregion

        #region Constructors
        public EditIO(string Text)
		{
			this.InitializeComponent();
            this.Text = Text;
            this.Text_RichTextBox.AppendText(this.Text);
        }
        #endregion

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(Text_RichTextBox.Document.ContentStart,
                Text_RichTextBox.Document.ContentEnd);
            this.Text = textRange.Text;
            this.Close();
        }
    }
}