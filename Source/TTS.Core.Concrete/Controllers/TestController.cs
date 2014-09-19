using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

using TTS.Core.Abstract.Controllers;
using TTS.Core.Abstract.Declarations;
using TTS.Core.Abstract.Model;
using TTS.Core.Abstract.Storage;

using TTS.Core.Concrete.Model;
using TTS.Core.Concrete.Processing;
using TTS.Core.Concrete.Storage;


namespace TTS.Core.Concrete.Controllers
{
    internal class TestController : ITestController
    {
        #region Data Members
        private readonly TestPerformer performer = TestPerformer.Instance;
        private readonly Queue<ITestInfo> testInfoQueue = new Queue<ITestInfo>();
        private readonly Queue<string> filesQueue = new Queue<string>();

        private Task task;
        private IList<ITestInfo> testsToPerform;
        private readonly List<ITestResult> results = new List<ITestResult>();
        #endregion

        #region Properties
        public ITask Task
        {
            get { return this.task; }
            set
            {
                if (value is Task)
                    this.task = value as Task;
            }
        }
        public IEnumerable<Guid> Tests
        {
            get { return this.Task.Tests; }    
        }
        public IReadOnlyCollection<ITaskTestResult> Results
        {
            get
            {
                IDataStorage storage = DataManager.Instance;
                return storage.Results.Where(result => result.TaskID == this.Task.ID).ToList();
            }    
        } 
        #endregion

        #region Constructors
        public TestController()
        {
            this.performer.ProgressChanged += performer_ProgressChanged;
            this.performer.TestStarted += performer_TestStarted;
            this.performer.TestFinished += performer_TestFinished;
        }
        #endregion

        #region Members
        public void Run(IList<Guid> tests, IList<string> files)
        {
            DataManager storage = DataManager.Instance;
            this.testsToPerform = storage.Tests.Where(test => tests.Contains(test.ID)).ToList();
            this.SetupFilesQueue(files);
            this.ProceedNextFile();
        }
        public void Stop()
        {
            this.testInfoQueue.Clear();
            this.filesQueue.Clear();
            this.performer.Stop();
            this.OnAllTestsFinished();
        }
        #endregion

        #region Events
        public event ProgressChangedEventHandler ProgressChanged;
        public event EventHandler TestStarted;
        public event EventHandler TestFinished;

        public event EventHandler TestChanged;
        public event EventHandler AllTestsFinished;
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
        protected virtual void OnTestFinished(BoolResultEventArgs args)
        {
            EventHandler handler = TestFinished;
            if (handler != null) handler(this, args);
        }
        protected virtual void OnTestChanged(TestEventArgs args)
        {
            EventHandler handler = TestChanged;
            if (handler != null) handler(this, args);
        }
        protected virtual void OnAllTestsFinished()
        {
            EventHandler handler = AllTestsFinished;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion

        #region Event Handlers
        private void performer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.OnProgressChanged(e);
        }
        private void performer_TestStarted(object sender, EventArgs e)
        {
            this.OnTestStarted();
        }
        private void performer_TestFinished(object sender, EventArgs e)
        {
            TestResult result = this.performer.Result;
            bool success = this.IsTestPassed(this.Task.Requirements, result.Requirements);
            result.IsPassed = success;
            this.results.Add(result);

            this.OnTestFinished(new BoolResultEventArgs(success));
            
            this.PerformNextTest();
        }
        #endregion

        #region Assistants
        private void SetupFilesQueue(IEnumerable<string> files)
        {
            foreach (string file in files)
            {
                this.filesQueue.Enqueue(file);
            }
        }
        private void ProceedNextFile()
        {
            if (this.filesQueue.Count != 0)
                this.TestFile(this.filesQueue.Dequeue(), this.testsToPerform);
            else
                this.OnAllTestsFinished();
        }
        private void TestFile(string file, IEnumerable<ITestInfo> tests)
        {
            this.SetupProcess(file);
            this.SetupTestsQueue(tests);
            this.PerformNextTest();
        }
        private void SetupProcess(string file)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo(file) { WindowStyle = ProcessWindowStyle.Hidden },
            };
            this.performer.Process = process;
        }
        private void SetupTestsQueue(IEnumerable<ITestInfo> tests)
        {
            foreach (ITestInfo test in tests)
            {
                this.testInfoQueue.Enqueue(test);
            }
        }
        private void PerformNextTest()
        {
            if (this.testInfoQueue.Count != 0)
            {
                this.performer.TestInfo = this.testInfoQueue.Dequeue();
                TestEventArgs args = new TestEventArgs(this.performer.TestInfo, this.performer.Process.StartInfo.FileName);
                this.OnTestChanged(args);
                this.performer.Run();
            }
            else
            {
                this.SaveResults();
                this.ProceedNextFile();
            }
        }
        private void SaveResults()
        {
            TaskTestsResult result = new TaskTestsResult(this.Task.ID, this.performer.Process.StartInfo.FileName, results);
            DataManager storage = DataManager.Instance;
            storage.Results.Add(result);
            this.results.Clear();
        }
        #endregion

        #region Static Members
        public bool IsTestPassed(IEnumerable<Characteristic> requirements, IReadOnlyCollection<Characteristic> results)
        {
            foreach (Characteristic requirement in requirements)
            {
                Characteristic result = results.SingleOrDefault(element => element.Type == requirement.Type);
                if (result != null)
                {
                    bool success = CharacteristicTypeValue.CheckForSuccess(requirement, result);
                    if (!success)
                        return false;
                }
                else
                    return false;
            }
            return true;
        } 
        #endregion
    }
}
