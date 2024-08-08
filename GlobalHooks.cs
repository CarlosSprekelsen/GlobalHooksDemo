using System.Diagnostics;
using System.Runtime.InteropServices;

public static class GlobalHooks
{
    // Define the delegates for mouse and keyboard procedures
    private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr _mouseHookID = IntPtr.Zero;
    private static IntPtr _keyboardHookID = IntPtr.Zero;
    private static LowLevelMouseProc _mouseProc;
    private static LowLevelKeyboardProc _keyboardProc;

    // Delegate to send event data to MainForm
    public delegate void EventAction(int eventType, IntPtr wParam, IntPtr lParam);
    private static EventAction eventAction;

    public static void SetHooks()
    {
        _mouseProc = MouseHookCallback;
        _keyboardProc = KeyboardHookCallback;
        _mouseHookID = SetHook(WH_MOUSE_LL, _mouseProc);
        _keyboardHookID = SetHook(WH_KEYBOARD_LL, _keyboardProc);
    }

    public static void ReleaseHooks()
    {
        if (_mouseHookID != IntPtr.Zero)
        {
            UnhookWindowsHookEx(_mouseHookID);
            _mouseHookID = IntPtr.Zero;
        }
        if (_keyboardHookID != IntPtr.Zero)
        {
            UnhookWindowsHookEx(_keyboardHookID);
            _keyboardHookID = IntPtr.Zero;
        }
    }

    public static void RegisterEventAction(EventAction action)
    {
        eventAction = action;
    }

    private static IntPtr SetHook(int idHook, Delegate proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(idHook, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && eventAction != null)
        {
            eventAction(0, wParam, lParam);  // 0 for mouse events
        }
        return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);
    }

    private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && eventAction != null)
        {
            eventAction(1, wParam, lParam);  // 1 for keyboard events
        }
        return CallNextHookEx(_keyboardHookID, nCode, wParam, lParam);
    }

    // Include necessary PInvoke methods
    private const int WH_MOUSE_LL = 14;
    private const int WH_KEYBOARD_LL = 13;

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, Delegate lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
}
