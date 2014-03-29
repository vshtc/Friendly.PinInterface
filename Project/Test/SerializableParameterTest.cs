using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using System.Diagnostics;
using VSHTC.Friendly.PinInterface;

namespace Test
{
    [TestClass]
    public class SerializableParameterTest
    {
        WindowsAppFriend _app;

        [TestInitialize]
        public void TestInitialize()
        {
            _app = new WindowsAppFriend(Process.Start("Target.exe"));
            WindowsAppExpander.LoadAssembly(_app, GetType().Assembly);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Process.GetProcessById(_app.ProcessId).CloseMainWindow();
        }

        [Serializable]
        class Data
        {
            public int A { get; set; }
            public string B { get; set; }
        }

        class Target
        {
            public Data Create(int a, string b)
            {
                return new Data() { A = a, B = b };
            }
            public int GetA(Data data)
            {
                return data.A;
            }
            public string GetB(Data data)
            {
                return data.B;
            }
        }

        interface ITarget : IInstance
        {
            Data Create(int a, string b);
            int GetA(Data data);
            string GetB(Data data);
        }

        [TestMethod]
        public void SerializableReturnValueAndArguments()
        {
            AppVar v = _app.Type<Target>()();
            ITarget target = v.Pin<ITarget>();
            Data data = target.Create(5, "X");
            Assert.AreEqual(5, data.A);
            Assert.AreEqual("X", data.B);
            Assert.AreEqual(5, target.GetA(data));
            Assert.AreEqual("X", target.GetB(data));
        }
    }
}
