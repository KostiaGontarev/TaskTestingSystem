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
using TTS.Core.Abstract.Model;

namespace TTS.UI
{
	/// <summary>
	/// Interaction logic for Check.xaml
	/// </summary>
	public partial class Test : UserControl
	{
        private ITask task;

        public ITask Task { 
            get { return task; } 
        }

		public Test()
		{
			this.InitializeComponent();
		}

        public Test(ITask task)
            : this()
        {
            this.task = task;
        }
	}
}