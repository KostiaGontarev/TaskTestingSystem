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
        public IReadOnlyList<ICharacteristic> Requirements
        {
            get { return this.requirements.AsReadOnly(); }
        }
        #endregion

        #region Constructors
        public TestResult(ITestInfo test, IEnumerable<ICharacteristic> results)
        {
            this.test = test;
            this.requirements = new List<ICharacteristic>();
            foreach (ICharacteristic item in results)
            {
                this.requirements.Add(item);
            }
        }
        #endregion
    }
}
