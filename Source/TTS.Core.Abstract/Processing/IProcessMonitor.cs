using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTS.Core.Abstract.Model;

namespace TTS.Core.Abstract.Processing
{
    public interface IProcessMonitor
    {
        Process Process { get; }
        ICharacteristic Result { get; }
        bool IsPassed { get; }
        bool IsSettedUp { get; }

        void Start();
        void Stop();
    }
}
