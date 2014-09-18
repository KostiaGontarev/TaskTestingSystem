using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using TTS.Core.Abstract.Model;
using TTS.Core.Abstract.Storage;

using TTS.Core.Concrete.Model;


namespace TTS.Core.Concrete.Storage
{
    internal class DataManager : IDataStorage
    {
        #region Nested Members
        [Serializable]
        private class Storage
        {
            #region Properties
            public Task[] Tasks { get; set; }
            public TestInfo[] Tests { get; set; }
            public TaskTestsResult[] Results { get; set; }
            #endregion
        }
        #endregion

        #region Data Members
        private readonly List<ITask> tasks = new List<ITask>();
        private readonly List<ITestInfo> tests = new List<ITestInfo>();
        private readonly List<ITaskTestResult> results = new List<ITaskTestResult>();
        #endregion

        #region Properties
        public IList<ITask> Tasks
        {
            get { return this.tasks; }
        }
        public IList<ITestInfo> Tests
        {
            get { return this.tests; }
        }
        public IList<ITaskTestResult> Results
        {
            get { return this.results; }
        }
        IReadOnlyCollection<ITaskTestResult> IDataStorage.Results
        {
            get { return this.results.AsReadOnly(); }
        }
        #endregion

        #region Constructors
        private DataManager()
        { }
        #endregion

        #region Members
        public void LoadFrom(string path)
        {
            try
            {
                if (!File.Exists(path))
                    throw new ArgumentException("File is not exist!");

                Storage result;
                using (TextReader reader = new StreamReader(new FileStream(path, FileMode.Open)))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    result = serializer.Deserialize(reader, typeof(Storage)) as Storage;
                }

                if (result != null && result.Tasks.Length != 0 && result.Tests.Length != 0)
                {
                    this.tasks.Clear();
                    this.tasks.AddRange(result.Tasks);
                    this.tests.Clear();
                    this.tests.AddRange(result.Tests);
                    this.results.Clear();
                    this.results.AddRange(result.Results);
                }
                else
                    throw new ArgumentException("File is incorrect or empty!");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Reading error! {0}", exc.Message);
                throw;
            }
        }
        public void WriteTo(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);

                Storage storage = new Storage
                {
                    Tasks = this.tasks.Select(task => task as Task).ToArray(),
                    Tests = this.tests.Select(test => test as TestInfo).ToArray(),
                    Results = this.results.Select(result => result as TaskTestsResult).ToArray()
                };

                using (TextWriter writer = new StreamWriter(File.Create(path)))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, storage);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Writting error! {0}", exc.Message);
                throw;
            }
        }
        #endregion

        #region Static Members
        private static readonly DataManager instance = new DataManager();
        public static DataManager Instance
        {
            get { return DataManager.instance; }
        }
        #endregion
    }
}
