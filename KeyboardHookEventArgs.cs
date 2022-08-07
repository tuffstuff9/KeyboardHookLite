using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KeyboardHookLite.KeyboardHook;

namespace KeyboardHookLite;
public class KeyboardHookEventArgs : EventArgs
{
    public LowLevelKeyboardInputEvent InputEvent
    {
        get; private set;
    }

    public KeyboardHookEventArgs(LowLevelKeyboardInputEvent inputEvent)
    {
        InputEvent = inputEvent;
    }
}
