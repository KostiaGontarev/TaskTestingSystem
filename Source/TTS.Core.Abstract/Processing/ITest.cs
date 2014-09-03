using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTS.Core.Abstract.Model;

namespace TTS.Core.Abstract.Processing
{
    public interface ITest
    {
        event EventHandler InputInjected;
        event EventHandler ProcessExecuted;
        event EventHandler OutputChecked;
        event EventHandler RequirementsChecked;

        ITestResult Result { get; }
        Process Process { get; set; }
        IReadOnlyList<IProcessMonitor> Monitors { get; }

        void Run();
    }
}
