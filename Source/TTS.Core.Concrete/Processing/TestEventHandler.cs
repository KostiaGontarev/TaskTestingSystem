using System;

using TTS.Core.Abstract.Model;


namespace TTS.Core.Concrete.Processing
{
    public class TestEventArgs : EventArgs
    {
        #region Properties
        public ITestInfo TestInfo { get; private set; }
        public string FileName { get; private set; }
        #endregion

        #region Constructors
        internal TestEventArgs(ITestInfo testInfo, string fileName)
        {
            this.FileName = fileName;
            this.TestInfo = testInfo;
        }

        #endregion
    }
}
