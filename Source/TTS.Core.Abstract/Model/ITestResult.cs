using System.Collections.Generic;


namespace TTS.Core.Abstract.Model
{
    public interface ITestResult
    {
        ITestInfo Test { get; }
        IReadOnlyList<ICharacteristic> Requirements { get; }
    }
}
