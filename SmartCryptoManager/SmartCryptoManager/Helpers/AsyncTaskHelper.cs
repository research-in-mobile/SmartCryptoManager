using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCryptoManager.Helpers
{
    public class AsyncTaskHelper
    {
        public delegate void TaskCompletedHandler(object sender, TaskCompletedEventArgs e);

        public event TaskCompletedHandler TaskCompleted;

        public void OnTaskCompleted(TaskCompletedEventArgs e)
        {
            TaskCompleted?.Invoke(this, e);
        }

        public class TaskCompletedEventArgs : EventArgs
        {
            public TaskCompletedEventArgs(Task task)
            {
                FinsihedTask = task;
            }

            public Task FinsihedTask { get; private set; }
        }

        public async Task TaskLoaderBufferAsync<T>(IEnumerable<Task<T>> tasks, CancellationToken ct)
        {
            List<Task<T>> taskList  = tasks.ToList();

            while (taskList.Count > 0)
            {
                // Identify the first task that completes.
                Task<T> finishedTask = await Task.WhenAny(taskList);

                taskList.Remove(finishedTask);

                TaskCompleted?.Invoke(this, new TaskCompletedEventArgs(finishedTask));

            }
        }

        private async Task LoadTest(string symbol)
        {
            //var loadCoinDetailTask = CryptoCompareClient.Instance.Coins()
            var tasks = new List<Task<string>>()
            {
                WriteLineTestAsync(0, 3000),
                WriteLineTestAsync(1, 4000),
                WriteLineTestAsync(2, 1000),
                WriteLineTestAsync(3, 200),
                WriteLineTestAsync(4, 5000),
                WriteLineTestAsync(5, 100)
            };

            while (tasks.Count > 0)
            {
                var finishedTask = await Task.WhenAny(tasks);

                tasks.Remove(finishedTask);

                var result = await finishedTask;

                Console.WriteLine(result);
            }


        }



        private async Task<string> WriteLineTestAsync(int taskNumber, int delayMilli)
        {
            Random rnd = new Random();

            await Task.Delay(delayMilli);

            return string.Format("Task {0} completed", taskNumber);
        }
    }
}
