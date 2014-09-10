using System.Collections.Generic;

using TTS.Core.Abstract.Model;


namespace TTS.Core.Concrete.Model
{
    internal class TestResult : ITestResult
    {
        #region Data Members
        private readonly ITestInfo test;
        private readonly List<ICharacteristic> requirements;
        #endregion

        #region Properties
        public ITestInfo Test
        {
            get { return this.test; }
        }
        IReadOnlyList<ICharacteristic> ITestResult.Requirements
        {
            get { return this.requirements.AsReadOnly(); }
        }
        public List<ICharacteristic> Requirements
        {
            get { return this.requirements; }
        }
        #endregion

        #region Constructors
        public TestResult(ITestInfo testInfo, IEnumerable<ICharacteristic> results)
        {
            this.test = testInfo;
            this.requirements = new List<ICharacteristic>();
            foreach (ICharacteristic item in results)
            {
                this.requirements.Add(item);
            }
        }
        #endregion
    }
}
