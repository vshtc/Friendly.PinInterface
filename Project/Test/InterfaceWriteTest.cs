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
    public class InterfaceWriteTest
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

        class Target
        {
            public int _data;
            public int Data { get { return _data; } set { _data = value; } }
            public int Func() { return 10; }
        }

        interface ITarget
        {
            int _data { get; set; }
            int Data { get; set; }
            int Func();
        }

        [TestMethod]
        public void PropertyTest()
        {
            var target = ((AppVar)_app.Type<Target>()()).Pin<ITarget>();
            target._data = 100;
            Assert.AreEqual(100, target._data);
        }

        [TestMethod]
        public void FieldTest()
        {
            var target = ((AppVar)_app.Type<Target>()()).Pin<ITarget>();
            target.Data = 101;
            Assert.AreEqual(101, target.Data);
        }

        [TestMethod]
        public void FuncTest()
        {
            var target = ((AppVar)_app.Type<Target>()()).Pin<ITarget>();
            Assert.AreEqual(10, target.Func());
        }

        public interface IIndexAccess
        {
            int this[int index] { get; set; }
        }

        [TestMethod]
        public void IndexAccess()
        {
            IIndexAccess list = PinHelper.Pin<IIndexAccess>(_app.Copy(new List<int>(new int[] { 0, 1, 2 })));
            list[1] = 100;
            Assert.AreEqual(100, list[1]);

            IIndexAccess array = PinHelper.Pin<IIndexAccess>(_app.Copy(new int[] { 0, 1, 2 }));
            array[1] = 100;
            Assert.AreEqual(100, array[1]);
        }
    }
}
