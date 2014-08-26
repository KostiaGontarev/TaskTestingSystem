using System.Collections.Generic;

using TTS.Core.Abstract.Model;


namespace TTS.Core.Abstract.Controllers
{
    public interface ITaskController
    {
        IList<ITask> Tasks { get; }

        void LoadFrom(string path);
        void WriteTo(string path);
    }
}
