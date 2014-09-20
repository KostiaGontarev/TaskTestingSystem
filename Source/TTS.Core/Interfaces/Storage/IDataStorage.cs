using System.Collections.Generic;

using TTS.Core.Interfaces.Model;


namespace TTS.Core.Interfaces.Storage
{
    public interface IDataStorage
    {
        IList<ITask> Tasks { get; }
        IList<ITestInfo> Tests { get; }
        IReadOnlyCollection<ITaskTestResult> Results { get; }

        void LoadFrom(string path);
        void WriteTo(string path);
    }
}
