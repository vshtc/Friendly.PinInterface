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

```cs  
//Map same signature interface.
interface IWindow
{
  double Top { get; set; }
  string Title { get; set; }
  bool IsActive { get; }
  bool Topmost { get; set; }
  bool Activate();
}

void Demo()
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

```cs
/*operation target.
public class MainWindow : Window
{
	DataGrid _grid;
	Button _button;
}*/

// wrapper class that the constructor argument is AppVar only.
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
WPFDataGrid, WPFButton
https://github.com/Roommetro/Friendly.WPFStandardControls/blob/master/README.md

