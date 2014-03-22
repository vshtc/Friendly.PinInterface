using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using Codeer.Friendly.Windows;
using System.Diagnostics;
using VSHTC.Friendly.PinInterface;
using Codeer.Friendly;
using VSHTC.Friendly.PinInterface.Inside;

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

            IObject DataContext { get; set; }

            ITextBlock UserNameBlock { get; set; }
        }

        public interface ITextBlock {
            string Text { get; set; }
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

        public interface IMainWindowViewModel {
            string Name { get; set; }
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
        public void DataContextを触ってみる() {
            var appStatic = _app.Pin<IApplicationStatic>();
            var window = appStatic.Current.MainWindow;
            var context = window.DataContext;
            //var model = (IMainWindowViewModel)context;
            var model = context.Cast<IMainWindowViewModel>();
            model.Name = "Bar";
            //Assert.AreEqual("Bar", window.UserNameBlock.Text);
        }
    }


}
