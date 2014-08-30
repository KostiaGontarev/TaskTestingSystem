using System.Collections.Generic;
using System.Linq;

using System.Windows.Controls;

using TTS.Core.Abstract.Model;


namespace TTS.UI.UserControls
{
    public partial class IOPanel : UserControl
    {
        #region Constructors
        public IOPanel()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Members
        public void AddItem()
        {
            IOSetupControl control = new IOSetupControl();
            control.DeleteButtonClick += ioSetupControl_DeleteButtonClick;
            this.IOControlsPanel.Children.Add(control);
        }
        public void AddItem(ITestInfo testInfo)
        {
            IOSetupControl control = new IOSetupControl(testInfo);
            control.DeleteButtonClick += ioSetupControl_DeleteButtonClick;
            this.IOControlsPanel.Children.Add(control);
        }
        public List<ITestInfo> GetTestsInfo()
        {
            return this.IOControlsPanel.Children.OfType<IOSetupControl>()
                                                .Select(control => control.TestInfo)
                                                .ToList();
        }
        #endregion

        #region Event Handlers
        private void ioSetupControl_DeleteButtonClick(object sender, System.EventArgs e)
        {
            IOSetupControl control = sender as IOSetupControl;
            if (control != null)
            {
                control.DeleteButtonClick -= ioSetupControl_DeleteButtonClick;
                this.IOControlsPanel.Children.Remove(control);
            }
        }
        #endregion
    }
}