using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using TTS.Core.Abstract.Model;

using TTS.Core.Concrete;


namespace TTS.UI.UserControls
{
    public partial class IOContent : UserControl
    {
        #region Members
        private List<ITestInfo> listTestInfo;
        #endregion

        public List<ITestInfo> ListTestInfo
        {
            get
            {
                return listTestInfo;
            }
        }

        public IOContent()
        {
            this.InitializeComponent();
            listTestInfo = new List<ITestInfo>();
        }
        public void AddButtonIO()
        {
            buttonIO buttonIO = new buttonIO();
            buttonIO.Delete = OnDelete;
            buttonIO.Delete(buttonIO);
            ButtonsIOStackPanel.Children.Add(buttonIO);
        }

        public void AddButtonIO(string input, string output)
        {
            buttonIO buttonIO = new buttonIO(input, output);
            buttonIO.Delete = OnDelete;
            buttonIO.Delete(buttonIO);
            ButtonsIOStackPanel.Children.Add(buttonIO);
        }

        public void SaveListTestInfo()
        {
            foreach (UIElement IO in ButtonsIOStackPanel.Children)
            {
                buttonIO button = new buttonIO();
                button = (buttonIO)IO;
                button.SaveTestInfo();
                if (button.TestInfo != null)
                {
                    this.listTestInfo.Add(button.TestInfo);
                }
            }
        }

        private void OnDelete(buttonIO buttonIO)
        {
            ButtonsIOStackPanel.Children.Remove(buttonIO);
        }
    }
}