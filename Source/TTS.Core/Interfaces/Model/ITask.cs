using System;
using System.Collections.Generic;

using TTS.Core.Declarations;


namespace TTS.Core.Interfaces.Model
{
    public interface ITask
    {
        Guid ID { get; }
        string Name { get; set; }
        string Description { get; set; }
        IList<Guid> Tests { get; }
        IList<Characteristic> Requirements { get; }
    }
}
