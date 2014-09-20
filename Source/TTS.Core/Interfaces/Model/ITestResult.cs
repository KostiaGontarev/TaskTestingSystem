using System;
using System.Collections.Generic;

using TTS.Core.Declarations;


namespace TTS.Core.Interfaces.Model
{
    public interface ITestResult
    {
        Guid TestID { get; }
        IReadOnlyList<Characteristic> Requirements { get; }
        bool IsPassed { get; }
    }
}
