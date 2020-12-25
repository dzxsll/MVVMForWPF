# MVVMForWPF
This MVVM is coded for WPF.

There are three part of it:  
1.the MVVM for WPF including INotifyPorpertyChange & ICommand & InvokeCommand  
2.Message between View&ViewModel Designed by `durow` in [博客园](https://www.cnblogs.com)  
3.IoC for View&VieModel

Here are blogs which show how to code it:  
https://www.cnblogs.com/durow/p/4853729.html

I made code some different:  
1.`NotifyObject.cs` is same as `ObserveObject.cs` for notifing the data changed but in `SetAndNotifyIfChanged()` will have exepection for using `null object`.The reason is `oldValue` is null when the function calls `oldValue.Equals()`.  
2.Use [CallerMemberName] and don't have to set propertyName on purpose , It will know. ：）。

So. That is it.   
If the code can help you, I will be happy.
