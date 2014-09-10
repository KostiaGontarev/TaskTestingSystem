using System.Collections.Generic;


namespace TTS.Core.Abstract.Model
{
    public interface ITaskTestResult
    {
        string FilePath { get; }
        IReadOnlyList<ITestResult> Results { get; }
    }
}
