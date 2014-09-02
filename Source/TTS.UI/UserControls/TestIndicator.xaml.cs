using System.Windows.Controls;

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
            this.test.InputInjected += test_InputInjected;
            this.test.ProcessExecuted += test_ProcessExecuted;
            this.test.OutputChecked += test_OutputChecked;
            this.test.RequirementsChecked += test_RequirementsChecked;

            this.TestNameLabel.Content = title;
            this.TestProgressBar.Value = 0;
        }
        #endregion

        #region Event Handlers
        protected void test_InputInjected(object sender, System.EventArgs e)
        {
            this.TestProgressBar.Value += 25;
        }
        protected void test_ProcessExecuted(object sender, System.EventArgs e)
        {
            this.TestProgressBar.Value += 25;
        }
        protected void test_OutputChecked(object sender, System.EventArgs e)
        {
            this.TestProgressBar.Value += 25;
        }
        protected void test_RequirementsChecked(object sender, System.EventArgs e)
        {
            this.TestProgressBar.Value += 25;
        }
        #endregion
    }
}