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
    public class UsabilityTest
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

        public interface IWindow : IAppVarOwner
        {
            string Title { get; set; }
        }

        public interface IApplication : IAppVarOwner
        {
            IWindow MainWindow { get; set; }
        }

        [ProxyTraget("System.Windows.Application")]
        public interface IApplicationStatic
        {
            IApplication Current { get; }
        }

        [TestMethod]
        public void こんな使い勝手でいかかでしょう()
        {
            var appStatic = _app.Pin<IApplicationStatic>();
            var window = appStatic.Current.MainWindow;
            window.Title = "TestTitle";
            Assert.AreEqual("TestTitle", window.Title);
        }

        [TestMethod]
        public void AppVarはこんな感じ()
        {
            AppVar main = _app.Type<Application>().Current.MainWindow;
            var window = main.Pin<IWindow>();
            window.Title = "TestTitle";
            Assert.AreEqual("TestTitle", window.Title);
        }

        [TestMethod]
        public void dynamicではこんな感じ()
        {
            dynamic main = _app.Type<Application>().Current.MainWindow;
            var window = InterfaceHelper.Pin<IWindow>(main);
            window.Title = "TestTitle";
            Assert.AreEqual("TestTitle", window.Title);
        }

        [TestMethod]
        public void objectのメソッド_GetTypeだけはプロクシしない()
        {
            var window = _app.Pin<IApplicationStatic>().Current.MainWindow;
            var window2 = _app.Pin<IApplicationStatic>().Current.MainWindow;

            //参照先のそれぞれのメソッドを呼び出す。
            Assert.AreEqual("Target.MainWindow", window.ToString());
            Assert.IsTrue(window.Equals(window2));
            Assert.AreEqual((int)window.Dynamic().GetHashCode(), window.GetHashCode());

            //これだけはプロクシしない。
            //タイプはシリアライズして持ってこれない場合がある。
            //かつシステム的なメソッドなので、意図せず呼び出される場合がある。
            Assert.AreEqual(typeof(IWindow), window.GetType());
        }
    }
}
