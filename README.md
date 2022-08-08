# <img src="https://user-images.githubusercontent.com/57072903/183295348-10681b7e-7a29-4018-9276-589a039d5ace.png#gh-light-mode-only" class="center"> <img src="https://user-images.githubusercontent.com/57072903/183296638-86803419-9891-4bb0-be61-101605e318a6.png#gh-dark-mode-only" class="center">

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

`KeyboardHookEventArgs` contains the property `InputEvent` which is of type `LowLevelKeyboardInputEvent`. See below for what that entails. If you would like to add further functionality, you can do so by adding properties to `KeyboardHookEventArgs`.
<img src="https://user-images.githubusercontent.com/57072903/183413214-6d8e1f0d-5f60-47c7-b091-fd5e2bffc879.png" class="center" width="800px">

## License

MIT



