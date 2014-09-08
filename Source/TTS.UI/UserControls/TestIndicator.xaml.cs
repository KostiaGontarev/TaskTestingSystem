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
            this.TestNameLabel.Content = title;
        }
        #endregion

        #region Event Handlers

        #endregion
    }
}