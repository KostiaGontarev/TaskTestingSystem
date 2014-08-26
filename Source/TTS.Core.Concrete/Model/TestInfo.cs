using System;

using TTS.Core.Abstract.Model;


namespace TTS.Core.Concrete.Model
{
    internal class TestInfo : ITestInfo
    {
        #region Data Members
        private string input;
        private string output;
        #endregion

        #region Constructors
        public TestInfo()
        {
            this.input = String.Empty;
            this.output = String.Empty;
        }
        #endregion

        #region Properties
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
    }
}
