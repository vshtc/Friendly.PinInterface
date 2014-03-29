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
    public class TesIModifyInvoke
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

        //@@@インスタンス版も見る
        [TestMethod]
        public void 非同期実行()
        {
            IMessageBox msg = _app.Pin<IMessageBox, MessageBox>();
            WindowControl top = WindowControl.FromZTop(_app);
            Async async = msg.AsyncNext();
            msg.Show("");
            WindowControl next = top.WaitForNextModal();
            new NativeMessageBox(next).EmulateButtonClick("OK");
            async.WaitForCompletion();
        }


        
        [Serializable]
        class Data{}

        static class TargetStatic
        {
            static int Func()
            {
                return 0;
            }
            static int Func(string value)
            {
                return 1;
            }
            static int Func(Data value)
            {
                return 2;
            }
        }

        interface ITargetStatic : IStatic
        {
            int Func();
            int Func(string value);
            int Func(Data value);
        }

        class TargetInstance
        {
            int Func()
            {
                return 0;
            }
            int Func(string value)
            {
                return 1;
            }
            int Func(Data value)
            {
                return 2;
            }
        }

        interface ITargetInstance : IInstance
        {
            int Func();
            int Func(string value);
            int Func(Data value);
        }

        [TargetType("Test.TesIModifyInvoke+TargetInstance")]
        interface ITargetInstanceAuto : IInstance
        {
            int Func();
            int Func(string value);
            int Func(Data value);
        }


        [TestMethod]
        public void TestOperationTypeInfoStaticAuto()
        {
            var target = _app.Pin<ITargetStatic>(typeof(TargetStatic));
            Assert.AreEqual(1, target.Func((string)null));
            Assert.AreEqual(2, target.Func((Data)null));
        }

        [TestMethod]
        public void TestOperationTypeInfoInstanceAuto()
        {
            var target = ((AppVar)_app.Type<TargetInstance>()()).Pin<ITargetInstanceAuto>();
            Assert.AreEqual(1, target.Func((string)null));
            Assert.AreEqual(2, target.Func((Data)null));
        }

        [TestMethod]
        public void TestOperationTypeInfo()
        {
            var target = ((AppVar)_app.Type<TargetInstance>()()).Pin<ITargetInstance>();
            target.OperationTypeInfoNext(typeof(TargetInstance).FullName);
            Assert.AreEqual(0, target.Func());
            target.OperationTypeInfoNext(typeof(TargetInstance).FullName, typeof(string).FullName);
            Assert.AreEqual(1, target.Func((string)null));
            target.OperationTypeInfoNext(typeof(TargetInstance).FullName, typeof(Data).FullName);
            Assert.AreEqual(2, target.Func((Data)null));
        }

        //@@@コンストラクタも見ておく
    }
}
