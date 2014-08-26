using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using TTS.Core.Abstract.Model;
using TTS.Core.Abstract.Storage;


namespace TTS.Core.Concrete.Storage
{
    internal class ModelStorage : IModelStorage
    {
        #region Data Members
        private readonly IModelSerializer serializer;
        private readonly List<ITask> tasks;
        #endregion

        #region Properties
        public IList<ITask> Tasks
        {
            get { return this.tasks; }
        }
        #endregion

        #region Constructors
        public ModelStorage()
        {
            this.serializer = JsonModelSerializer.GetInstance();
            this.tasks = new List<ITask>();
        }
        #endregion

        #region Members
        public void LoadFrom(string path)
        {
            try
            {
                if (!File.Exists(path))
                    throw new ArgumentException("File is not exist!");

                IList<ITask> result = this.ReadTasksFromFile(path);
                if (result.Count > 0)
                {
                    this.FillTasksCollection(result);
                }
                else
                {
                    throw new ArgumentException("File is incorrect or empty!");
                }
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

                using (FileStream stream = File.Create(path))
                {
                    String json = this.serializer.ConvertToString(this.Tasks);
                    byte[] buffer = Encoding.Unicode.GetBytes(json);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Writting error! {0}", exc.Message);
                throw;
            }
        }
        #endregion

        #region Assistants
        private IList<ITask> ReadTasksFromFile(string path)
        {
            byte[] buffer = File.ReadAllBytes(path);
            string json = Encoding.Unicode.GetString(buffer);
            IList<ITask> result = this.serializer.ConvertToTasks(json);
            return result;
        }
        private void FillTasksCollection(IEnumerable<ITask> result)
        {
            this.Tasks.Clear();
            foreach (ITask task in result)
            {
                this.Tasks.Add(task);
            }
        }
        #endregion
    }
}
