using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using System.Diagnostics;
using VSHTC.Friendly.PinInterface;
using VSHTC.Friendly.PinInterface.Inside;
using System.Windows.Documents;
using System.Collections.Generic;
using Codeer.Friendly.Windows.Grasp;
using Codeer.Friendly.Windows.NativeStandardControls;

namespace Test
{
    [TestClass]
    public class TesIModifyAsync
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

        interface IMessageBox : IStatic
        {
            MessageBoxResult Show(string messageBoxText);
        }

        
        [Serializable]
        class Data { }

        interface IData : IInstance{ }

        class Target
        {
            void Show()
            {
                MessageBox.Show("");
            }
            int Func(ref int vale, out Data data)
            {
                vale = 1;
                data = new Data();
                return 2;
            }
        }

        interface ITarget : IInstance
        {
            void Show();
            int Func(ref int vale, out Data data);
            IInstance Func(ref IInstance vale, out IData data);
        }

        [TestMethod]
        public void AsyncStatic()
        {
            IMessageBox msg = _app.Pin<IMessageBox, MessageBox>();
            WindowControl top = WindowControl.FromZTop(_app);
            Async async = msg.AsyncNext();
            msg.Show("");
            WindowControl next = top.WaitForNextModal();
            new NativeMessageBox(next).EmulateButtonClick("OK");
            async.WaitForCompletion();
        }

        [TestMethod]
        public void AsyncInstance()
        {
            var target = ((AppVar)_app.Type<Target>()()).Pin<ITarget>();
            WindowControl top = WindowControl.FromZTop(_app);
            Async async = target.AsyncNext();
            target.Show();
            WindowControl next = top.WaitForNextModal();
            new NativeMessageBox(next).EmulateButtonClick("OK");
            async.WaitForCompletion();
        }

        [TestMethod]
        public void AsyncSerialize()
        {
            var target = ((AppVar)_app.Type<Target>()()).Pin<ITarget>();
            Async async = target.AsyncNext();
            int value = 0;
            Data data = null;
            int ret = target.Func(ref value, out data);
            Assert.AreEqual(0, ret);
            Assert.AreEqual(0, value);
            Assert.IsNull(data);
        }

        [TestMethod]
        public void AsyncInterface()
        {
            var target = ((AppVar)_app.Type<Target>()()).Pin<ITarget>();
            Async async = target.AsyncNext();
            IInstance value = null;
            IData data = null;
            IInstance ret = target.Func(ref value, out data);
            Assert.AreEqual(2, ret.Cast<int>());
            Assert.AreEqual(1, value.Cast<int>());
            Assert.IsNotNull(data.Cast<Data>());
        }
    }
}