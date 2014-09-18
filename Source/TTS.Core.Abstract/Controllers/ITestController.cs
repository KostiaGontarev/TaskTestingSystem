using System;
using System.Collections.Generic;
using System.ComponentModel;

using TTS.Core.Abstract.Declarations;
using TTS.Core.Abstract.Model;


namespace TTS.Core.Abstract.Controllers
{
    public interface ITestController
    {
        event EventHandler TestStarted;
        event ProgressChangedEventHandler ProgressChanged;
        event EventHandler TestFinished;
        event EventHandler TestChanged;
        event EventHandler AllTestsFinished;
        
        ITask Task { get; set; }
        IEnumerable<Guid> Tests { get; }
        IReadOnlyCollection<ITaskTestResult> Results { get; }
        void Run(IList<Guid> tests, IList<string> files);
        void Stop();

        bool IsTestPassed(IEnumerable<Characteristic> requirements, IReadOnlyCollection<Characteristic> results);
    }
}
