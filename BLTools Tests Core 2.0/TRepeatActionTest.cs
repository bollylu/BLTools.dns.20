using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTools.UnitTest
{
    [TestClass]
    public class TRepeatActionTest
    {
        [TestMethod]
        public void Repeat_Sync_ExecIsOrdered()
        {
            List<string> Result = new List<string>();
            int i = 0;
            TRepeatAction Repeat = new TRepeatAction();
            Repeat.ToDo = () =>
            {
                Result.Add(i++.ToString());
            };
            Repeat.Delay = 100;

            Repeat.Start();
            Thread.Sleep(1000);
            Repeat.Cancel();

            Assert.AreEqual(10, Result.Count());
            Assert.AreEqual("0123456789", string.Join("", Result.Select(x => x)));
        }

        [TestMethod]
        public void Repeat_Async_ExecIsNotOrdered()
        {
            List<string> Result = new List<string>();
            TRepeatAction Repeat = new TRepeatAction();
            Repeat.ToDo = async () =>
            {
                Result.Add(( await GetValue() ).ToString());
            };
            Repeat.Delay = 100;

            Repeat.Start();
            Thread.Sleep(10000);
            Repeat.Cancel();

            string Target = string.Join("", Result.Select(x => x));
            Debug.WriteLine(Target);
            Assert.IsTrue(Target.StartsWith("0123456789"));
        }

        private int AsyncI = 0;
        private async Task<int> GetValue()
        {
            Random R = new Random(DateTime.Now.Millisecond);
            await Task.Delay(R.Next(5, 50)).ConfigureAwait(false);
            return AsyncI++;
        }
    }
}
