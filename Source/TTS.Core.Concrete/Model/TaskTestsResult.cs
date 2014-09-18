using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using TTS.Core.Abstract.Model;


namespace TTS.Core.Concrete.Model
{
    [Serializable]
    internal class TaskTestsResult : ITaskTestResult
    {
        #region Data Members
        private Guid taskId;
        private string filePath;
        private List<TestResult> results;
        #endregion

        #region Properties
        public Guid TaskID
        {
            get { return this.taskId; }
            set
            {
                if (value != Guid.Empty)
                    this.taskId = value;
            }
        }
        public string FilePath
        {
            get { return this.filePath; }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                    this.filePath = value;
            }
        }
        public List<TestResult> Results
        {
            get { return this.results; }
            set
            {
                if (value != null && value.Count != 0)
                    this.results = value;
            }
        }

        [JsonIgnore]
        IReadOnlyList<ITestResult> ITaskTestResult.Results
        {
            get { return this.results.AsReadOnly(); }
        }
        #endregion

        #region Constructors
        public TaskTestsResult()
        {
            this.results = new List<TestResult>();            
        }
        public TaskTestsResult(Guid taskId, string filePath, IEnumerable<ITestResult> results) : this()
        {
            this.taskId = taskId;
            this.filePath = filePath;
            this.results.AddRange(results.OfType<TestResult>());
        }
        #endregion
    }
}
