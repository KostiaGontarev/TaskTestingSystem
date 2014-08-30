using System.Windows.Controls;

using TTS.Core.Abstract.Model;


namespace TTS.UI.UserControls
{
	public partial class TestIndicator : UserControl
    {
        #region Data Members
        private readonly ITestInfo testInfo;
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
            TestNameLabel.Content = title;
        }
        #endregion
    }
}