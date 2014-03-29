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
        }

        interface ITarget : IInstance
        {
            int _data { get; set; }
            int Data { get; set; }
        }

        [TestMethod]
        public void PropertyAndFieldEqually()
        {
            var target = ((AppVar)_app.Type<Target>()()).Pin<ITarget>();
            target._data = 100;
            Assert.AreEqual(100, target.Data);
        }

        public interface IIndexAccess : IInstance
        {
            int this[int index] { get; set; }
        }

        [TestMethod]
        public void IndexAccess()
        {
            IIndexAccess list = InterfaceHelper.Pin<IIndexAccess>(_app.Copy(new List<int>(new int[] { 0, 1, 2 })));
            list[1] = 100;
            Assert.AreEqual(100, list[1]);

            IIndexAccess array = InterfaceHelper.Pin<IIndexAccess>(_app.Copy(new int[] { 0, 1, 2 }));
            array[1] = 100;
            Assert.AreEqual(100, array[1]);
        }
    }
}
