using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace KeyboardHookLite;

internal class Program
{

    static void Main(string[] args)
    {
        KeyboardHook kb = new KeyboardHook();
        kb.KeyboardPressed += Kb_KeyboardPressed;
        System.Windows.Threading.Dispatcher.Run();
        kb.Dispose();
    }

    private static void Kb_KeyboardPressed(object? sender, KeyboardHookEventArgs e)
    {
        Console.WriteLine(e.InputEvent.Key);
    }
}