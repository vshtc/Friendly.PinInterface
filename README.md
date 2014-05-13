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

void Test
{
  var process = Process.GetProcessesByName("WPFTarget")[0];  
  using (var app = new WindowsAppFriend(process))  
  {  
      AppVar src = app.Type(typeof(Application)).Current.MainWindow;
      IWindow main = src.Pin<IWindow>();
      main.Topmost = true;
  }
}
```
