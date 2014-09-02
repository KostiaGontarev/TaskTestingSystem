using System;
using System.Diagnostics;
using TTS.Core.Abstract.Controllers;
using TTS.Core.Abstract.Model;
using TTS.Core.Concrete;
using TTS.Core.Abstract.Declarations;

namespace TTS.ConsoleTesting
{
    class Program
    {
        static void Main()
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo("D:\\Games\\Revolt\\Revolt.exe");
            ITaskController taskController = CoreAccessor.GetTaskController();
            ITestController testController = CoreAccessor.GetTestController();
            ITask task = CreateTask();
            taskController.Tasks.Add(task);
            testController.Task = task;
            testController.Run(task.Tests);

            Console.WriteLine("Нажмите энтер для завершения...");
            Console.ReadLine();
        }

        private static ITask CreateTask()
        {
            ITask task = CoreAccessor.CreateTask();
            task.Name = "Задача о переводе нечетного числа в ближайшее четное";
            task.Description = "Дано нечетное число, необходимо преобразовать его в ближайшее четное";

            task.Requirements.Add(CreateRequirement(CharacteristicType.InputOutputCompliance, true));
            task.Requirements.Add(CreateRequirement(CharacteristicType.MaxExecutionTime, 2.0));

            task.Tests.Add(CreateTest("1", "2"));
            task.Tests.Add(CreateTest("2", "3"));
            
            return task;
        }
        private static ICharacteristic CreateRequirement(CharacteristicType type, object value)
        {
            ICharacteristic ch = CoreAccessor.CreateCharacteristic();
            ch.Type = type;
            ch.Value = value;
            return ch;
        }
        private static ITestInfo CreateTest(string input, string output)
        {
            ITestInfo test = CoreAccessor.CreateTestInfo();
            test.Input = input;
            test.Output = output;
            return test;
        }
    }
}
