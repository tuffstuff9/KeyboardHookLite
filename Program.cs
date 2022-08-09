using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace KeyboardHookLite;

internal class Program
{
    // Here is a demo of the library in action
    static void Main(string[] args)
    {
        KeyboardHook kb = new KeyboardHook();
        kb.KeyboardPressed += Kb_KeyboardPressed;
        System.Windows.Threading.Dispatcher.Run();
        kb.Dispose();
    }

    private static void Kb_KeyboardPressed(object? sender, KeyboardHookEventArgs e)
    {
        // Outputs the pressed type of key press.
        // WM_KEYDOWN, WM_SYSKEYDOWN, WM_KEYUP, WM_SYSKEYUP
        Console.WriteLine(e.KeyPressType);

        // Outputs the pressed Key (of type System.Windows.Input).
        Console.WriteLine(e.InputEvent.Key);

        // Outputs the virtual key code.
        Console.WriteLine(e.InputEvent.VirtualCode);

        // Outputs a hardware scan code for the key. 
        Console.WriteLine(e.InputEvent.HardwareScanCode);

        // Outputs the extended-key flag, event-injected Flags,
        // context code, and transition-state flag.
        Console.WriteLine(e.InputEvent.Flags);

        // Outputs the time stamp stamp for this message, equivalent
        // to what GetMessageTime would return for this message.
        Console.WriteLine(e.InputEvent.TimeStamp);

        // Outputs additional information associated with the message. 
        Console.WriteLine(e.InputEvent.AdditionalInformation);
    }
}