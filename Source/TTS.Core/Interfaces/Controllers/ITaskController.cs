using System.Collections.Generic;

using TTS.Core.Interfaces.Model;


namespace TTS.Core.Interfaces.Controllers
{
    public interface ITaskController
    {
        IList<ITask> Tasks { get; }

        void LoadFrom(string path);
        void WriteTo(string path);
    }
}
