using System;
using System.Collections.Generic;

using TTS.Core.Abstract.Controllers;
using TTS.Core.Abstract.Model;


namespace TTS.Core.Concrete.Controllers
{
    internal class TestController : ITestController
    {
        #region Data Members
        private ITask task;
        #endregion

        #region Properties
        public ITask Task
        {
            get { return this.task; }
            set
            {
                if (value != null)
                    this.task = value;
            }
        }
        #endregion

        #region Members
        public void Run(IList<ITestInfo> tests)
        {
            foreach (ITestInfo test in tests)
            {
                Console.WriteLine("{0}:{1}", test.Input, test.Output);
            }
        } 
        #endregion
    }
}
