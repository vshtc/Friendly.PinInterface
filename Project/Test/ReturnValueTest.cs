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
using RM.Friendly.WPFStandardControls;

namespace Test
{
    [TestClass]
    public class ReturnValueTest
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

        interface IMainWindow : IInstance
        {
            WPFTextBox _textBox { get; }
        }

        [TestMethod]
        public void TestUserWrapper()
        {
            AppVar v = _app.Type<Application>().Current.MainWindow;
            var main = v.Pin<IMainWindow>();
            main._textBox.EmulateChangeText("abc");
            Assert.AreEqual("abc", main._textBox.Text);
        }
    }
}
