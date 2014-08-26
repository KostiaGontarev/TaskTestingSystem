using System;
using System.Diagnostics;
using System.IO;
using TTS.Core.Abstract.Declarations;
using TTS.Core.Abstract.Model;
using TTS.Core.Concrete.Model;

namespace TTS.Core.Concrete.Processing
{
    internal class IOCTestPerformer
    {
        #region Data Members
        private ICharacteristic result;
        private ITestInfo testInfo;
        private string inputPath;
        private string outputPath;
        #endregion

        #region Properties
        public Process Process { get; private set; }
        public ICharacteristic Result
        {
            get
            {
                if (!this.IsSettedUp)
                    return null;

                return this.result;
            }
        }
        public bool IsPassed
        {
            get { return (bool)this.result.Value; }
        }
        public bool IsSettedUp
        {
            get
            {
                return (this.testInfo != null) && (this.Process != null);
            }
        }
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
                this.PrepareInput();
                this.Process.Start();
                this.Process.WaitForExit();
                this.CheckOutput();
            }
            catch (Exception exc)
            {
                throw new Exception("The test was interrupted by error!", exc);
            }
        }
        #endregion

        #region Assistants
        private void PrepareInput()
        {
            File.WriteAllText(this.inputPath, this.testInfo.Input);
        }
        private void CheckOutput()
        {
            string output = File.ReadAllText(this.outputPath);
            if (output.Equals(this.testInfo.Output))
            {
                this.result.Value = true;
            }
        }

        private void SetupRequirement(ITestInfo testInfo)
        {
            this.testInfo = testInfo;
            this.result = new Characteristic
            {
                Type = CharacteristicType.InputOutputCompliance,
                Value = false
            };
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
