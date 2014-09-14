using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

using TTS.Core.Abstract.Model;

namespace TTS.Core.Concrete.Processing
{
    internal class IOTest
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
        public ITestInfo TestInfo
        {
            get { return this.testInfo; }
            set
            {
                if (value != null)
                    this.testInfo = value;
            }
        }

        public bool IsReady
        {
            get { return this.Process != null && this.TestInfo != null; }
        }
        #endregion

        #region Constructors
        private IOTest() { }
        #endregion

        #region Static Members
        public static readonly IOTest Instance = new IOTest();
        #endregion

        #region Members
        public void Start(BackgroundWorker worker, DoWorkEventArgs args)
        {
            try
            {
                if (!this.IsReady)
                    throw new InvalidOperationException("The controller is not ready!");
                if (worker.CancellationPending)
                {
                    args.Cancel = true;
                    return;
                }
                this.PrepareInput();
                worker.ReportProgress(30);
                if (worker.CancellationPending)
                {
                    args.Cancel = true;
                    return;
                }
                this.PerformProcess();
                worker.ReportProgress(60);
                if (worker.CancellationPending)
                {
                    args.Cancel = true;
                    return;
                }
                this.CheckOutput();
                worker.ReportProgress(90);
                if (worker.CancellationPending)
                {
                    args.Cancel = true;
                    return;
                }
                this.DeleteFiles();
                worker.ReportProgress(95);
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
        }
        private void PerformProcess()
        {
            this.Process.Start();
            this.Process.WaitForExit();
        }
        private void CheckOutput()
        {
            try
            {
                string output = File.ReadAllText(this.outputPath);
                this.result = output.Equals(this.TestInfo.Output);
            }
            catch (Exception exc)
            {
                if (!this.result.HasValue)
                    this.result = false;
            }
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
            string path = execPath.Substring(0, execPath.LastIndexOf("\\", System.StringComparison.Ordinal));
            this.inputPath = path + "\\input.txt";
            this.outputPath = path + "\\output.txt";
        }
        private void DeleteFiles()
        {
            File.Delete(this.inputPath);
            File.Delete(this.outputPath);
        }
        #endregion
    }
}
