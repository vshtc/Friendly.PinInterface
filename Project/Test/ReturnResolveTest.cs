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
    public class ReturnResolveTest
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
            public static Data Create(int a)
            {
                return new Data() { A = a };
            }
        }

        interface ITargetSerialize
        {
            Data Create(int a);
        }

        [TestMethod]
        public void Serializable()
        {
            ITargetSerialize target = _app.Pin<ITargetSerialize, Target>();
            Data data = target.Create(5);
            Assert.AreEqual(5, data.A);
        }

        interface IData
        {
            int A { get; set; }
        }

        interface ITargetInterface
        {
            IData Create(int a);
        }

        [TestMethod]
        public void Interface()
        {
            ITargetInterface target = _app.Pin<ITargetInterface, Target>();
            IData data = target.Create(5);
            Assert.AreEqual(5, data.A);
        }

        interface ITargetAppVar
        {
            AppVar Create(int a);
        }

        [TestMethod]
        public void AppVar()
        {
            ITargetAppVar target = _app.Pin<ITargetAppVar, Target>();
            AppVar data = target.Create(5);
            Assert.AreEqual(5, (int)data.Dynamic().A);
        }

        class DataWrapper
        {
             AppVar AppVar { get; set; }
            public DataWrapper(AppVar appVar)
            {
                AppVar = appVar;
            }
            public int A { get { return AppVar.Dynamic().A; } }
        }

        interface ITargetWrapper
        {
            DataWrapper Create(int a);
        }

        [TestMethod]
        public void WrapperAndAppVarOwner()
        {
            ITargetWrapper target = _app.Pin<ITargetWrapper, Target>();
            DataWrapper data = target.Create(5);
            Assert.AreEqual(5, (int)data.A);
        }
    }
}
