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
using Codeer.Friendly.Windows.Grasp;
using Codeer.Friendly.Windows.NativeStandardControls;

namespace Test
{
    [TestClass]
    public class TesIModifyInvoke
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

        [Serializable]
        class Data
        {
            public int A { get; set; }
            public string B { get; set; }
        }

        class Target
        {
            public Data Create(int a, string b)
            {
                return new Data() { A = a, B = b };
            }
            public int GetA(Data data)
            {
                return data.A;
            }
            public string GetB(Data data)
            {
                return data.B;
            }
        }

        interface IMessageBox : IStatic
        {
            MessageBoxResult Show(string messageBoxText);
        }

        [TestMethod]
        public void 非同期実行()
        {
            IMessageBox msg = _app.Pin<IMessageBox, MessageBox>();
            WindowControl top = WindowControl.FromZTop(_app);
            Async async = msg.AsyncNext();
            msg.Show("");
            WindowControl next = top.WaitForNextModal();
            new NativeMessageBox(next).EmulateButtonClick("OK");
            async.WaitForCompletion();
        }       
    }
}
