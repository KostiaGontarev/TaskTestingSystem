using System.Collections.Generic;

using TTS.Core.Abstract.Model;


namespace TTS.Core.Abstract.Controllers
{
    public interface ITestController
    {
        ITask Task { get; set; }

        void Run(IList<ITestInfo> tests);
    }
}
