using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

using TTS.Core;
using TTS.Core.Arguments;
using TTS.Core.Interfaces.Controllers;


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
        private readonly Guid testId;
        private bool? indicatorState;
        #endregion
        
        #region Properties
        public Guid TestId
        {
            get { return this.testId; }
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
        public TestIndicator(Guid testId, string title)
            : this()
        {
            this.testId = testId;
            this.TestNameLabel.Content = title;
        }
        #endregion

        #region Members
        public void SubscribeToController()
        {
            ITestController controller = CoreAccessor.GetTestController();
            controller.TestStarted += this.controller_TestStarted;
            controller.TestFinished += this.controller_TestFinished;
            controller.ProgressChanged += this.controller_ProgressChanged;
        }
        public void UnsubscribeFromController()
        {
            ITestController controller = CoreAccessor.GetTestController();
            controller.TestStarted -= this.controller_TestStarted;
            controller.TestFinished -= this.controller_TestFinished;
            controller.ProgressChanged -= this.controller_ProgressChanged;
        }

        public void Reset()
        {
            this.Indicator.Fill = TestIndicator.IndicatorDefaultColor;
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