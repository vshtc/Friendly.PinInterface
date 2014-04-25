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
    public class ModifyAsyncTest
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

        interface IMessageBox
        {
            MessageBoxResult Show(string messageBoxText);
        }
        
        [TestMethod]
        public void Async()
        {
            IMessageBox msg = _app.Pin<IMessageBox, MessageBox>();
            WindowControl top = WindowControl.FromZTop(_app);
            Async async = PinHelper.AsyncNext(msg);
            msg.Show("");
            WindowControl next = top.WaitForNextModal();
            new NativeMessageBox(next).EmulateButtonClick("OK");
            async.WaitForCompletion();
        }

        class TargetInstance
        {
            public void Show()
            {
                MessageBox.Show("");
            }
        }

        interface ITargetInstance
        {
            void Show();
        }

        [TestMethod]
        public void Instance()
        {
            ITargetInstance target = PinHelper.Pin<ITargetInstance>(_app.Type<TargetInstance>()());
            WindowControl w = WindowControl.FromZTop(_app);
            Async async = PinHelper.AsyncNext(target);
            target.Show();
            new NativeMessageBox(w.WaitForNextModal()).EmulateButtonClick("OK");
            async.WaitForCompletion();
        }

        interface ITargetInstanceConstructor
        {
            ITargetInstance New();
        }

        [TestMethod]
        public void Constructor()
        {
            var constructor = _app.PinConstructor<ITargetInstanceConstructor, TargetInstance>();
            try
            {
                PinHelper.AsyncNext(constructor);
                Assert.Fail();
            }
            catch
            { }//@@@msg
        }

        [Serializable]
        class Data 
        {
            public int A { get; set; }
        }

        class Target
        {
            static Data Func(out Data data, out int a)
            {
                data = new Data() { A = 1 };
                a = 2;
                return new Data() { A = 3 };
            }
        }

        interface ITargetSerialize
        {
            Data Func(out Data data, out int a);
        }

        [TestMethod]
        public void Serialize()
        {
            var target = _app.Pin<ITargetSerialize, Target>();
            Async async  = PinHelper.AsyncNext(target);
            Data arg;
            int a;
            Data ret = target.Func(out arg, out a);
            async.WaitForCompletion();
            Assert.IsNull(ret);
            Assert.AreEqual(0, a);
            Assert.IsNull(arg);
        }

        interface IData { }
        interface ITargetInterface
        {
            IData Func(out IData data, out int a);
        }

        [TestMethod]
        public void Interface()
        {
            var target = _app.Pin<ITargetInterface, Target>();
            Async async = PinHelper.AsyncNext(target);
            IData arg;
            int a;
            IData ret = target.Func(out arg, out a);
            async.WaitForCompletion();
            Assert.IsNull(ret);
            Assert.IsNull(arg);
        }

        interface ITargetAppVar
        {
            AppVar Func(out AppVar data, out int a);
        }

        [TestMethod]
        public void AppVar()
        {
            var target = _app.Pin<ITargetAppVar, Target>();
            Async async = PinHelper.AsyncNext(target);
            AppVar arg;
            int a;
            AppVar ret = target.Func(out arg, out a);
            async.WaitForCompletion();
            Assert.AreEqual(3, (int)ret.Dynamic().A);
            Assert.AreEqual(1, (int)arg.Dynamic().A);
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
            DataWrapper Func(out DataWrapper data, out int a);
        }

        [TestMethod]
        public void Wrapper()
        {
            var target = _app.Pin<ITargetWrapper, Target>();
            Async async = PinHelper.AsyncNext(target);
            DataWrapper arg;
            int a;
            DataWrapper ret = target.Func(out arg, out a);
            async.WaitForCompletion();
            Assert.IsNull(ret);
            Assert.IsNull(arg);
        }
    }
}