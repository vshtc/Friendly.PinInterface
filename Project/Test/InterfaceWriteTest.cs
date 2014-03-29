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
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Process.GetProcessById(_app.ProcessId).CloseMainWindow();
        }

        public interface IWindow : IInstance
        {
            string Title { get; set; }
            ITextBlock UserNameBlock { get; set; }
        }

        public interface ITextBlock : IInstance
        {
            string Text { get; set; }
        }

        public interface IApplication : IInstance
        {
            IWindow MainWindow { get; set; }
        }

        public interface IApplicationStatic : IStatic
        {
            IApplication Current { get; }
        }

        [TestMethod]
        public void PropertyAndFieldEqually()
        {
            var appStatic = _app.Pin<IApplicationStatic, Application>();
            var window = appStatic.Current.MainWindow;
            Assert.AreEqual("Foo", window.UserNameBlock.Text);
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
