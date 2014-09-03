using System.Collections.Generic;

using TTS.Core.Abstract.Model;
using TTS.Core.Abstract.Processing;


namespace TTS.Core.Abstract.Controllers
{
    public interface ITestController
    {
        ITask Task { get; set; }
        IReadOnlyList<ITest> Tests { get; }
        void Run(IList<ITest> tests, IList<string> files);
    }
}
