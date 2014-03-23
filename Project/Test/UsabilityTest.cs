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

        public interface IObject : IAppVarOwner { }

        public interface IWindow : IAppVarOwner
        {
            string Title { get; set; }

            IObject DataContext { get; set; }

            ITextBlock UserNameBlock { get; set; }
        }

        public interface ITextBlock : IAppVarOwner
        {
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

        public interface IMainWindowViewModel : IAppVarOwner
        {
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

        [TestMethod]
        public void DataContextを触ってみる() 
        {
            var appStatic = _app.Pin<IApplicationStatic>();
            var window = appStatic.Current.MainWindow;
            var context = window.DataContext;
            var model = context.Cast<IMainWindowViewModel>();
            model.Name = "Bar";
        }

        [TestMethod]
        public void Fieldもプロパティーとして取得できる()
        {
            var appStatic = _app.Pin<IApplicationStatic>();
            var window = appStatic.Current.MainWindow;
            Assert.AreEqual("Foo", window.UserNameBlock.Text);
        }

        public interface IIndexAccess : IAppVarOwner
        {
            int this[int index] { get; set; }
        }

        [TestMethod]
        public void インデックスアクセス()
        {
            IIndexAccess list = InterfaceHelper.Pin<IIndexAccess>(_app.Copy(new List<int>(new int[] { 0, 1, 2 })));
            list[1] = 100;
            Assert.AreEqual(100, list[1]);

            IIndexAccess array = InterfaceHelper.Pin<IIndexAccess>(_app.Copy(new int[] { 0, 1, 2 }));
            array[1] = 100;
            Assert.AreEqual(100, array[1]);
        }


        interface IPoint : IAppVarOwner
        {
            double X { get; set; }
            double Y { get; set; }
        }

        [ProxyTraget("System.Windows.Point")]
        interface IPointStatic
        {
            [NewAttribute]
            IPoint New();

        }

        [TestMethod]
        public void 生成()
        {
            IPoint pos = _app.Pin<IPointStatic>().New();
            pos.X = 100;
            Assert.AreEqual(100, pos.X);
        }
            
    }
}
