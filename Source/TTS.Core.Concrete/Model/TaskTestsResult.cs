using System;
using System.Collections.Generic;

using TTS.Core.Abstract.Model;


namespace TTS.Core.Concrete.Model
{
    internal class TaskTestsResult : ITaskTestResult
    {
        #region Data Members
        private readonly List<ITestResult> results;
        private readonly string filePath;
        #endregion

        #region Properties
        public string FilePath
        {
            get { return this.filePath; }
        }
        public IReadOnlyList<ITestResult> Results
        {
            get { return this.results.AsReadOnly(); }
        }

        #endregion

        #region Constructors
        public TaskTestsResult(string filePath, IEnumerable<ITestResult> results)
        {
            this.filePath = filePath;
            this.results = new List<ITestResult>();
            this.results.AddRange(results);
        }
        #endregion
    }
}
