using System;
using System.Collections.Generic;


namespace TTS.Core.Interfaces.Model
{
    public interface ITaskTestResult
    {
        Guid TaskID { get; }
        string FilePath { get; }
        IReadOnlyList<ITestResult> Results { get; }
    }
}
