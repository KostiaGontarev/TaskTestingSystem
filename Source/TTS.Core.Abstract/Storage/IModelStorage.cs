using System.Collections.Generic;

using TTS.Core.Abstract.Model;


namespace TTS.Core.Abstract.Storage
{
    public interface IModelStorage
    {
        IList<ITask> Tasks { get; }

        void LoadFrom(string path);
        void WriteTo(string path);
    }
}
