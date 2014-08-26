using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TTS.Core.Abstract.Declarations;
using TTS.Core.Abstract.Model;
using TTS.Core.Abstract.Storage;

using TTS.Core.Concrete.Model;


namespace TTS.Core.Concrete.Storage
{
    internal class JsonModelSerializer : IModelSerializer
    {
        #region Static Members
        private static readonly JsonModelSerializer Instance = new JsonModelSerializer();
        public static JsonModelSerializer GetInstance()
        {
            return JsonModelSerializer.Instance;
        }
        #endregion

        #region Data Members
        private readonly JsonSerializer serializer = JsonSerializer.CreateDefault();
        #endregion

        #region Constructors
        private JsonModelSerializer() { }
        #endregion

        #region IModelSerializer Members
        public string ConvertToString(IEnumerable<ITask> tasks)
        {
            try
            {
                using (TextWriter writer = new StringWriter())
                {
                    this.serializer.Serialize(writer, tasks);
                    return writer.ToString();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Serialization error! {0}", exc.Message);
                return String.Empty;
            }
        }
        public IList<ITask> ConvertToTasks(string text)
        {
            try
            {
                JToken jsonTasks = JToken.Parse(text);
                return jsonTasks
                    .Select(this.ConvertToTask)
                    .ToList();
            }
            catch (Exception exc)
            {
                Console.WriteLine("Deserialization error! {0}", exc.Message);
                return new List<ITask>();
            }
        }
        #endregion

        #region Assistants
        private ITask ConvertToTask(JToken jsonTask)
        {
            try
            {
                Task task = new Task
                {
                    Name = (string)jsonTask["Name"],
                    Description = (string)jsonTask["Description"]
                };

                foreach (ICharacteristic charachteristic in this.ParseRequirements(jsonTask))
                {
                    task.Requirements.Add(charachteristic);
                }
                foreach (ITestInfo test in this.ParseTests(jsonTask))
                {
                    task.Tests.Add(test);
                }
                foreach (ITaskTestResult result in this.ParseResults(jsonTask))
                {
                    task.AddTestingResult(result);
                }
                return task;
            }
            catch (Exception exc)
            {
                Console.WriteLine("Deserialization error! {0}", exc.Message);
                return new Task();
            }
        }

        private IEnumerable<ICharacteristic> ParseRequirements(JToken requirementsJson)
        {
            return requirementsJson["Requirements"]
                .Select(this.ParseRequirement)
                .ToList();
        }
        private ICharacteristic ParseRequirement(JToken charachteristicJson)
        {
            ICharacteristic charachteristic = new Characteristic();

            string typeString = (string)charachteristicJson["Type"];
            int typeInt = 0;
            int.TryParse(typeString, out typeInt);
            bool isTypeCorrect = Enum.IsDefined(typeof(CharacteristicType), typeInt);
            if (isTypeCorrect)
                charachteristic.Type = (CharacteristicType)typeInt;
            else
                throw new ArgumentException("Charachteristics json is incorrect!");

            string valueString = (string)charachteristicJson["Value"];
            bool valueBool;
            double valueDouble;
            bool isBool = bool.TryParse(valueString, out valueBool);
            bool isDouble = double.TryParse(valueString, out valueDouble);
            if (isDouble)
                charachteristic.Value = valueDouble;
            else if (isBool)
                charachteristic.Value = valueBool;
            else
                throw new ArgumentException("Charachteristics json is incorrect!");

            return charachteristic;
        }

        private IEnumerable<ITestInfo> ParseTests(JToken testsJson)
        {
            return testsJson["Tests"]
                .Select(this.ParseTest)
                .ToList();
        }
        private ITestInfo ParseTest(JToken testJson)
        {
            TestInfo test = new TestInfo
            {
                Input = (string) testJson["Input"], 
                Output = (string) testJson["Output"]
            };
            return test;
        }

        private IEnumerable<ITaskTestResult> ParseResults(JToken jsonTask)
        {
            return jsonTask["Results"]
                .Select(this.ParseResult)
                .ToList();
        }
        private ITaskTestResult ParseResult(JToken resultJson)
        {
            string author = (string)resultJson["Author"];
            string filePath = (string) resultJson["FilePath"];
            IEnumerable<ITestResult> results = this.ParseTestResults(resultJson);
            TaskTestsResult taskResult = new TaskTestsResult(author, filePath, results);
            return taskResult;
        }
        private IEnumerable<ITestResult> ParseTestResults(JToken resultsJson)
        {
            return resultsJson["Results"]
                .Select(this.ParseTestResult)
                .ToList();
        }
        private ITestResult ParseTestResult(JToken resultJson)
        {
            ITestInfo test = this.ParseTest(resultJson["Test"]);
            IEnumerable<ICharacteristic> results = this.ParseRequirements(resultJson);
            TestResult result = new TestResult(test, results);
            return result;
        }
        #endregion
    }
}
