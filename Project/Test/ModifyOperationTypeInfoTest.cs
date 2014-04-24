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
    public class ModifyOperationTypeInfoTest
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
        class Data { }

        class Target
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
            static int FuncRefOut(ref string value1, out string value2)
            {
                value2 = null;
                return 1;
            }
            static int FuncRefOut(ref Data value1, out Data value2)
            {
                value2 = null;
                return 2;
            }
        }

        [TargetType("Test.ModifyOperationTypeInfoTest+Data")]
        interface IData { }

        interface ITarget
        {
            int Func();
            int Func(string value);
            int Func(Data value);
            int Func(IData value);
            int FuncRefOut(ref string value1, out string value2);
            int FuncRefOut(ref Data value1, out Data value2);
            int FuncRefOut(ref IData value1, out IData value2);
        }

        [TestMethod]
        public void TestOrder()
        {
            var target = _app.Pin<ITarget, Target>();
            PinHelper.OperationTypeInfoNext(target,
                new OperationTypeInfo("Test.ModifyOperationTypeInfoTest+Target",
                    typeof(string).FullName + "&",
                    typeof(string).FullName + "&"));
            string value1 = null;
            string value2 = null;
            Assert.AreEqual(1, target.FuncRefOut(ref value1, out value2));
        }

        [TestMethod]
        public void TestAuto()
        {
            var target = _app.Pin<ITarget, Target>();

            PinHelper.OperationTypeInfoNext(target);
            Assert.AreEqual(1, target.Func((string)null));

            PinHelper.OperationTypeInfoNext(target);
            Assert.AreEqual(2, target.Func((Data)null));

            PinHelper.OperationTypeInfoNext(target);
            Assert.AreEqual(2, target.Func((IData)null));

            string value1 = null;
            string value2 = null;
            PinHelper.OperationTypeInfoNext(target);
            Assert.AreEqual(1, target.FuncRefOut(ref value1, out value2));
        }

        //@@@仕様としてAlwaysをつくるか？
            //→まあ面倒だけどあった方がいいよね。

        //Instance、Static、Constructorに対して実行→軽くでいい。


        /*
        class TargetInstance
        {
            public int _value;

            TargetInstance() { }

            TargetInstance(string data) { _value = 1; }

            TargetInstance(Data data) { _value = 2; }

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

        interface ITargetInstance
        {
            int Func();
            int Func(string value);
            int Func(Data value);
            int _value { get; }
        }

        [TargetType("Test.TesIModifyOperationTypeInfo+TargetInstance")]
        interface ITargetInstanceConstructor
        {
            ITargetInstance New(string value);
            ITargetInstance New(Data value);
        }

        [TargetType("Test.TesIModifyOperationTypeInfo+TargetInstance")]
        interface ITargetInstanceAuto
        {
            int Func();
            int Func(string value);
            int Func(Data value);
        }

        [TestMethod]
        public void OperationTypeInfoStaticAuto()
        {
            var target = _app.Pin<ITargetStatic>(typeof(TargetStatic));
            target.IsAutoOperationTypeInfo = true;
            Assert.AreEqual(1, target.Func((string)null));
            Assert.AreEqual(2, target.Func((Data)null));
        }

        [TestMethod]
        public void OperationTypeInfoInstanceAuto()
        {
            var target = ((AppVar)_app.Type<TargetInstance>()()).Pin<ITargetInstanceAuto>();
            target.IsAutoOperationTypeInfo = true;
            Assert.AreEqual(1, target.Func((string)null));
            Assert.AreEqual(2, target.Func((Data)null));
        }

        [TestMethod]
        public void OperationTypeInfo()
        {
            var target = ((AppVar)_app.Type<TargetInstance>()()).Pin<ITargetInstance>();
            target.OperationTypeInfoNext(typeof(TargetInstance).FullName);
            Assert.AreEqual(0, target.Func());
            target.OperationTypeInfoNext(typeof(TargetInstance).FullName, typeof(string).FullName);
            Assert.AreEqual(1, target.Func((string)null));
            target.OperationTypeInfoNext(typeof(TargetInstance).FullName, typeof(Data).FullName);
            Assert.AreEqual(2, target.Func((Data)null));
        }

        [TestMethod]
        public void OperationTypeInfoStaticAutoRefOut()
        {
            var target = _app.Pin<ITargetStatic>(typeof(TargetStatic));
            target.IsAutoOperationTypeInfo = true;
            {
                string value1 = null;
                string value2 = null;
                Assert.AreEqual(1, target.FuncRefOut(ref value1, out value2));
            }
            {
                Data value1 = null;
                Data value2 = null;
                Assert.AreEqual(2, target.FuncRefOut(ref value1, out value2));
            }
        }

        [TestMethod]
        public void OperationTypeInfoConstructorAuto()
        {
            var target = _app.Pin<ITargetInstanceConstructor>();
            target.IsAutoOperationTypeInfo = true;
            {

                var ret = target.New((string)null);
                ret.IsAutoOperationTypeInfo = true;
                Assert.AreEqual(1, ret._value);
            }
            {
                var ret = target.New((Data)null);
                ret.IsAutoOperationTypeInfo = true;
                Assert.AreEqual(2, ret._value);
            }
        }*/
    }
}
