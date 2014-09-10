using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using TTS.Core.Abstract.Controllers;
using TTS.Core.Abstract.Model;


namespace TTS.UI.UserControls
{
    public partial class TestIndicator : UserControl
    {
        #region Data Members
        private ITestInfo testInfo;
        #endregion

        #region Properties
        public ITestInfo TestInfo
        {
            get { return this.testInfo; }
        }
        #endregion

        #region Constructors
        private TestIndicator()
        {
            this.InitializeComponent();
        }
        public TestIndicator(ITestInfo testInfo, string title)
            : this()
        {
            this.testInfo = testInfo;
            this.TestNameLabel.Content = title;
        }
        #endregion

        #region Members
        public void SubscribeTo(ITestController controller)
        {
            controller.TestStarted += controller_TestStarted;
            controller.TestFinished += controller_TestFinished;
            controller.ProgressChanged += controller_ProgressChanged;
        }
        #endregion

        #region Event Handlers
        private void controller_TestStarted(object sender, EventArgs e)
        {
            this.Indicator.Fill = new SolidColorBrush(new Color
            {
                A = 255,
                R = 255,
                G = 215,
                B = 0
            });
        }
        private void controller_TestFinished(object sender, EventArgs e)
        {
            this.Indicator.Fill = new SolidColorBrush(new Color
            {
                A = 255,
                R = 173,
                G = 255,
                B = 47
            });
        }
        private void controller_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.TestProgressBar.Value = e.ProgressPercentage;
        }
        #endregion
    }
}