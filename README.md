<img src="https://user-images.githubusercontent.com/57072903/183416319-9f35f51c-d322-417f-a5c8-f5288a0a9c9e.svg#gh-light-mode-only" width="600px"> 
<img src="https://user-images.githubusercontent.com/57072903/183415708-a49e3da4-e0ef-4792-9999-cca6613283c3.svg#gh-dark-mode-only" width="600px">

<p>
    <a href="https://www.nuget.org/packages/KeyboardHookLite">
        <img src="https://img.shields.io/nuget/v/KeyboardHookLite?style=for-the-badge" alt="NuGet Version">
    </a>
 <a href="https://www.nuget.org/packages/KeyboardHookLite">
  <img src="https://img.shields.io/nuget/dt/KeyboardHookLite?style=for-the-badge" alt="Target Framework">
 </a>
 <a href="https://github.com/tuffstuff9/KeyboardHookLite/blob/a19121592231c060c247823e6bd8701751706596/KeyboardHookLite.csproj#L5">
        <img src="https://img.shields.io/badge/target-net6.0-blue?style=for-the-badge&color=%23512bd4" alt="Target Framework">
    </a>
 
 <a href="https://github.com/tuffstuff9/KeyboardHookLite/blob/master/LICENSE.txt">
        <img src="https://img.shields.io/github/license/tuffstuff9/KeyboardHookLite?style=for-the-badge" alt="Target Framework">
    </a>
    
</p>

<p>
 
 

</p>



## 🎯 Features


 - Lightweight low-level global keyboard hook with minimal dependencies.
 - Designed to work with modern UI frameworks (WPF, MAUI, WinUI 3) out of the box - no fiddling with outdated namespaces and Key types.
 - Uses PInvoke signatures in order to avoid reliance on [CsWin32](https://github.com/microsoft/CsWin32) source generator.
 - Source code is commented and thoroughly explained to allow you to expand functionality.
 - Proper garbage disposal of unmanaged code to prevent memory leaks.
 - Usage guide below allows you to get up and running quickly.
<img src="https://user-images.githubusercontent.com/57072903/183410107-55f881de-21a3-4a86-8c90-13bcc297f09c.gif" class="center" width="800px">

## ⚡️  Usage Guide
### Bring in the namespace
<img src="https://user-images.githubusercontent.com/57072903/183394211-c98e2ede-cd0c-488f-80cd-6680c9a40848.png" class="center" width="400px">

### Console Application
<img src="https://user-images.githubusercontent.com/57072903/183395715-a8938368-d96b-4d7a-a966-dc0adb8fd2fa.png" class="center" width="800px">

### WPF / WinUI 3 / Other

It is essentially the same as the code for the console application above. You can omit `System.Windows.Threading.Dispatcher.Run();` as UI frameworks already have a message queue. You can initialize the keyboard hook wherever you choose, whether it be upon a click or when the screen is initialized. Please remember to dispose (`kbh.Dispose();`) when you no longer require the hook. 

### KeyboardHookEventArgs features

`KeyboardHookEventArgs` contains the property `KeyPressType` and `InputEvent` which is of type `LowLevelKeyboardInputEvent`. See below for what that entails. If you would like to add further functionality, you can do so by adding properties to the `KeyboardHookEventArgs` class.
<img src="https://user-images.githubusercontent.com/57072903/183647600-5ab9fc21-5783-44fc-8649-2a413b43658e.png" class="center" width="800px">

## License

MIT




