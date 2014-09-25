using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

using TTS.Core.Interfaces.Model;


namespace TTS.Core.Processing
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
                this.result = null;
            }
        }
        public ITestInfo TestInfo
        {
            get { return this.testInfo; }
            set
            {
                if (value != null)
                {
                    this.testInfo = value;
                    this.result = null;
                }
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

                this.PrepareInput(worker);
                this.PerformProcess(worker);
                this.CheckOutput(worker);
                this.DeleteFiles(worker);
            }
            catch (Exception exc)
            {
                throw new Exception("The test was interrupted by error!", exc);
            }
        }
        #endregion

        #region Assistants
        private void PrepareInput(BackgroundWorker worker)
        {
            try
            {
                File.WriteAllText(this.inputPath, this.TestInfo.Input);
            }
            finally
            {
                worker.ReportProgress(30);
            }
        }
        private void PerformProcess(BackgroundWorker worker)
        {
            try
            {
                this.Process.Start();
                this.Process.WaitForExit();
            }
            finally
            {
                worker.ReportProgress(60);
            }
        }
        private void CheckOutput(BackgroundWorker worker)
        {
            try
            {
                string output = File.ReadAllText(this.outputPath);
                this.result = output.Equals(this.TestInfo.Output);
            }
            catch (Exception)
            {
                if (!this.result.HasValue)
                    this.result = false;
            }
            finally
            {
                worker.ReportProgress(90);                
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
        private void DeleteFiles(BackgroundWorker worker)
        {
            try
            {
                File.Delete(this.inputPath);
                File.Delete(this.outputPath);
            }
            finally
            {
                worker.ReportProgress(95);
            }
        }
        #endregion
    }
}
