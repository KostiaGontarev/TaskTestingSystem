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

using TTS.Core.Abstract.Declarations;

namespace TTS.UI.UserControls
{
	/// <summary>
	/// Interaction logic for Requirements.xaml
	/// </summary>
	public partial class Requirements : UserControl
	{
        private List<string> requirements;
        private CharacteristicType type;

		public Requirements()
		{
			this.InitializeComponent();
            requirements = new List<string>();
            requirements.Add("");
        }

        private void RequirementComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((string)RequirementComboBox.SelectedItem)
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