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
    public class ReferenceParameterTest
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

        interface IData : IInstance
        {
            int A { get; set; }
            string B { get; set; }
        }

        interface ITarget : IInstance
        {
            IData Create(int a, string b);
            int GetA(IData data);
            string GetB(IData data);
        }

        [TestMethod]
        public void 参照としてインターフェイスを戻り値と引数につかう()
        {
            AppVar v = _app.Type<Target>()();
            ITarget target = v.Pin<ITarget>();
            IData data = target.Create(5, "X");
            Assert.AreEqual(5, data.A);
            Assert.AreEqual("X", data.B);
            Assert.AreEqual(5, target.GetA(data));
            Assert.AreEqual("X", target.GetB(data));
        }

        [TestMethod]
        public void 既存のFriendlyの操作に引数として渡すことができる()
        {
            AppVar v = _app.Type<Target>()();
            ITarget target = v.Pin<ITarget>();
            IData data = target.Create(5, "X");
            Assert.AreEqual(5, (int)target.Dynamic().GetA(data));
            Assert.AreEqual("X", (string)target.Dynamic().GetB(data));
        }
    }
}
