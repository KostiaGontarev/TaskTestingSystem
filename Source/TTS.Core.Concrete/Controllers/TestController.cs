using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using TTS.Core.Abstract.Controllers;
using TTS.Core.Abstract.Model;
using TTS.Core.Abstract.Processing;

using TTS.Core.Concrete.Model;
using TTS.Core.Concrete.Processing;


namespace TTS.Core.Concrete.Controllers
{
    internal class TestController : ITestController
    {
        #region Data Members
        private Task task;
        private readonly List<IProcessMonitor> monitors = new List<IProcessMonitor>();
        private readonly List<ITest> tests = new List<ITest>();
        #endregion

        #region Properties
        public ITask Task
        {
            get { return this.task; }
            set
            {
                if (value is Task)
                    this.task = value as Task;
                this.tests.Clear();
                foreach (ITestInfo testInfo in this.Task.Tests)
                {
                    this.tests.Add(new Test(testInfo, this.monitors));
                }
                //Здесь добавление мониторов
            }
        }

        public IReadOnlyList<ITest> Tests
        {
            get { return this.tests; }
        }
        #endregion

        #region Members
        public void Run(IList<ITest> tests, IList<string> files)
        {
            foreach (string file in files)
            {
                this.TestFile(file);
            }
        }
        #endregion

        #region Assistants
        private void TestFile(string file)
        {
            this.SetTestProcess(file);
            this.PerformTests();
            this.SaveResult(file);
        }
       private void SetTestProcess(string file)
        {
            foreach (ITest test in this.tests)
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo(file){WindowStyle = ProcessWindowStyle.Hidden},
                };
                test.Process = process;
            }
        }
        private void PerformTests()
        {
            foreach (ITest test in this.Tests)
            {
                test.Run();
            }
        }
        private IEnumerable<ITestResult> CollectResults()
        {
            return this.Tests.Select(test => test.Result)
                .ToList();
        }
        private void SaveResult(string file)
        {
            IEnumerable<ITestResult> results = this.CollectResults();
            string author = Path.GetDirectoryName(file);
            ITaskTestResult result = new TaskTestsResult(author, file, results);
            this.task.AddTestingResult(result);
        }
        #endregion
    }
}
