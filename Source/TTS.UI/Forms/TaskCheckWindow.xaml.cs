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

        public TaskCheck()
        {
            this.InitializeComponent();
        }

        public TaskCheck(ITask task)
            : this()
        {
            this.task = task;
        }

    }
}