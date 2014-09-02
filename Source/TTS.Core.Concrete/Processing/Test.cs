using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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
        private readonly List<IProcessMonitor> monitors;
        private readonly List<ICharacteristic> requirements;
        private readonly TestResult result;
        #endregion

        #region Properties
        public IReadOnlyList<IProcessMonitor> Monitors
        {
            get { return this.monitors; }
        }
        public ITestResult Result
        {
            get { return this.result; }
        }
        #endregion

        #region Constructors
        public Test(ITestInfo testInfo, Process process, List<IProcessMonitor> monitors, List<ICharacteristic> requirements)
        {
            this.ioTestController.Setup(process, testInfo);
            this.monitors = monitors;
            this.requirements = requirements;
            this.ioTestController.InputInjected += (sender, args) => this.OnInputInjected();
            this.ioTestController.ProcessExecuted += (sender, args) => this.OnProcessExecuted();
            this.ioTestController.OutputChecked += (sender, args) => this.OnOutputChecked();

            this.result = new TestResult(testInfo);
        }
        #endregion

        #region Events
        public event EventHandler InputInjected;
        public event EventHandler ProcessExecuted;
        public event EventHandler OutputChecked;
        public event EventHandler RequirementsChecked;
        #endregion

        #region Members
        public void Run()
        {
            this.StartMonitors();
            this.PerformIOTest();
            this.StopMonitors();
            this.SaveMonitorsValues();
        }
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
        protected virtual void OnRequirementsChecked()
        {
            EventHandler handler = RequirementsChecked;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion

        #region Assistants
        private void StartMonitors()
        {
            foreach (IProcessMonitor monitor in this.Monitors)
            {
                monitor.Start();
            }
        }
        private void PerformIOTest()
        {
            this.ioTestController.Start();
            this.result.Requirements.Add(new Characteristic
            {
                Type = CharacteristicType.InputOutputCompliance,
                Value = this.ioTestController.Result
            });
        }
        private void StopMonitors()
        {
            foreach (IProcessMonitor monitor in this.Monitors)
            {
                monitor.Stop();
            }
        }
        private void SaveMonitorsValues()
        {
            foreach (IProcessMonitor monitor in this.Monitors)
            {
                this.result.Requirements.Add(new Characteristic
                {
                    Type = monitor.Result.Type,
                    Value = monitor.Result.Value
                });
            }
        }
        #endregion
    }
}
