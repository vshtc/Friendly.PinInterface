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
    public class NullTest
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
            public static Data Get(out Data dataOut, ref Data dataRef)
            {
                dataOut = null;
                dataRef = null;
                return null;
            }
        }

        interface IData
        {
            int A { get; set; }
        }

        interface ITarget
        {
            IData Get(out IData dataOut, ref IData dataRef);
        }

        [TestMethod]
        public void InterfaceNull()
        {
            ITarget target = _app.Pin<ITarget, Target>();
            IData dataOut;
            IData dataRef = null;
            IData data = target.Get(out dataOut, ref dataRef);
            Assert.IsNull(data);
            Assert.IsNull(dataOut);
            Assert.IsNull(dataRef);
        }

        [TestMethod]
        public void InterfaceRefValueToNull()
        {
            ITarget target = _app.Pin<ITarget, Target>();
            IData dataOut;
            IData dataRef = PinHelper.Pin<IData>(_app.Type<Data>()());
            IData data = target.Get(out dataOut, ref dataRef);
            Assert.IsNull(data);
            Assert.IsNull(dataOut);
            Assert.IsNull(dataRef);
        }

        class DataWrapper : IAppVarOwner
        {
            public AppVar AppVar{get;set;}
            public DataWrapper(AppVar a)
            {
                AppVar = a;
            }
            int A { get { return this.Dynamic().A; } set { this.Dynamic().A = value; } }
        }

        interface ITargetDataWrapper
        {
            DataWrapper Get(out DataWrapper dataOut, ref DataWrapper dataRef);
        }

        [TestMethod]
        public void WrapperNull()
        {
            ITargetDataWrapper target = _app.Pin<ITargetDataWrapper, Target>();
            DataWrapper dataOut;
            DataWrapper dataRef = null;
            DataWrapper data = target.Get(out dataOut, ref dataRef);
            Assert.IsNull(data);
            Assert.IsNull(dataOut);
            Assert.IsNull(dataRef);
        }

        [TestMethod]
        public void WrapperRefValueToNull()
        {
            ITargetDataWrapper target = _app.Pin<ITargetDataWrapper, Target>();
            DataWrapper dataOut;
            DataWrapper dataRef = new DataWrapper(_app.Type<Data>()());
            DataWrapper data = target.Get(out dataOut, ref dataRef);
            Assert.IsNull(data);
            Assert.IsNull(dataOut);
            Assert.IsNull(dataRef);
        }

    }
}
