using System;
using System.Collections.Generic;

using TTS.Core.Abstract.Declarations;


namespace TTS.Core.Abstract.Model
{
    public interface ITestResult
    {
        Guid TestID { get; }
        IReadOnlyList<Characteristic> Requirements { get; }
        bool IsPassed { get; }
    }
}
