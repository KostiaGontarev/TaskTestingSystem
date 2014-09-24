using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using TTS.Core.Interfaces.Model;
using TTS.Core.Interfaces.Storage;

using TTS.Core.Model;


namespace TTS.Core.Storage
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
                Storage result = this.ParseStorage(path);
                this.LoadStorage(result);
            }
            catch (Exception exc)
            {
                throw new FileLoadException("Storage opening error!", exc);
            }
        }
        public void WriteTo(string path)
        {
            try
            {
                Storage storage = this.PrepareStorage();
                this.SaveStorage(path, storage);
            }
            catch (Exception exc)
            {
                throw new IOException("Storage saving error!", exc);
            }
        }
        public void SubstituteIOSet(IList<ITestInfo> newTests, IList<ITestInfo> oldTests)
        {
            this.AddTests(newTests, oldTests);
            this.DeleteTests(newTests, oldTests);
            this.UpdateTests(newTests, oldTests);
        }
        #endregion

        #region Assistants
        private Storage ParseStorage(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentException("File is not exist!");

            Storage result;
            using (TextReader reader = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                JsonSerializer serializer = new JsonSerializer();
                result = serializer.Deserialize(reader, typeof(Storage)) as Storage;
            }
            return result;
        }
        private void LoadStorage(Storage result)
        {
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
                throw new ArgumentException("The storage is incorrect or empty!");
        }
        private Storage PrepareStorage()
        {
            Storage storage = new Storage
            {
                Tasks = this.tasks.Select(task => task as Task).ToArray(),
                Tests = this.tests.Select(test => test as TestInfo).ToArray(),
                Results = this.results.Select(result => result as TaskTestsResult).ToArray()
            };
            return storage;
        }
        private void SaveStorage(string path, Storage storage)
        {
            if (File.Exists(path))
                File.Delete(path);

            using (TextWriter writer = new StreamWriter(File.Create(path)))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, storage);
            }
        }

        private void AddTests(IEnumerable<ITestInfo> newTests, IEnumerable<ITestInfo> currentTests)
        {
            List<ITestInfo> toAdd = newTests.Where(newTest => currentTests.All(oldTest => oldTest.ID != newTest.ID)).ToList();
            foreach (ITestInfo testInfo in toAdd)
            {
                this.Tests.Add(testInfo);
            }
        }
        private void DeleteTests(IEnumerable<ITestInfo> newTests, IEnumerable<ITestInfo> currentTests)
        {
            List<ITestInfo> toDelete = currentTests.Where(oldTest => newTests.All(newTest => newTest.ID != oldTest.ID)).ToList();
            foreach (ITestInfo testInfo in toDelete)
            {
                this.Tests.Remove(testInfo);
            }
        }
        private void UpdateTests(IEnumerable<ITestInfo> newTests, IEnumerable<ITestInfo> currentTests)
        {
            List<ITestInfo> toUpdate = currentTests.Where(oldTest => newTests.Any(test => test.ID == oldTest.ID)).ToList();
            foreach (ITestInfo testInfo in newTests)
            {
                ITestInfo newTestInfo = toUpdate.SingleOrDefault(test => test.ID == testInfo.ID);
                if (newTestInfo == null)
                    return;
                newTestInfo.Input = testInfo.Input;
                newTestInfo.Output = testInfo.Output;
            }
        }
        #endregion

        #region Static Members
        public static readonly DataManager Instance = new DataManager();
        #endregion
    }
}
