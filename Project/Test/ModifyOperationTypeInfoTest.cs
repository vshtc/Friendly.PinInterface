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

        [OperationTypeInfoAuto]
        interface ITargetAlways
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
        public void Order()
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
        public void Auto()
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

        [TestMethod]
        public void AutoAlways()
        {
            var target = _app.Pin<ITarget, Target>();

            PinHelper.SetOperationTypeInfoAlways(target, true);
            Assert.AreEqual(1, target.Func((string)null));
            Assert.AreEqual(2, target.Func((Data)null));
            Assert.AreEqual(2, target.Func((IData)null));
            string value1 = null;
            string value2 = null;
            Assert.AreEqual(1, target.FuncRefOut(ref value1, out value2));
        }

        [TestMethod]
        public void AutoAlwaysAttr()
        {
            var target = _app.Pin<ITargetAlways, Target>();
            Assert.AreEqual(1, target.Func((string)null));
            Assert.AreEqual(2, target.Func((Data)null));
            Assert.AreEqual(2, target.Func((IData)null));
            string value1 = null;
            string value2 = null;
            Assert.AreEqual(1, target.FuncRefOut(ref value1, out value2));
        }

        class TargetInstance
        {
            public int ConstructorNo { get; set; }
            public TargetInstance() { }
            public TargetInstance(string value)
            {
                ConstructorNo = 1;
            }
            public TargetInstance(Data value)
            {
                ConstructorNo = 2;
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

        [TargetType("Test.ModifyOperationTypeInfoTest+TargetInstance")]
        interface ITargetInstance
        {
            int ConstructorNo { get; set; }
            int Func(string value);
            int Func(Data value);
            int Func(IData value);
        }

        [TestMethod]
        public void Instance()
        {
            ITargetInstance target = PinHelper.Pin<ITargetInstance>(_app.Type<TargetInstance>()());
            WindowControl w = WindowControl.FromZTop(_app);
            PinHelper.SetOperationTypeInfoAlways(target, true);
            Assert.AreEqual(1, target.Func((string)null));
            Assert.AreEqual(2, target.Func((Data)null));
            Assert.AreEqual(2, target.Func((IData)null));
        }

        [TargetType("Test.ModifyOperationTypeInfoTest+TargetInstance")]
        interface ITargetInstanceConstructor
        {
            ITargetInstance New(string value);
            ITargetInstance New(Data value);
            ITargetInstance New(IData value);
        }

        [TestMethod]
        public void Constructor()
        {
            var constructor = _app.PinConstructor<ITargetInstanceConstructor>();
            PinHelper.SetOperationTypeInfoAlways(constructor, true);
            var target = constructor.New((string)null);
            Assert.AreEqual(1, target.ConstructorNo);
            target = constructor.New((Data)null);
            Assert.AreEqual(2, target.ConstructorNo);
            target = constructor.New((IData)null);
            Assert.AreEqual(2, target.ConstructorNo);
        }
    }
}
