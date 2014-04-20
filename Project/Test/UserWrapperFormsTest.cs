using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using System.Diagnostics;
using VSHTC.Friendly.PinInterface;
using VSHTC.Friendly.PinInterface.Inside;
using System.Windows.Documents;
using System.Collections.Generic;
using Ong.Friendly.FormsStandardControls;

namespace Test
{
    [TestClass]
    public class UserWrapperFormsTest
    {
        WindowsAppFriend _app;

        [TestInitialize]
        public void TestInitialize()
        {
            _app = new WindowsAppFriend(Process.Start("TargetForms.exe"));
            WindowsAppExpander.LoadAssembly(_app, GetType().Assembly);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Process.GetProcessById(_app.ProcessId).CloseMainWindow();
        }

        interface IMainWindow
        {
            FormsTextBox _textBox { get; }
        }

        [TestMethod]
        public void TestUserWrapper()
        {
            AppVar v = _app.Type().System.Windows.Forms.Application.OpenForms[0];
            var main = v.Pin<IMainWindow>();
            main._textBox.EmulateChangeText("abc");
            Assert.AreEqual("abc", main._textBox.Text);
        }
    }
}
