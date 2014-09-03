using System;
using System.Diagnostics;
using System.IO;

using TTS.Core.Abstract.Model;

namespace TTS.Core.Concrete.Processing
{
    internal class IOTestController
    {
        #region Data Members
        private Process process;
        private ITestInfo testInfo;

        private string inputPath;
        private string outputPath;
        private bool? result;
        #endregion

        #region Properties
        public bool? Result
        {
            get { return this.result; }
        }
        public Process Process
        {
            get { return this.process; }
            set
            {
                this.SetupProcess(value);
            }
        }
        public ITestInfo TestInfo { get; set; }
        public bool IsReady
        {
            get { return this.Process != null && this.TestInfo != null; }
        }
        #endregion

        #region Constructors
        private IOTestController() { }
        #endregion

        #region Events
        public event EventHandler InputInjected;
        public event EventHandler ProcessExecuted;
        public event EventHandler OutputChecked;
        #endregion

        #region Event Invokators
        protected virtual void OnInputInjected()
        {
            EventHandler handler = InputInjected;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        protected virtual void OnProcessExecuted()
        {
            EventHandler handler = ProcessExecuted;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        protected virtual void OnOutputChecked()
        {
            EventHandler handler = OutputChecked;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion

        #region Static Members
        public static readonly IOTestController Instance = new IOTestController();
        #endregion

        #region Members
        public void Start()
        {
            try
            {
                if (!this.IsReady)
                    throw new InvalidOperationException("The controller is not ready!");
                this.PrepareInput();
                this.PerformProcess();
                this.CheckOutput();
            }
            catch (Exception exc)
            {
                throw new Exception("The test was interrupted by error!", exc);
            }
        }
        public void Reset()
        {
            this.process = null;
            this.testInfo = null;
            this.inputPath = null;
            this.outputPath = null;
            this.result = null;
        }
        #endregion

        #region Assistants
        private void PrepareInput()
        {
            File.WriteAllText(this.inputPath, this.TestInfo.Input);
            this.OnInputInjected();
        }
        private void PerformProcess()
        {
            this.Process.Start();
            this.Process.WaitForExit();
            this.OnProcessExecuted();
        }
        private void CheckOutput()
        {
            string output = File.ReadAllText(this.outputPath);
            this.result = output.Equals(this.testInfo.Output);
            this.OnOutputChecked();
        }

        private void SetupProcess(Process process)
        {
            try
            {
                if (File.Exists(process.StartInfo.FileName))
                {
                    this.process = process;
                    this.ConstructIOFilePath();
                }
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentException("The process startinfo is not setted!");
            }
            catch (Exception exc)
            {
                throw new ArgumentException("The process is incorrect!", exc);
            }
        }

        private void ConstructIOFilePath()
        {
            string execPath = this.Process.StartInfo.FileName;
            string path = execPath.Substring(execPath.LastIndexOf("\\", System.StringComparison.Ordinal));
            this.inputPath = path + "\\input.txt";
            this.outputPath = path + "\\output.txt";
        }
        #endregion
    }
}
