using System;
using System.Collections.Generic;
using System.Diagnostics;
using TTS.Core.Abstract.Declarations;
using TTS.Core.Abstract.Model;
using TTS.Core.Abstract.Processing;
using TTS.Core.Concrete.Model;

namespace TTS.Core.Concrete.Processing
{
    internal class Test : ITest
    {
        #region Data Members
        private readonly IOTestController ioTestController = IOTestController.Instance;
        private readonly TestResult result;
        private Process process;
        #endregion

        #region Properties
        public ITestResult Result
        {
            get { return this.result; }
        }
        public Process Process 
        { 
            get { return this.process; }
            set
            {
                if (value != null)
                {
                    this.process = value;
                    this.ioTestController.Process = this.Process;
                }
            } 
        }
        public bool IsReady
        {
            get { return this.Process != null; }
        }
        #endregion

        #region Constructors
        public Test(ITestInfo testInfo)
        {
            this.ioTestController.TestInfo = testInfo;

            this.result = new TestResult(testInfo);
        }
        #endregion

        #region Events
        public event EventHandler TestingFinished;
        #endregion

        #region Members
        public void Run()
        {
            this.PerformIOTest();
            this.SetupResult();
            this.OnTestingFinished();
        }
        #endregion

        #region Event Invokators
        protected virtual void OnTestingFinished()
        {
            EventHandler handler = TestingFinished;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion

        #region Assistants
        private void PerformIOTest()
        {
            this.ioTestController.Start();
            this.result.Requirements.Add(new Characteristic
            {
                Type = CharacteristicType.InputOutputCompliance,
                Value = this.ioTestController.Result
            });
        }

        private void SetupResult()
        {
            ICharacteristic iocTest = new Characteristic
            {
                Type = CharacteristicType.InputOutputCompliance,
                Value = this.ioTestController.Result
            };
            this.result.Requirements.Add(iocTest);
        }
        #endregion
    }
}
