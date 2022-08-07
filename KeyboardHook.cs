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

    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;

    private LowLevelKeyboardProc _proc;
    private IntPtr _hookID = IntPtr.Zero;
    private IntPtr _user32LibraryHandle = IntPtr.Zero;


    public KeyboardHook()
    {
        _proc = HookCallback;
        _user32LibraryHandle = LoadLibrary("User32");

        _hookID =  SetWindowsHookEx(WH_KEYBOARD_LL, _proc, _user32LibraryHandle, 0);
    }


    // We have a struct to read the KBDLLHOOKSTRUCT  
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
        // We need to convert the KBDLLHOOKSTRUCT into a structure that we can access
        // You could also do int vkCode = Marshal.ReadInt32(lParam); for just the 
        // virtual key code.
        object o = Marshal.PtrToStructure(lParam, typeof(LowLevelKeyboardInputEvent));

        // We cast the object into the desired type (which is LowLevelKeyboardInputEvent)
        LowLevelKeyboardInputEvent e = (LowLevelKeyboardInputEvent)o;

        KeyboardHookEventArgs eventArgs = new KeyboardHookEventArgs(e);


        if (nCode >= 0)
        {
            // We need to check for keydown and syskeydown because different keys
            // will trigger either one.
            if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                EventHandler<KeyboardHookEventArgs> handler = KeyboardPressed;
                handler?.Invoke(this, eventArgs);
                // Printing out the wParam to see what type of message we are receiving
                // Console.WriteLine(wParam);

                // Printing out the actual keypress
                // Console.WriteLine(e.Key);
            }

        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

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
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
    static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);
}
