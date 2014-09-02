using System;
using System.Collections.Generic;

using TTS.Core.Abstract.Controllers;
using TTS.Core.Abstract.Model;
using TTS.Core.Abstract.Processing;
using TTS.Core.Concrete.Processing;


namespace TTS.Core.Concrete.Controllers
{
    internal class TestController : ITestController
    {
        #region Data Members
        private ITask task;
        private readonly List<IProcessMonitor> monitors = new List<IProcessMonitor>();
        private readonly List<ITest> tests = new List<ITest>();
        #endregion

        #region Properties
        public ITask Task
        {
            get { return this.task; }
            set
            {
                if (value != null)
                    this.task = value;

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
        public void Run(IList<ITest> tests)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
