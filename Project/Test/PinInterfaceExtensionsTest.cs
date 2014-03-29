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
    public class PinInterfaceExtensionsTest
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

        [TargetType("Test.PinInterfaceExtensionsTest+Target")]
        class Target_
        {
            internal interface Instance : IInstance
            {
                int A { get; set; }
                int Func();
            }
            internal interface Constructor : IConstructor
            {
                Instance New(int a);
            }
            internal interface Static : IStatic
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

            target = _app.Pin<Target_.Static, Target>();
            Assert.AreEqual(2, target.FuncStatic());

            target = _app.Pin<Target_.Static>(typeof(Target));
            Assert.AreEqual(2, target.FuncStatic());

            target = _app.Pin<Target_.Static>(typeof(Target).ToString());
            Assert.AreEqual(2, target.FuncStatic());
        }

        [TestMethod]
        public void PinCopy()
        {
            var target = _app.PinCopy<Target_.Instance>(new Target(5));
            Assert.AreEqual(5, target.A);
        }

        [TestMethod]
        public void PinNull()
        {
            var target = _app.PinNull<Target_.Instance>();
            _app.Pin<Target_.Static>().Get(out target);
            Assert.AreEqual(10, target.A);
        }

        [TestMethod]
        public void PinConstructor()
        {
            var target = _app.Pin<Target_.Constructor>();
            var instance = target.New(3);
            Assert.AreEqual(3, instance.A);
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
