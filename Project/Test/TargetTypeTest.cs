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
    public class TargetTypeTest
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

        [TargetType("System.Windows.Point")]
        class Point_
        {
            internal interface Instance
            {
                double X { get; set; }
                double Y { get; set; }
            }
            internal interface Constructor
            {
                Instance New();
            }
        }

        [TargetType("System.Collections.Generic.Dictionary`2")]
        class Dictionary_<Key, Value>
        {
            internal interface Instance
            {
                Value this[Key key] { get; set; }
                void Add(Key key, Value value);
            }
            internal interface Constructor
            {
                Instance New();
            }
        }

        [TargetType("System.Collections.Generic.List`1")]
        class List_<T>
        {
            internal interface Instance
            {
                T this[int index] { get; set; }
                void Add(T value);
            }
            internal interface Constructor
            {
                Instance New();
            }
        }

        [TargetType("[]")]
        class Array1<T>
        {
            internal interface Instance
            {
                T this[int index] { get; set; }
            }
            internal interface Constructor
            {
                Instance New(int count);
            }
        }

        [TargetType("[,]")]
        class Array2<T>
        {
            internal interface Instance
            {
                T this[int index0, int index1] { get; set; }
            }
            internal interface Constructor
            {
                Instance New(int count0, int count1);
            }
        }

        [TargetType("[,,]")]
        class Array3<T>
        {
            internal interface Instance
            {
                T this[int index0, int index1, int index2] { get; set; }
            }
            internal interface Constructor
            {
                Instance New(int count0, int count1, int count2);
            }
        }

        [TestMethod]
        public void NameGeneric()
        {
            Assert.AreEqual(
                typeof(Dictionary<int, List<Point>>).FullName,
                TargetTypeUtility.GetFullName(_app, typeof(Dictionary_<int, List_<Point_.Instance>.Instance>.Instance)));
        }

        [TestMethod]
        public void NameArray()
        {
            Assert.AreEqual(
                typeof(int[]).FullName,
                TargetTypeUtility.GetFullName(_app, typeof(Array1<int>.Instance)));
        }

        [TestMethod]
        public void NameJugArray()
        {
            Assert.AreEqual(
                typeof(int[][]).FullName,
                TargetTypeUtility.GetFullName(_app, typeof(Array1<Array1<int>.Instance>.Instance)));
        }

        [TestMethod]
        public void NameArray2()
        {
            Assert.AreEqual(
                typeof(int[,]).FullName,
                TargetTypeUtility.GetFullName(_app, typeof(Array2<int>.Instance)));
        }

        [TestMethod]
        public void NameArray1_2()
        {
            Assert.AreEqual(
                typeof(int[][,]).FullName,
                TargetTypeUtility.GetFullName(_app, typeof(Array1<Array2<int>.Instance>.Instance)));
        }

        [TestMethod]
        public void NameArray1_2_3()
        {
            Assert.AreEqual(
                typeof(int[][,][, ,]).FullName,
                TargetTypeUtility.GetFullName(_app, typeof(Array1<Array2<Array3<int>.Instance>.Instance>.Instance)));
        }

        [TestMethod]
        public void GenericConstructor()
        {
            var dic = _app.PinConstructor<Dictionary_<int, Point_.Instance>.Constructor>().New();
            var pos = _app.PinConstructor<Point_.Constructor>().New();
            pos.X = 100;
            dic.Add(1, pos);
            Assert.AreEqual(100, dic[1].X);
        }
    }
}
