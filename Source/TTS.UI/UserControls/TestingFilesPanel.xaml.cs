using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TTS.UI.UserControls
{
    /// <summary>
    /// Interaction logic for TestingFilesPanel.xaml
    /// </summary>
    public partial class TestingFilesPanel : UserControl
    {

        #region Constructors
        public TestingFilesPanel()
        {
            InitializeComponent();
        }
        #endregion

        public void AddItem(string filePath)
        {
            TestingFileControl testingFileControl = new TestingFileControl(filePath);
            this.TestingFilesStackPanel.Children.Add(testingFileControl);
        }
    }
}
