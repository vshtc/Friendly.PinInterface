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
    public class MapIInstanceGenerateTest
    {
        //target code.
        public interface IA
        {
            string CnvToString(int value);
        }

        public interface IB
        {
            IA A { get; }
        }

        class A : IA
        {
            public string CnvToString(int value)
            {
                return value.ToString();
            }
        }

        class B : IB
        {
            A a = new A();
            public IA A { get { return a; } }
        }

        WindowsAppFriend _app;

        [TestInitialize]
        public void TestInitialize()
        {
            _app = new WindowsAppFriend(Process.Start("Target.exe"));
            WindowsAppExpander.LoadAssembly(_app, GetType().Assembly);
            MapIInstance.GenerateIInstancePlus(typeof(IA), typeof(IB));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Process.GetProcessById(_app.ProcessId).CloseMainWindow();
        }

        [TestMethod]
        public void TestAdd()
        {
            AppVar v = _app.Type<B>()();
            var ib = v.FindPin<IB>();
            Assert.AreEqual("1", ib.A.CnvToString(1));
        }

        [TestMethod]
        public void TestCastIInstance()
        {
            AppVar v = _app.Type<B>()();
            var ib = v.FindPin<IB>();
            var ii = (IInstance)ib;
            Assert.AreEqual("1", (string)ii.Dynamic().A.CnvToString(1));
        }

        //@@@Generic interface
    }
}
