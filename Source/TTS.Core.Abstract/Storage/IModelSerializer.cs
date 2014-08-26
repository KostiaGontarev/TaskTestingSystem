using System;
using System.Collections.Generic;

using TTS.Core.Abstract.Model;


namespace TTS.Core.Abstract.Storage
{
    public interface IModelSerializer
    {
        string ConvertToString(IEnumerable<ITask> task);
        IList<ITask> ConvertToTasks(String text);
    }
}
