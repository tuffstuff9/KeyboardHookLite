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

    public KeyPressType KeyPressType
    {
        get; private set;
    }

    public bool SuppressKeyPress
    {
        get; set;
    }

    public KeyboardHookEventArgs(LowLevelKeyboardInputEvent inputEvent, KeyPressType keyPressType)
    {
        InputEvent = inputEvent;
        KeyPressType = keyPressType;
    }
}
