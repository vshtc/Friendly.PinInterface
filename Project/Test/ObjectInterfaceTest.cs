using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using System.Diagnostics;
using VSHTC.Friendly.PinInterface;

namespace Test
{
    [TestClass]
    public class ObjectInterfaceTest
    {
        WindowsAppFriend _app;
        IWindow _window;
        IWindow _window2;

        [TestInitialize]
        public void TestInitialize()
        {
            _app = new WindowsAppFriend(Process.Start("Target.exe"));
            _window = _app.Pin<IApplicationStatic, Application>().Current.MainWindow;
            _window2 = _app.Pin<IApplicationStatic, Application>().Current.MainWindow;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Process.GetProcessById(_app.ProcessId).CloseMainWindow();
        }

        public interface IWindow
        {
            string Title { get; set; }
        }

        public interface IApplication
        {
            IWindow MainWindow { get; set; }
        }

        public interface IApplicationStatic
        {
            IApplication Current { get; }
        }

        [TestMethod]
        public void Equals_IsProxy()
        {
            Assert.IsTrue(_window.Equals(_window2));
        }

        [TestMethod]
        public void ToString_IsProxy()
        {
            Assert.AreEqual("Target.MainWindow", _window.ToString());
        }

        [TestMethod]
        public void GetHashCode_IsProxy()
        {
            Assert.AreEqual((int)PinHelper.GetAppVar(_window).Dynamic().GetHashCode(), _window.GetHashCode());
        }

        [TestMethod]
        public void GetType_IsNotProxy()
        {
            Assert.AreEqual(typeof(IWindow), _window.GetType());
        }
    }
}
