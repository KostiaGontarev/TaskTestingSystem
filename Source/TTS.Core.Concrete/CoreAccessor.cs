using TTS.Core.Abstract.Controllers;
using TTS.Core.Abstract.Model;
using TTS.Core.Abstract.Storage;

using TTS.Core.Concrete.Controllers;
using TTS.Core.Concrete.Model;
using TTS.Core.Concrete.Storage;


namespace TTS.Core.Concrete
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
