using System;

using TTS.Core.Abstract.Model;


namespace TTS.Core.Concrete.Model
{
    [Serializable]
    internal class TestInfo : ITestInfo
    {
        #region Data Members
        private Guid id;
        private string input;
        private string output;
        #endregion

        #region Properties
        public Guid ID
        {
            get { return this.id; }
            set
            {
                if (value != Guid.Empty)
                    this.id = value;
            }
        }
        public string Input
        {
            get { return this.input; }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                    this.input = value;
            }
        }
        public string Output
        {
            get { return this.output; }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                    this.output = value;
            }
        }
        #endregion

        #region Constructors
        public TestInfo()
        {
            this.id = Guid.NewGuid();
            this.input = String.Empty;
            this.output = String.Empty;
        }
        #endregion
    }
}
