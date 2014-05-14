Friendly.PinInterface
=====================

You must learn Friendly first.  
Because this library is built on Friendly Layer.  
But, it is very easy.  

http://www.english.codeer.co.jp/test-automation/friendly-fundamental  

=====================
Friendly use dynamic.  
Therefore, User can't use IntelliSense.  
PinInterface pins it by order interface.  
Since the user can use intellisense, He doesn't stray any longer.   

============================
Download form nuget.  
https://www.nuget.org/packages/RM.Friendly.WPFStandardControls/  

============================
* PinInterface API.
```cs
namespace VSHTC.Friendly.PinInterface
{
    //Helper for PinInterface.
    public static class PinHelper
    {
        public static TInterface Pin<TInterface>(AppFriend app);
        public static TInterface Pin<TInterface, TTarget>(AppFriend app);
        public static TInterface Pin<TInterface>(AppFriend app, string targetTypeFullName);
        public static TInterface Pin<TInterface>(AppFriend app, Type targetType);
        public static TInterface Pin<TInterface>(AppVar appVar);
        public static TInterface PinConstructor<TInterface>(AppFriend app);
        public static TInterface PinConstructor<TInterface, TTarget>(AppFriend app);
        public static TInterface PinConstructor<TInterface>(AppFriend app, string targetTypeFullName);
        public static TInterface PinConstructor<TInterface>(AppFriend app, Type targetType);
        public static Async AsyncNext(object pinnedInterface);
        public static AppVar GetAppVar(object pinnedInterface);
        public static void OperationTypeInfoNext(object pinnedInterface);
        public static void OperationTypeInfoNext(object pinnedInterface, OperationTypeInfo operationTypeInfo);
    }

    //Extension method of PinHelper.
    public static class PinHelperExtensions
    {
        public static TInterface Pin<TInterface, TTarget>(this AppFriend app);
        public static TInterface Pin<TInterface>(this AppFriend app);
        public static TInterface Pin<TInterface>(this AppFriend app, string targetTypeFullName);
        public static TInterface Pin<TInterface>(this AppFriend app, Type targetType);
        public static TInterface Pin<TInterface>(this AppVar appVar);
        public static TInterface PinConstructor<TInterface>(this AppFriend app);
        public static TInterface PinConstructor<TInterface, TTarget>(this AppFriend app);
        public static TInterface PinConstructor<TInterface>(this AppFriend app, string targetTypeFullName);
        public static TInterface PinConstructor<TInterface>(this AppFriend app, Type targetType);
    }

    //Specify type.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class TargetTypeAttribute : Attribute
    {
        public TargetTypeAttribute(string fullNmae);
        public string FullName { get; }
    }
}

```

============================
* Sample

Map same signature interface.
```cs
interface IApplicationStatic
{
    IApplication Current { get; }
}

interface IApplication
{
    IWindow MainWindow { get; set; }
}

interface IWindow
{
    double Top { get; set; }
    string Title { get; set; }
    bool IsActive { get; }
    bool Topmost { get; set; }
    bool Activate();
}

void DemoPinStaticInterface()
{
    var process = Process.GetProcessesByName("WPFTarget")[0];  
    using (var app = new WindowsAppFriend(process))  
    {  
        var application = app.Pin<IApplicationStatic>(typeof(Application));
        application.Current.MainWindow.Topmost = true;
    }
}

void DemoPinInstanceInterface()
{
    var process = Process.GetProcessesByName("WPFTarget")[0];  
    using (var app = new WindowsAppFriend(process))  
    {  
        AppVar src = app.Type(typeof(Application)).Current.MainWindow;
        var main = src.Pin<IWindow>();
        main.Topmost = true;
    }
}
```

Constrcutor
```cs
/*operation target.
namespace Target
{
    public class Data
    {
        public int Core { get; set; }
        public Data() { }
        public Data(int core)
        {
            Core = core;
        }
    }
}*/

interface IData
{
    int Core { get; set; }
}

interface IDataConstructor
{
    IData New();
    IData New(int core);
}

void DemoPinStaticInterface()
{
    var process = Process.GetProcessesByName("WPFTarget")[0];  
    using (var app = new WindowsAppFriend(process))  
    {  
        var constructor = app.PinConstructor<IDataConstructor>("Target.Data");
        var data = constructor.New(3);
    }
}
```

TargetTypeAttribute
```cs
[TargetType("System.Windows.Application")]
interface IApplicationStatic
{
    IApplication Current { get; }
}

[TargetType("Target.Data")]
interface IDataConstructor
{
    IData New();
    IData New(int core);
}

void DemoPinStaticInterface()
{
    var process = Process.GetProcessesByName("WPFTarget")[0];  
    using (var app = new WindowsAppFriend(process))  
    {  
        var application = app.Pin<IApplicationStatic>();
        var constructor = app.PinConstructor<IDataConstructor>();
        var data = constructor.New(3);
    }
}
```

Wrapper class that the constructor argument is AppVar only.
```cs
/*operation target.
public class MainWindow : Window
{
    DataGrid _grid;
    Button _button;
}*/

interface IMainWindow
{
    WPFDataGrid _grid { get; }
    WPFButtonBase _button { get; }
}

void Demo()
{
    var process = Process.GetProcessesByName("WPFTarget")[0];  
    using (var app = new WindowsAppFriend(process))  
    {  
        AppVar src = app.Type(typeof(Application)).Current.MainWindow;
        var main = src.Pin<IMainWindow>();
        main._grid.EmulateChangeCellText(0, 0, "abc");
        main._button.EmulateClick();
    }
}
```
(WPFDataGrid, WPFButton https://www.nuget.org/packages/RM.Friendly.WPFStandardControls/)  
    
Map AppVar.
```cs
interface IWindow
{
    AppVar OwnedWindows { get; }
}

void DemoPinInstanceInterface()
{
    var process = Process.GetProcessesByName("WPFTarget")[0];  
    using (var app = new WindowsAppFriend(process))  
    {  
        AppVar src = app.Type(typeof(Application)).Current.MainWindow;
        var main = src.Pin<IWindow>();
        AppVar owner = main.OwnedWindows;
    }
}
```

Other helper methods.
```cs
interface IWindow
{
    bool Activate();
}

void DemoPinInstanceInterface()
{
    var process = Process.GetProcessesByName("WPFTarget")[0];  
    using (var app = new WindowsAppFriend(process))  
    {  
        AppVar src = app.Type(typeof(Application)).Current.MainWindow;
        var main = src.Pin<IWindow>();
        
        //async
        PinHelper.AsyncNext(main);
        main.Activate();
        
        //OperationTypeInfo
        PinHelper.OperationTypeInfoNext(main);
        main.Activate();
        
        //Get core.
        src = PinHelper.GetAppVar(main);
    }
}
```
