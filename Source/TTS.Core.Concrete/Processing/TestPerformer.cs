using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using TTS.Core.Abstract.Declarations;
using TTS.Core.Abstract.Model;

using TTS.Core.Concrete.Model;


namespace TTS.Core.Concrete.Processing
{
    internal class TestPerformer
    {
        #region Data Members
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private readonly IOTest ioTest = IOTest.Instance;
        private readonly object locker = new object();

        private TestResult result;
        #endregion

        #region Properties
        public ITestResult Result
        {
            get
            {
                lock (this.locker)
                {
                    return this.result;
                }
            }
        }
        public Process Process
        {
            get { return this.ioTest.Process; }
            set
            {
                if (value != null)
                {
                    this.ioTest.Process = value;
                }
            }
        }
        public ITestInfo TestInfo
        {
            get { return this.ioTest.TestInfo; }
            set
            {
                if (value != null)
                {
                    this.ioTest.TestInfo = value;
                }
            }
        }
        public bool IsReady
        {
            get { return this.Process != null; }
        }
        #endregion

        #region Constructors
        private TestPerformer()
        {
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;

            this.worker.DoWork += worker_DoWork;
            this.worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            this.worker.ProgressChanged += worker_ProgressChanged;
        }
        #endregion

        #region Static Members
        private static readonly TestPerformer instance = new TestPerformer();
        public static TestPerformer Instance
        {
            get { return TestPerformer.instance; }
        }
        #endregion

        #region Members
        public void Run()
        {
            this.OnTestStarted();
            this.worker.RunWorkerAsync();
        }
        #endregion

        #region Events
        public event ProgressChangedEventHandler ProgressChanged;
        public event EventHandler TestStarted;
        public event EventHandler TestFinished;
        #endregion

        #region Event Invokators
        protected virtual void OnProgressChanged(ProgressChangedEventArgs args)
        {
            ProgressChangedEventHandler handler = ProgressChanged;
            if (handler != null) handler(this, args);
        }
        protected virtual void OnTestStarted()
        {
            EventHandler handler = TestStarted;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        protected virtual void OnTestFinished()
        {
            EventHandler handler = TestFinished;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion

        #region Event Handlers
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.OnProgressChanged(e);
        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.OnTestFinished();
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.PerformIOTest();
            this.SetupResult();
            this.worker.ReportProgress(100);
        }
        #endregion

        #region Assistants
        private void PerformIOTest()
        {
            this.ioTest.Start(this.worker);
        }
        private void SetupResult()
        {
            List<ICharacteristic> results = new List<ICharacteristic>();
            ICharacteristic iocTest = new Characteristic
            {
                Type = CharacteristicType.InputOutputCompliance,
                Value = this.ioTest.Result
            };
            results.Add(iocTest);

            lock (this.locker)
            {
                this.result = new TestResult(this.ioTest.TestInfo, results);
            }
        }
        #endregion
    }
}
