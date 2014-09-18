using System.Collections.Generic;

using TTS.Core.Abstract.Model;


namespace TTS.Core.Abstract.Storage
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
