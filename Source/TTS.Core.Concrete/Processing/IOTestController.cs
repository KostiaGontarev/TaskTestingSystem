using System;
using System.Diagnostics;
using System.IO;
using TTS.Core.Abstract.Declarations;
using TTS.Core.Abstract.Model;
using TTS.Core.Concrete.Model;

namespace TTS.Core.Concrete.Processing
{
    internal class IOTestController
    {
        #region Data Members
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
        public Process Process { get; private set; }
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
        public void Setup(Process process, ITestInfo testInfo)
        {
            this.SetupRequirement(testInfo);
            this.SetupProcess(process);
        }
        public void Start()
        {
            try
            {
                if (this.Process != null && this.testInfo != null) 
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
            this.testInfo = null;
            this.inputPath = null;
            this.outputPath = null;
            this.result = null;
            this.Process = null;
        }
        #endregion

        #region Assistants
        private void PrepareInput()
        {
            File.WriteAllText(this.inputPath, this.testInfo.Input);
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

        private void SetupRequirement(ITestInfo testInfo)
        {
            this.testInfo = testInfo;
        }
        private void SetupProcess(Process process)
        {
            try
            {
                if (File.Exists(this.Process.StartInfo.FileName))
                {
                    this.Process = process;
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
            this.inputPath = path + "input.txt";
            this.outputPath = path + "output.txt";
        }
        #endregion
    }
}
