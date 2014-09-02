using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTS.Core.Abstract.Declarations;
using TTS.Core.Abstract.Model;

namespace TTS.Core.Abstract.Processing
{
    public interface IProcessMonitor
    {
        Process Process { get; }
        CharacteristicType Type { get; }
        ICharacteristic Result { get; }

        void Start();
        void Stop();
    }
}
