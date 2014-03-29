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
    public class OutParameterTest
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
            public void Create(int a, string b, out Data data)
            {
                data = new Data() { A = a, B = b };
            }
            public void GetOut(Data data, out int a, out string b)
            {
                a = data.A;
                b = data.B;
            }
        }

        interface IData : IInstance
        {
            int A { get; set; }
            string B { get; set; }
        }

        interface ITarget : IInstance
        {
            void Create(int a, string b, out Data data);
            void Create(int a, string b, out IData data);
            void GetOut(Data data, out int a, out string b);
        }

        [TestMethod]
        public void OutSerialize()
        {
            AppVar v = _app.Type<Target>()();
            ITarget target = v.Pin<ITarget>();
            Data data;
            target.Create(5, "X", out data);
            Assert.AreEqual(5, data.A);
            Assert.AreEqual("X", data.B);
            int a;
            string b;
            target.GetOut(data, out a, out b);
            Assert.AreEqual(5, a);
            Assert.AreEqual("X", b);
        }

        [TestMethod]
        public void OutReference()
        {
            AppVar v = _app.Type<Target>()();
            ITarget target = v.Pin<ITarget>();
            IData data;
            target.Create(5, "X", out data);
            Assert.AreEqual(5, data.A);
            Assert.AreEqual("X", data.B);

            //バッファにデータが入っている場合
            target.Create(6, "Y", out data);
            Assert.AreEqual(6, data.A);
            Assert.AreEqual("Y", data.B);
        }
    }
}
