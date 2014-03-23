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
    public class RefParameterTest
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

        [Serializable]
        class Data
        {
            public int A { get; set; }
            public string B { get; set; }
        }

        class Target
        {
            public void Create(int a, string b, ref Data data)
            {
                data = new Data() { A = a, B = b };
            }
            public void GetRef(Data data, ref int a, ref string b)
            {
                a = data.A;
                b = data.B;
            }
            public void ChangeX(ref Point location)
            {
                location.X = 10;
            }
        }


        interface IData : IAppVarOwner
        {
            int A { get; set; }
            string B { get; set; }
        }

        interface IPoint : IAppVarOwner
        {
            double X { get; set; }
            double Y { get; set; }
        }

        interface ITarget : IAppVarOwner
        {
            void Create(int a, string b, ref Data data);
            void Create(int a, string b, ref IData data);
            void GetRef(Data data, ref int a, ref string b);
            void GetRef(Data data, ref AppVar a, ref AppVar b);
            void ChangeX(ref Point location);
            void ChangeX(ref AppVar location);
            void ChangeX(ref IPoint location);
        }

        [TestMethod]
        public void Refシリアライズ()
        {
            AppVar v = _app.Type<Target>()();
            ITarget target = v.Pin<ITarget>();
            Data data = null;
            target.Create(5, "X", ref data);
            Assert.AreEqual(5, data.A);
            Assert.AreEqual("X", data.B);
            int a = 0;
            string b = null;
            target.GetRef(data, ref a, ref b);
            Assert.AreEqual(5, a);
            Assert.AreEqual("X", b);
            Point pos = new Point() { X = 1, Y = 2 };

            //構造体
            target.ChangeX(ref pos);
            Assert.AreEqual(10, pos.X);
            Assert.AreEqual(2, pos.Y);
        }

        [TestMethod]
        public void RefAppVar()
        {
            AppVar v = _app.Type<Target>()();
            ITarget target = v.Pin<ITarget>();
            Data data = null;
            target.Create(5, "X", ref data);

            {
                AppVar a = null;
                AppVar b = null;
                target.GetRef(data, ref a, ref b);
                Assert.AreEqual(5, (int)a.Core);
                Assert.AreEqual("X", (string)b.Core);
            }

            //もともと入っている場合
            {
                AppVar a = _app.Copy(100);
                AppVar b = _app.Copy("abc");
                target.GetRef(data, ref a, ref b);
                Assert.AreEqual(5, (int)a.Core);
                Assert.AreEqual("X", (string)b.Core);
            }

            //構造体
            AppVar pos = _app.Copy(new Point() { X = 1, Y = 2 });
            target.ChangeX(ref pos);
            Assert.AreEqual(10, (double)pos.Dynamic().X);
            Assert.AreEqual(2, (double)pos.Dynamic().Y);
        }

        [TestMethod]
        public void RefReference()
        {
            AppVar v = _app.Type<Target>()();
            ITarget target = v.Pin<ITarget>();
            IData data = null;
            target.Create(5, "X", ref data);
            Assert.AreEqual(5, data.A);
            Assert.AreEqual("X", data.B);

            //バッファにデータが入っている場合
            target.Create(6, "Y", ref data);
            Assert.AreEqual(6, data.A);
            Assert.AreEqual("Y", data.B);

            //構造体
            IPoint pos = InterfaceHelper.Pin<IPoint>(_app.Copy(new Point() { X = 1, Y = 2 }));
            target.ChangeX(ref pos);
            Assert.AreEqual(10, pos.X);
            Assert.AreEqual(2, pos.Y);
        }
    }
}
