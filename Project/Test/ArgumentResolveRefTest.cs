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
    public class ArgumentResolveRefTest
    {
        WindowsAppFriend _app;

        [TestInitialize]
        public void TestInitialize()
        {
            _app = new WindowsAppFriend(Process.Start("Target.exe"));
            WindowsAppExpander.LoadAssembly(_app, GetType().Assembly);
            _app.Type<Target>().InputData = null;
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
            public static Data InputData { get; set; }
            public static void GetOut(ref Data data, ref int a, ref string b)
            {
                InputData = data;
                data = new Data() { A = 3 };
                a = 4;
                b = "5";
            }
        }
        interface ITargetSerialize
        {
            void GetOut(ref Data data, ref int a, ref string b);
        }

        [TestMethod]
        public void Serialize()
        {
            var target = _app.Pin<ITargetSerialize, Target>();
            Data data = null;
            int a = 0;
            string b = string.Empty;
            target.GetOut(ref data, ref a, ref b);
            Assert.AreEqual(3, data.A);
            Assert.AreEqual(4, a);
            Assert.AreEqual("5", b);
        }

        [TestMethod]
        public void SerializeHasEntity()
        {
            var target = _app.Pin<ITargetSerialize, Target>();
            Data data = new Data() { A = 100 };
            int a = 0;
            string b = string.Empty;
            target.GetOut(ref data, ref a, ref b);
            Assert.AreEqual(100, (int)_app.Type<Target>().InputData.A);
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
            void GetOut(ref IData data, ref int a, ref string b);
        }

        [TestMethod]
        public void InterfaceNull()
        {
            var target = _app.Pin<ITargetInterface, Target>();
            IData data = null;
            int a = 0;
            string b = string.Empty;
            target.GetOut(ref data, ref a, ref b);
            Assert.AreEqual(3, data.A);
        }

        [TestMethod]
        public void InterfaceHasEntity()
        {
            var target = _app.Pin<ITargetInterface, Target>();
            IData data = PinHelper.Pin<IData>(_app.Type<Data>()());
            data.A = 101;
            int a = 0;
            string b = string.Empty;
            target.GetOut(ref data, ref a, ref b);
            Assert.AreEqual(101, (int)_app.Type<Target>().InputData.A);
            Assert.AreEqual(3, data.A);
        }

        interface ITargetAppVar
        {
            void GetOut(ref AppVar data, ref int a, ref string b);
        }

        [TestMethod]
        public void AppVarNull()
        {
            var target = _app.Pin<ITargetAppVar, Target>();
            AppVar data = null;
            int a = 0;
            string b = string.Empty;
            target.GetOut(ref data, ref a, ref b);
            Assert.AreEqual(3, (int)data.Dynamic().A);
        }

        [TestMethod]
        public void AppVarHasEntity()
        {
            var target = _app.Pin<ITargetAppVar, Target>();
            AppVar data = _app.Type<Data>()();
            data.Dynamic().A = 102;
            int a = 0;
            string b = string.Empty;
            target.GetOut(ref data, ref a, ref b);
            Assert.AreEqual(102, (int)_app.Type<Target>().InputData.A);
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
            void GetOut(ref DataWrapperAndAppVarOwner data, ref int a, ref string b);
        }

        [TestMethod]
        public void WrapperNull()
        {
            var target = _app.Pin<ITargetWrapper, Target>();
            DataWrapperAndAppVarOwner data = null;
            int a = 0;
            string b = string.Empty;
            target.GetOut(ref data, ref a, ref b);
            Assert.AreEqual(3, (int)data.A);
        }

        [TestMethod]
        public void WrapperHasEntity()
        {
            var target = _app.Pin<ITargetWrapper, Target>();
            DataWrapperAndAppVarOwner data = new DataWrapperAndAppVarOwner(_app.Type<Data>()());
            data.A = 103;
            int a = 0;
            string b = string.Empty;
            target.GetOut(ref data, ref a, ref b);
            Assert.AreEqual(103, (int)_app.Type<Target>().InputData.A);
            Assert.AreEqual(3, (int)data.A);
        }
    }
}
