using System.Windows.Controls;
using System.Windows.Media;
using TTS.Core.Abstract.Processing;


namespace TTS.UI.UserControls
{
    public partial class TestIndicator : UserControl
    {
        #region Data Members
        private readonly ITest test;
        #endregion

        #region Properties
        public ITest Test
        {
            get { return this.test; }
        }
        #endregion

        #region Constructors
        private TestIndicator()
        {
            this.InitializeComponent();
        }
        public TestIndicator(ITest test, string title)
            : this()
        {
            this.test = test;
            this.Test.TestingFinished += Test_TestingFinished;
            this.TestNameLabel.Content = title;
        }
        #endregion

        #region Event Handlers
        private void Test_TestingFinished(object sender, System.EventArgs e)
        {
            this.Indicator.Fill = new SolidColorBrush(new Color{R = 0, G = 0, B = 0});
        }
        #endregion

        #region Members

        public void TestStarted()
        {
            this.Indicator.Fill = new SolidColorBrush(new Color { R = 50, G = 100, B = 100 });            
        }
        #endregion
    }
}