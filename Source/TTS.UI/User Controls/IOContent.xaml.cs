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

using TTS.UI.UserControls;
using TTS.Core.Abstract.Model;
using TTS.Core.Concrete;

namespace TTS.UI
{
    /// <summary>
    /// Interaction logic for IOContent.xaml
    /// </summary>
    public partial class IOContent : UserControl
    {
        public IOContent()
        {
            this.InitializeComponent();
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

        public void SaveContentIO(ITask task)
        {
            task.Tests.Clear();
            foreach (UIElement IO in ButtonsIOStackPanel.Children)
            {
                buttonIO button = new buttonIO();
                button = (buttonIO)IO;
                if (button.Input_TextBox.Text != "" || button.Output_TextBox.Text != "")
                {
                    ITestInfo testInfo = CoreAccessor.CreateTest();
                    testInfo.Input = button.Input_TextBox.Text;
                    testInfo.Output = button.Output_TextBox.Text;
                    task.Tests.Add(testInfo);
                }
            }
        }

        private void OnDelete(buttonIO buttonIO)
        {
            ButtonsIOStackPanel.Children.Remove(buttonIO);
        }
    }
}