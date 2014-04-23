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

namespace Test
{
    [TestClass]
    public class PinHelperExtensionsTest
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

        [TargetType("Test.PinHelperExtensionsTest+Target")]
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
            var target = _app.Pin<Target_.Static>();
            Assert.AreEqual(2, target.FuncStatic());

            var targetNoAttr = _app.Pin<TargetNoAttr_.Static, Target>();
            Assert.AreEqual(2, target.FuncStatic());

            targetNoAttr = _app.Pin<TargetNoAttr_.Static>(typeof(Target));
            Assert.AreEqual(2, target.FuncStatic());

            targetNoAttr = _app.Pin<TargetNoAttr_.Static>(typeof(Target).ToString());
            Assert.AreEqual(2, target.FuncStatic());
        }

        [TestMethod]
        public void PinConstructor()
        {
            var target = _app.PinConstructor<Target_.Constructor>();
            var instance = target.New(1);
            Assert.AreEqual(1, instance.A);

            var targetNoAttr = _app.PinConstructor<TargetNoAttr_.Constructor, Target>();
            var instanceNoAttr = targetNoAttr.New(2);
            Assert.AreEqual(2, instanceNoAttr.A);

            targetNoAttr = _app.PinConstructor<TargetNoAttr_.Constructor>(typeof(Target));
            instanceNoAttr = targetNoAttr.New(3);
            Assert.AreEqual(3, instanceNoAttr.A);

            targetNoAttr = _app.PinConstructor<TargetNoAttr_.Constructor>(typeof(Target).ToString());
            instanceNoAttr = targetNoAttr.New(4);
            Assert.AreEqual(4, instanceNoAttr.A);
        }

        [TestMethod]
        public void PinInstance()
        {
            AppVar appVar = _app.Type<Target>()(3);
            var target = appVar.Pin<Target_.Instance>();
            Assert.AreEqual(3, target.A);
        }
    }
}
