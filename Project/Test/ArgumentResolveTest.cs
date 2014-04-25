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
    public class ArgumentResolveTest
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
            public Data Create(int a)
            {
                return new Data() { A = a };
            }
            public int GetA(Data data)
            {
                return data.A;
            }
        }

        interface ITargetSerialize
        {
            Data Create(int a);
            int GetA(Data data);
        }

        [TestMethod]
        public void Serializable()
        {
            AppVar v = _app.Type<Target>()();
            ITargetSerialize target = v.Pin<ITargetSerialize>();
            Data data = target.Create(5);
            Assert.AreEqual(5, data.A);
            Assert.AreEqual(5, target.GetA(data));
        }

        interface IData
        {
            int A { get; set; }
        }

        interface ITargetInterface
        {
            IData Create(int a);
            int GetA(IData data);
        }

        [TestMethod]
        public void Interface()
        {
            AppVar v = _app.Type<Target>()();
            ITargetInterface target = v.Pin<ITargetInterface>();
            IData data = target.Create(5);
            Assert.AreEqual(5, data.A);
            Assert.AreEqual(5, target.GetA(data));
        }

        interface ITargetAppVar
        {
            AppVar Create(int a);
            int GetA(AppVar data);
        }

        [TestMethod]
        public void AppVar()
        {
            AppVar v = _app.Type<Target>()();
            ITargetAppVar target = v.Pin<ITargetAppVar>();
            dynamic data = target.Create(5).Dynamic();
            Assert.AreEqual(5, (int)data.A);
            Assert.AreEqual(5, target.GetA(data));
        }

        class DataWrapperAndAppVarOwner : IAppVarOwner
        {
            public AppVar AppVar { get; set; }
            public DataWrapperAndAppVarOwner(AppVar appVar)
            {
                AppVar = appVar;
            }
            public int A { get { return AppVar.Dynamic().A; } }
        }

        interface ITargetWrapper
        {
            DataWrapperAndAppVarOwner Create(int a);
            int GetA(DataWrapperAndAppVarOwner data);
        }

        [TestMethod]
        public void WrapperAndAppVarOwner()
        {
            AppVar v = _app.Type<Target>()();
            ITargetWrapper target = v.Pin<ITargetWrapper>();
            DataWrapperAndAppVarOwner data = target.Create(5);
            Assert.AreEqual(5, (int)data.A);
            Assert.AreEqual(5, target.GetA(data));
        }
    }
}
