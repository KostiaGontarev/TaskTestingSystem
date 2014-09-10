using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using TTS.Core.Abstract.Controllers;
using TTS.Core.Abstract.Model;
using TTS.Core.Concrete;
using TTS.Core.Concrete.Processing;


namespace TTS.UI.UserControls
{
    public partial class TestIndicator : UserControl
    {
        #region Static Members
        private static readonly SolidColorBrush IndicatorDefaultColor = new SolidColorBrush(new Color { A = 255, R = 255, G = 255, B = 255 });
        private static readonly SolidColorBrush TestInProgress = new SolidColorBrush(new Color { A = 255, R = 255, G = 215, B = 0 });
        private static readonly SolidColorBrush TestSuccessfullyPassed = new SolidColorBrush(new Color{A = 255,R = 173,G = 255,B = 47});
        private static readonly SolidColorBrush TestFailed = new SolidColorBrush(new Color { A = 255, R = 255, G = 0, B = 0 });
        #endregion

        #region Data Members
        private readonly ITestInfo testInfo;
        private bool? indicatorState;
        #endregion
        
        #region Properties
        public ITestInfo TestInfo
        {
            get { return this.testInfo; }
        }
        public bool? IndicatorState
        {
            get { return this.indicatorState; }
            set
            {
                this.indicatorState = value;
                this.Indicator.Fill = value.HasValue
                    ? (value.Value ? TestSuccessfullyPassed : TestFailed)
                    : TestInProgress;
            }
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
        public void SubscribeToController()
        {
            ITestController controller = CoreAccessor.GetTestController();
            controller.TestStarted += controller_TestStarted;
            controller.TestFinished += controller_TestFinished;
            controller.ProgressChanged += controller_ProgressChanged;
        }
        public void UnsubscribeFromController()
        {
            ITestController controller = CoreAccessor.GetTestController();
            controller.TestStarted -= controller_TestStarted;
            controller.TestFinished -= controller_TestFinished;
            controller.ProgressChanged -= controller_ProgressChanged;
        }

        public void Reset()
        {
            this.Indicator.Fill = IndicatorDefaultColor;
            this.TestProgressBar.Value = 0;
        }
        #endregion

        #region Event Handlers
        private void controller_TestStarted(object sender, EventArgs e)
        {
            this.IndicatorState = null;
        }
        private void controller_TestFinished(object sender, EventArgs e)
        {
            BoolResultEventArgs args = e as BoolResultEventArgs;
            if (args != null)
            {
                this.IndicatorState = args.Result;
            }
        }
        private void controller_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.TestProgressBar.Value = e.ProgressPercentage;
        }
        #endregion
    }
}