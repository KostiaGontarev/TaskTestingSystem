using System.Collections.Generic;

using TTS.Core.Abstract.Controllers;
using TTS.Core.Abstract.Model;
using TTS.Core.Concrete.Controllers;
using TTS.Core.Concrete.Model;
using TTS.Core.Concrete.Storage;

namespace TTS.Core.Concrete
{
    public static class CoreAccessor
    {
        #region Data Members
        private static readonly ITaskController TaskController = new TaskController(new ModelStorage());
        private static readonly ITestController TestController = new TestController();
        #endregion

        #region Editable Models
        public static ITestInfo CreateTestInfo()
        {
            return new TestInfo();
        }
        public static ICharacteristic CreateCharacteristic()
        {
            return new Characteristic();
        }
        public static ITask CreateTask()
        {
            return new Task();
        } 
        #endregion

        #region ReadOnly Models
        public static ITestResult CreateTestResult(ITestInfo test, IList<ICharacteristic> requirements)
        {
            return new TestResult(test, requirements);
        }
        public static ITaskTestResult CreateTaskTestResult(string author, string path, IEnumerable<ITestResult> results)
        {
            return new TaskTestsResult(author, path, results);
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
        #endregion
    }
}
