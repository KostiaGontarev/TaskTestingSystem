using System.Collections.Generic;

using TTS.Core.Interfaces.Model;
using TTS.Core.Interfaces.Storage;
using TTS.Core.Interfaces.Controllers;


namespace TTS.Core.Controllers
{
    internal class TaskController : ITaskController
    {
        #region Data Members
        private readonly IDataStorage storage = CoreAccessor.GetStorage();
        #endregion

        #region Properties
        public IList<ITask> Tasks
        {
            get { return this.storage.Tasks; }
        }
        #endregion

        #region Members
        public void LoadFrom(string path)
        {
            this.storage.LoadFrom(path);
        }
        public void WriteTo(string path)
        {
            this.storage.WriteTo(path);
        }
        #endregion
    }
}
