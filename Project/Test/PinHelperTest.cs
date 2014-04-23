using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using System.Diagnostics;
using VSHTC.Friendly.PinInterface;
using System.Collections.Generic;

namespace Test
{
    [TestClass]
    public class PinHelperTest
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
        class Target
        {
            public int A { get; set; }
            public Target(int a)
            {
                A = a;
            }
            public int Func()
            {
                return 1;
            }
            public static int FuncStatic()
            {
                return 2;
            }
            public static void Get(out Target target)
            {
                target = new Target(10);
            }
        }

        [TargetType("Test.PinHelperTest+Target")]
        class Target_
        {
            internal interface Instance
            {
                int A { get; set; }
                int Func();
            }
            internal interface Constructor
            {
                Instance New(int a);
            }
            internal interface Static
            {
                int FuncStatic();
                void Get(out Instance target);
            }
        }

        class TargetNoAttr_
        {
            internal interface Instance
            {
                int A { get; set; }
                int Func();
            }
            internal interface Constructor
            {
                Instance New(int a);
            }
            internal interface Static
            {
                int FuncStatic();
                void Get(out Instance target);
            }
        }

        [TestMethod]
        public void PinStatic()
        {
            var target = PinHelper.Pin<Target_.Static>(_app);
            Assert.AreEqual(2, target.FuncStatic());

            var targetNoAttr = PinHelper.Pin<TargetNoAttr_.Static, Target>(_app);
            Assert.AreEqual(2, target.FuncStatic());

            targetNoAttr = PinHelper.Pin<TargetNoAttr_.Static>(_app, typeof(Target));
            Assert.AreEqual(2, target.FuncStatic());

            targetNoAttr = PinHelper.Pin<TargetNoAttr_.Static>(_app, typeof(Target).ToString());
            Assert.AreEqual(2, target.FuncStatic());
        }

        [TestMethod]
        public void PinConstructor()
        {
            var target = PinHelper.PinConstructor<Target_.Constructor>(_app);
            var instance = target.New(1);
            Assert.AreEqual(1, instance.A);

            var targetNoAttr = PinHelper.PinConstructor<TargetNoAttr_.Constructor, Target>(_app);
            var instanceNoAttr = targetNoAttr.New(2);
            Assert.AreEqual(2, instanceNoAttr.A);

            targetNoAttr = PinHelper.PinConstructor<TargetNoAttr_.Constructor>(_app, typeof(Target));
            instanceNoAttr = targetNoAttr.New(3);
            Assert.AreEqual(3, instanceNoAttr.A);

            targetNoAttr = PinHelper.PinConstructor<TargetNoAttr_.Constructor>(_app, typeof(Target).ToString());
            instanceNoAttr = targetNoAttr.New(4);
            Assert.AreEqual(4, instanceNoAttr.A);

        }

        [TestMethod]
        public void PinInstance()
        {
            AppVar appVar = _app.Type<Target>()(3);
            var target = PinHelper.Pin<Target_.Instance>(appVar);
            Assert.AreEqual(3, target.A);
        }

        [TestMethod]
        public void GetAppVarTest()
        {
            AppVar appVar = _app.Type<Target>()(3);
            var target = PinHelper.Pin<Target_.Instance>(appVar);
            appVar = PinHelper.GetAppVar(target);
            Assert.AreEqual(3, ((Target)appVar.Core).A);
        }

        [TestMethod]
        public void GetValue()
        {
            AppVar appVar = _app.Type<Target>()(3);
            var target = PinHelper.Pin<Target_.Instance>(appVar);
            appVar = PinHelper.GetAppVar(target);
            Assert.AreEqual(3, PinHelper.GetValue<Target>(target).A);
        }

        [TestMethod]
        public void PinStaticException()
        {
            TestUtility.TestExceptionMessage(() => { _app.Pin<TargetNoAttr_.Static>(); },
                "Not found target type.\r\nOrder by TargetTypeAttribute,Or Use other Pin method which can specify the target type.",
                "対象の型を見つけることができませんでした。\r\nTargetTypeAttributeで指定するか、対象の型を指定できるPinメソッドを使用してください。");
        }

        [TestMethod]
        public void PinConstructorException()
        {
            TestUtility.TestExceptionMessage(() => { _app.Pin<TargetNoAttr_.Constructor>(); },
                "Not found target type.\r\nOrder by TargetTypeAttribute,Or Use other Pin method which can specify the target type.",
                "対象の型を見つけることができませんでした。\r\nTargetTypeAttributeで指定するか、対象の型を指定できるPinメソッドを使用してください。");
        }
    }
}
