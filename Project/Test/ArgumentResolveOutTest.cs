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
    public class ArgumentResolveOutTest
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
        }

        class Target
        {
            public static void GetOut(out Data data, out int a, out string b)
            {
                data = new Data() { A = 3 };
                a = 4;
                b = "5";
            }
        }
        interface ITargetSerialize
        {
            void GetOut(out Data data, out int a, out string b);
        }

        [TestMethod]
        public void SerializeTest()
        {
            var target = _app.Pin<ITargetSerialize, Target>();
            Data data;
            int a;
            string b;
            target.GetOut(out data, out a, out b);
            Assert.AreEqual(3, data.A);
            Assert.AreEqual(4, a);
            Assert.AreEqual("5", b);
        }

        interface IData
        {
            int A { get; set; }
        }

        interface ITargetInterface
        {
            void GetOut(out IData data, out int a, out string b);
        }

        [TestMethod]
        public void InterfaceNull()
        {
            var target = _app.Pin<ITargetInterface, Target>();
            IData data;
            int a;
            string b;
            target.GetOut(out data, out a, out b);
            Assert.AreEqual(3, data.A);
        }

        [TestMethod]
        public void InterfaceHasEntity()
        {
            var target = _app.Pin<ITargetInterface, Target>();
            IData data = PinHelper.Pin<IData>(_app.Type<Data>()());
            data.A = 10;
            int a;
            string b;
            target.GetOut(out data, out a, out b);
            Assert.AreEqual(3, data.A);
        }

        interface ITargetAppVar
        {
            void GetOut(out AppVar data, out int a, out string b);
        }

        [TestMethod]
        public void AppVarNull()
        {
            var target = _app.Pin<ITargetAppVar, Target>();
            AppVar data;
            int a;
            string b;
            target.GetOut(out data, out a, out b);
            Assert.AreEqual(3, (int)data.Dynamic().A);
        }

        [TestMethod]
        public void AppVarHasEntity()
        {
            var target = _app.Pin<ITargetAppVar, Target>();
            AppVar data = _app.Type<Data>()();
            data.Dynamic().A = 10;
            int a;
            string b;
            target.GetOut(out data, out a, out b);
            Assert.AreEqual(3, (int)data.Dynamic().A);
        }

        class DataWrapperAndAppVarOwner : IAppVarOwner
        {
            public AppVar AppVar { get; set; }
            public DataWrapperAndAppVarOwner(AppVar appVar)
            {
                AppVar = appVar;
            }
            public int A { get { return AppVar.Dynamic().A; } set { AppVar.Dynamic().A = value; } }
        }

        interface ITargetWrapper
        {
            void GetOut(out DataWrapperAndAppVarOwner data, out int a, out string b);
        }

        [TestMethod]
        public void WrapperNull()
        {
            var target = _app.Pin<ITargetWrapper, Target>();
            DataWrapperAndAppVarOwner data;
            int a;
            string b;
            target.GetOut(out data, out a, out b);
            Assert.AreEqual(3, (int)data.A);
        }

        [TestMethod]
        public void WrapperHasEntity()
        {
            var target = _app.Pin<ITargetWrapper, Target>();
            DataWrapperAndAppVarOwner data = new DataWrapperAndAppVarOwner(_app.Type<Data>()());
            data.A = 10;
            int a;
            string b;
            target.GetOut(out data, out a, out b);
            Assert.AreEqual(3, (int)data.A);
        }
    }
}
