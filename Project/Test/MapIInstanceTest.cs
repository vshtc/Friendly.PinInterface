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
    public class MapIInstanceTest
    {
        //target code.
        interface IA
        {
            string CnvToString(int value);
        }

        interface IB
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

        //operator code.
        interface IA_ : IA, IInstance { }
        interface IB_ : IB, IInstance { }


        WindowsAppFriend _app;

        [TestInitialize]
        public void TestInitialize()
        {
            _app = new WindowsAppFriend(Process.Start("Target.exe"));
            WindowsAppExpander.LoadAssembly(_app, GetType().Assembly);
            MapIInstance.EntryIInstancePlus(typeof(IA_), typeof(IB_));
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
        //@@@out, ref引数, 対象タイプ推測(推測できないこと)。
    }
}
