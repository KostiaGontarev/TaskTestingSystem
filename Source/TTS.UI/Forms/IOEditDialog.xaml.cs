using System;
using System.Windows;
using System.Windows.Documents;


namespace TTS.UI.Forms
{
	public partial class IOEditDialog : Window
    {
        #region Data Members
	    private string text;
        #endregion

        #region Properties
	    public string Text
	    {
	        get { return this.text; }
	    }
        #endregion

        #region Constructors
        public IOEditDialog(string text)
		{
			this.InitializeComponent();
            this.text = text;
            this.TextRichTextBox.AppendText(this.Text);
        }
        #endregion

        #region Event Handlers
        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(this.TextRichTextBox.Document.ContentStart, 
                this.TextRichTextBox.Document.ContentEnd);
            if (!String.IsNullOrWhiteSpace(textRange.Text))
                this.text = textRange.Text.Replace("\r\n","");

            this.Close();
        } 
        #endregion
    }
}