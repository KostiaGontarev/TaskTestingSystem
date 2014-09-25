using System;
using System.Collections.Generic;
using System.ComponentModel;

using TTS.Core.Interfaces.Model;
using TTS.Core.Declarations;


namespace TTS.Core.Interfaces.Controllers
{
    public interface ITestController
    {
        event EventHandler TestStarted;
        event ProgressChangedEventHandler ProgressChanged;
        event EventHandler TestFinished;
        event EventHandler TestChanged;
        event EventHandler AllTestsFinished;
        
        ITask Task { get; set; }
        IReadOnlyCollection<ITaskTestResult> Results { get; }

        void Run(IList<Guid> tests, IList<string> files);
        void Stop();
    }
}
