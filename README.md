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
* Map same signature interface.
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

TargetTypeAttribute
```cs

[TargetType("System.Windows.Application")]
interface IApplicationStatic
{
  IApplication Current { get; }
}

void DemoPinStaticInterface()
{
  var process = Process.GetProcessesByName("WPFTarget")[0];  
  using (var app = new WindowsAppFriend(process))  
  {  
      var application = app.Pin<IApplicationStatic>();
      application.Current.MainWindow.Topmost = true;
  }
}
```

============================
* wrapper class that the constructor argument is AppVar only.
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
============================
Download form nuget.  
https://www.nuget.org/packages/RM.Friendly.WPFStandardControls/  

============================
WPFDataGrid, WPFButton  
https://www.nuget.org/packages/RM.Friendly.WPFStandardControls/

