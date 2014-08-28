using System.Windows;

using TTS.Core.Abstract.Model;


namespace TTS.UI.Forms
{
    public partial class TaskCheckWindow : Window
    {
        private ITask task;

        public ITask Task
        {
            get { return this.task; }
        }

        public TaskCheckWindow()
        {
            this.InitializeComponent();
        }

        public TaskCheckWindow(ITask task)
            : this()
        {
            this.task = task;
        }

    }
}