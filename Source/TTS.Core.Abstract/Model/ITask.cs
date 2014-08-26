using System.Collections.Generic;


namespace TTS.Core.Abstract.Model
{
    public interface ITask
    {
        string Name { get; set; }
        string Description { get; set; }
        IList<ITestInfo> Tests { get; }
        IList<ICharacteristic> Requirements { get; }
        IReadOnlyList<ITaskTestResult> Results { get; }
    }
}
