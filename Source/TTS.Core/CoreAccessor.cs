using TTS.Core.Interfaces.Model;
using TTS.Core.Interfaces.Storage;
using TTS.Core.Interfaces.Controllers;

using TTS.Core.Model;
using TTS.Core.Storage;
using TTS.Core.Controllers;


namespace TTS.Core
{
    public static class CoreAccessor
    {
        #region Data Members
        private static readonly IDataStorage Storage = DataManager.Instance;
        private static readonly ITaskController TaskController = new TaskController();
        private static readonly ITestController TestController = new TestController();
        #endregion

        #region Editable Models
        public static ITestInfo CreateTestInfo()
        {
            return new TestInfo();
        }
        public static ITask CreateTask()
        {
            return new Task();
        } 
        #endregion

        #region Controllers
        public static ITaskController GetTaskController()
        {
            return CoreAccessor.TaskController;
        }
        public static ITestController GetTestController()
        {
            return CoreAccessor.TestController;
        }
        public static IDataStorage GetStorage()
        {
            return CoreAccessor.Storage;
        }
        #endregion
    }
}
