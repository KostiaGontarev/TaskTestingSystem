using System;
using System.Collections.Generic;
using System.ComponentModel;
using TTS.Core.Abstract.Model;


namespace TTS.Core.Abstract.Controllers
{
    public interface ITestController
    {
        event ProgressChangedEventHandler ProgressChanged;
        event EventHandler TestStarted;
        event EventHandler TestFinished;

        event EventHandler TestChanged;
        event EventHandler AllTestsFinished;
        
        ITask Task { get; set; }
        int TestCount { get; }
        void Run(IList<ITestInfo> tests, IList<string> files);
        void Stop();

        bool IsTestPassed(IList<ICharacteristic> requirements, IReadOnlyList<ICharacteristic> results);
    }
}
