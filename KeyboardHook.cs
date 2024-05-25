using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Diagnostics;
using System.ComponentModel;

namespace KeyboardHookLite;

public class KeyboardHook : IDisposable
{
    public event EventHandler<KeyboardHookEventArgs> KeyboardPressed;

    // We require this to sepcify the type of hook we are setting.
    private const int WH_KEYBOARD_LL = 13;

    // This enum will be part of the KeyboardHookEventArgs, so that
    // we know what type of key press it was.
    public enum KeyPressType
    {
        KeyDown = 0x0100,
        KeyUp = 0x0101,
        SysKeyDown = 0x0104,
        SysKeyUp = 0x0105
    }

    private LowLevelKeyboardProc _proc;
    private IntPtr _hookID = IntPtr.Zero;
    private IntPtr _user32LibraryHandle = IntPtr.Zero;


    // Constructor to set the hook
    public KeyboardHook()
    {
        _proc = HookCallback;
        _user32LibraryHandle = LoadLibrary("User32");

        _hookID =  SetWindowsHookEx(WH_KEYBOARD_LL, _proc, _user32LibraryHandle, 0);
    }


    // We have a struct to read the KBDLLHOOKSTRUCT
    // (refer to http://pinvoke.net/default.aspx/Structures/KBDLLHOOKSTRUCT.html)
    // for more information
    public struct LowLevelKeyboardInputEvent
    {
        /// <summary>
        /// A virtual-key code. The code must be a value in the range 1 to 254.
        /// </summary>
        public int VirtualCode;

        /// <summary>
        /// The VirtualCode converted to typeof(Keys) for higher usability.
        /// </summary>
        public Key Key
        {
            get
            {
                return KeyInterop.KeyFromVirtualKey(VirtualCode);
            }
        }

        /// <summary>
        /// A hardware scan code for the key. 
        /// </summary>
        public int HardwareScanCode;

        /// <summary>
        /// The extended-key flag, event-injected Flags, context code, and transition-state flag. This member is specified as follows. An application can use the following values to test the keystroke Flags. Testing LLKHF_INJECTED (bit 4) will tell you whether the event was injected. If it was, then testing LLKHF_LOWER_IL_INJECTED (bit 1) will tell you whether or not the event was injected from a process running at lower integrity level.
        /// </summary>
        public int Flags;

        /// <summary>
        /// The time stamp stamp for this message, equivalent to what GetMessageTime would return for this message.
        /// </summary>
        public int TimeStamp;

        /// <summary>
        /// Additional information associated with the message. 
        /// </summary>
        public IntPtr AdditionalInformation;
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    public IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        // If nCode is less than 0, it means the callback function must return the value returned by CallNextHookEx.
        // https://learn.microsoft.com/en-us/previous-versions/windows/desktop/legacy/ms644985(v=vs.85)#return-value
        if (nCode < 0)
        {
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        // We must identify the type of keypress this was, so we can pass it into
        // the event arguments.
        var wParamType = wParam.ToInt32();


        // We need to ensure the message is of a keypress type (from KeyPressType enum).
        if (Enum.IsDefined(typeof(KeyPressType), wParamType))
        {
            // We need to convert the LowLevelKeyboardInputEvent (KBDLLHOOKSTRUCT) into a
            // structure that we can access.
            // You could also do int vkCode = Marshal.ReadInt32(lParam); for just the 
            // virtual key code.
            object o = Marshal.PtrToStructure(lParam, typeof(LowLevelKeyboardInputEvent));

            // We cast the object into the desired type (which is LowLevelKeyboardInputEvent).
            LowLevelKeyboardInputEvent e = (LowLevelKeyboardInputEvent)o;

            // We declare the event arguments
            KeyboardHookEventArgs eventArgs = new KeyboardHookEventArgs(e, (KeyPressType)wParamType);

            EventHandler<KeyboardHookEventArgs> handler = KeyboardPressed;
            handler?.Invoke(this, eventArgs);

            // Printing out the wParam to see what type of message we are receiving
            // Console.WriteLine(wParam);
        }

        // We call the next hook so the next hook in line can receive the message
        return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }

    // Below, we are disposing the unmanaged code to prevent memory leaks.
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_hookID != IntPtr.Zero)
            {
                if (!UnhookWindowsHookEx(_hookID))
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode, $"Failed to remove the hook. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}");
                }
                _hookID = IntPtr.Zero;
                _proc -= HookCallback;
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~KeyboardHook()
    {
        Dispose(false);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
    static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);
}
