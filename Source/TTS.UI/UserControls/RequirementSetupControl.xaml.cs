using System.Windows.Controls;
using System.Collections.Generic;

using TTS.Core.Abstract.Declarations;

namespace TTS.UI.UserControls
{
	public partial class RequirementSetupControl : UserControl
    {
        private List<string> requirements;
        private CharacteristicType type;

        #region Constructors
        public RequirementSetupControl()
		{
			this.InitializeComponent();
            requirements = new List<string>();
        }
        #endregion

        private void RequirementTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((string)RequirementTypeComboBox.SelectedItem)
            {
                case "Использование памяти":
                    type = CharacteristicType.MaxMemoryUsage;
                    break;
                case "Использование CPU": 
                    type = CharacteristicType.MaxCPUUsage; 
                    break;
                case "Время выполнения": 
                    type = CharacteristicType.MaxExecutionTime; 
                    break;
            }
        }
    }
}