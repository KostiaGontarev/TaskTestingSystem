using System;


namespace TTS.Core.Concrete.Processing
{
    public class BoolResultEventArgs : EventArgs
    {
        #region Properties
        public bool Result { get; private set; }
        #endregion

        #region Constructors
        internal BoolResultEventArgs(bool result)
        {
            this.Result = result;
        }
        #endregion
    }
}
