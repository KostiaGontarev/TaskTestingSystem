using System;
using System.Collections.Generic;

using TTS.Core.Abstract.Model;


namespace TTS.Core.Concrete.Model
{
    internal class Task : ITask
    {
        #region Data Members
        private string name;
        private string description;

        private readonly List<ICharacteristic> requirements;
        private readonly List<ITestInfo> tests;
        private readonly List<ITaskTestResult> results;
        #endregion

        #region Properties
        public string Name
        {
            get { return this.name; }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                    this.name = value;
            }
        }
        public string Description
        {
            get { return this.description; }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                    this.description = value;
            }
        }
        public IList<ICharacteristic> Requirements
        {
            get { return this.requirements; }
            set
            {
                if (value == null || value.Count == 0)
                    return;

                this.requirements.Clear();
                this.requirements.AddRange(value);
            }
        }
        public IList<ITestInfo> Tests
        {
            get { return this.tests; }
            set
            {
                if (value == null || value.Count == 0)
                    return;

                this.tests.Clear();
                this.tests.AddRange(value);
            }
        }
        public IReadOnlyList<ITaskTestResult> Results
        {
            get { return this.results; }
        }

        public bool IsEmpty
        {
            get
            {
                return String.IsNullOrWhiteSpace(this.Name) ||
                       String.IsNullOrWhiteSpace(this.Description) ||
                       this.Tests.Count == 0;
            }
        }
        #endregion

        #region Constructors
        public Task()
        {
            this.name = String.Empty;
            this.description = String.Empty;
            this.requirements = new List<ICharacteristic>();
            this.tests = new List<ITestInfo>();
            this.results = new List<ITaskTestResult>();
        }
        public Task(IEnumerable<ICharacteristic> requirements, IEnumerable<ITestInfo> tests)
            :this()
        {
            this.requirements.AddRange(requirements);
            this.tests.AddRange(tests);
        } 
        #endregion

        #region Internal Members
        public void AddTestingResult(ITaskTestResult result)
        {
            this.results.Add(result);
        }
        #endregion
    }
}
