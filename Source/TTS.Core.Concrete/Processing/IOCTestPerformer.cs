using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
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
        private Thread executionThread;
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
                this.executionThread = new Thread(this.TestProcess);
                this.executionThread.Start();
            }
            catch (Exception exc)
            {
                throw new Exception("The test was interrupted by error!", exc);
            }
        }
        #endregion

        #region Assistants
        private void TestProcess()
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
        private void PrepareInput()
        {
            string execPath = Process.StartInfo.FileName;
            string path = execPath.Substring(execPath.LastIndexOf("\\", System.StringComparison.Ordinal));
            string inputPath = path + "input.txt";
            FileInfo input = new FileInfo(inputPath);
            using (FileStream stream = input.Create())
            {
                byte[] buffer = Encoding.Unicode.GetBytes(this.testInfo.Input);
                stream.Write(buffer, 0, buffer.Length);
            }
        }
        private void CheckOutput()
        {
            string execPath = Process.StartInfo.FileName;
            string path = execPath.Substring(execPath.LastIndexOf("\\", System.StringComparison.Ordinal));
            string outputPath = path + "output.txt";
            string output = File.ReadAllText(outputPath);

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
                    this.Process = process;
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
        #endregion
    }
}
