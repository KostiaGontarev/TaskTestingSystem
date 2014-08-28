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
using TTS.Core.Abstract.Model;

namespace TTS.UI
{
	/// <summary>
	/// Interaction logic for Check.xaml
	/// </summary>
	public partial class Test : UserControl
    {
        #region Members
        private ITestInfo testInfo;
        #endregion

        #region Properties
        public ITestInfo TestInfo { 
            get { return testInfo; } 
        }
        #endregion

        #region Constructors
        public Test()
		{
			this.InitializeComponent();
		}

        public Test(ITestInfo testInfo, int i)
            : this()
        {
            this.testInfo = testInfo;
            TestNameLabel.Content = "Тест №"+ i.ToString();
        }
        #endregion
    }
}