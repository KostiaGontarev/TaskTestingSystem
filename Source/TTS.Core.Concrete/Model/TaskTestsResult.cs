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

        private string author;
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

        public string Author
        {
            get { return this.author; }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    this.author = value;
                }
            }
        }
        #endregion

        #region Constructors
        public TaskTestsResult(String author, string filePath, IEnumerable<ITestResult> results)
        {
            this.filePath = filePath;
            this.results = new List<ITestResult>();
            this.results.AddRange(results);

            this.Author = author;
        }
        #endregion
    }
}
