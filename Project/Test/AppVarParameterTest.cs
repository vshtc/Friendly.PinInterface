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
    public class AppVarParameterTest
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

        interface ITarget : IInstance
        {
            AppVar Create(int a, string b);
            int GetA(AppVar data);
            string GetB(AppVar data);
        }

        interface ITargetDynamic : IInstance
        {
            dynamic Create(int a, string b);
            int GetA(AppVar data);
            string GetB(AppVar data);
        }

        interface ITargetDynamicArg : IInstance
        {
            dynamic Create(int a, string b);
            int GetA(dynamic data);
        }

        [TestMethod]
        public void AppVarを戻り値と引数につかう()
        {
            AppVar v = _app.Type<Target>()();
            ITarget target = v.Pin<ITarget>();
            AppVar data = target.Create(5, "X");
            Assert.AreEqual(5, (int)data.Dynamic().A);
            Assert.AreEqual("X", (string)data.Dynamic().B);
            Assert.AreEqual(5, target.GetA(data));
            Assert.AreEqual("X", target.GetB(data));
        }

        [TestMethod]
        public void dynamicを戻り値につかう()
        {
            AppVar v = _app.Type<Target>()();
            ITargetDynamic target = v.Pin<ITargetDynamic>();
            dynamic data = target.Create(5, "X");
            Assert.AreEqual(5, (int)data.A);
            Assert.AreEqual("X", (string)data.B);
            Assert.AreEqual(5, target.GetA(data));
            Assert.AreEqual("X", target.GetB(data));
        }

        [TestMethod]
        public void dynamicを引数につかう()
        {
            AppVar v = _app.Type<Target>()();
            ITargetDynamicArg target = v.Pin<ITargetDynamicArg>();
            dynamic data = target.Create(5, "X");
            Assert.AreEqual(5, (int)data.A);
        }
    }
}
