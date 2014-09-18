using System;
using System.Collections.Generic;

using TTS.Core.Abstract.Declarations;
using TTS.Core.Abstract.Model;


namespace TTS.Core.Concrete.Model
{
    [Serializable]
    internal class TestResult : ITestResult
    {
        #region Data Members
        private Guid testId;
        private List<Characteristic> requirements;
        #endregion

        #region Properties
        public Guid TestID
        {
            get { return this.testId; }
            set
            {
                if (value != Guid.Empty)
                    this.testId = value;
            }
        }
        public List<Characteristic> Requirements
        {
            get { return this.requirements; }
            set
            {
                if (value != null && value.Count != 0)
                    this.requirements = value;
            }
        }

        IReadOnlyList<Characteristic> ITestResult.Requirements
        {
            get { return this.requirements.AsReadOnly(); }
        }
        public bool IsPassed { get; set; }
        #endregion

        #region Constructors
        public TestResult()
        {
            this.requirements = new List<Characteristic>();            
        }
        public TestResult(Guid testId, IEnumerable<Characteristic> results) : this()
        {
            this.testId = testId;
            foreach (Characteristic item in results)
            {
                this.requirements.Add(item);
            }
        }
        #endregion
    }
}
