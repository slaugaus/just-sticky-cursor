using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace JustStickyCursor.GlobalLowLevelHooks;

/// <summary>
/// Class for intercepting low level Windows mouse hooks. <br/>
/// Taken from https://github.com/riyasy/Global-Low-Level-Key-Board-And-Mouse-Hook
/// </summary>
public class MouseHook
{
    /// <summary>
    /// Internal callback processing function
    /// </summary>
    private delegate nint MouseHookHandler(int nCode, nint wParam, nint lParam);
    private MouseHookHandler? _hookHandler;

    /// <summary>
    /// Function to be called when defined even occurs
    /// </summary>
    /// <param name="mouseStruct">MSLLHOOKSTRUCT mouse structure</param>
    public delegate void MouseHookCallback(Msllhookstruct mouseStruct);

    #region Events
    public event MouseHookCallback? LeftButtonDown;
    public event MouseHookCallback? LeftButtonUp;
    public event MouseHookCallback? RightButtonDown;
    public event MouseHookCallback? RightButtonUp;
    public event MouseHookCallback? MouseMove;
    public event MouseHookCallback? MouseWheel;
    public event MouseHookCallback? DoubleClick;
    public event MouseHookCallback? MiddleButtonDown;
    public event MouseHookCallback? MiddleButtonUp;
    #endregion

    /// <summary>
    /// Low level mouse hook's ID
    /// </summary>
    private nint _hookId = nint.Zero;

    /// <summary>
    /// Install low level mouse hook
    /// </summary>
    /// <param name="mouseHookCallbackFunc">Callback function</param>
    public void Install()
    {
        _hookHandler = HookFunc;
        _hookId = SetHook(_hookHandler);
    }

    /// <summary>
    /// Remove low level mouse hook
    /// </summary>
    public void Uninstall()
    {
        if (_hookId == nint.Zero)
        {
            return;
        }

        UnhookWindowsHookEx(_hookId);
        _hookId = nint.Zero;
    }

    /// <summary>
    /// Destructor. Unhook current hook
    /// </summary>
    ~MouseHook()
    {
        Uninstall();
    }

    /// <summary>
    /// Sets hook and assigns its ID for tracking
    /// </summary>
    /// <param name="proc">Internal callback function</param>
    /// <returns>Hook ID</returns>
    private nint SetHook(MouseHookHandler proc)
    {
        using var module = Process.GetCurrentProcess().MainModule;
        return SetWindowsHookEx(WhMouseLl, proc, GetModuleHandle(module?.ModuleName), 0);
    }

    /// <summary>
    /// Callback function
    /// </summary>
    private nint HookFunc(int nCode, nint wParam, nint lParam)
    {
        // parse system messages
        if (nCode < 0)
        {
            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        if (MouseMessages.WmLbuttondown == (MouseMessages)wParam)
        {
            LeftButtonDown?.Invoke((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
        }

        if (MouseMessages.WmLbuttonup == (MouseMessages)wParam)
        {
            LeftButtonUp?.Invoke((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
        }

        if (MouseMessages.WmRbuttondown == (MouseMessages)wParam)
        {
            RightButtonDown?.Invoke((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
        }

        if (MouseMessages.WmRbuttonup == (MouseMessages)wParam)
        {
            RightButtonUp?.Invoke((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
        }

        if (MouseMessages.WmMousemove == (MouseMessages)wParam)
        {
            MouseMove?.Invoke((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
        }

        if (MouseMessages.WmMousewheel == (MouseMessages)wParam)
        {
            MouseWheel?.Invoke((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
        }

        if (MouseMessages.WmLbuttondblclk == (MouseMessages)wParam)
        {
            DoubleClick?.Invoke((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
        }

        if (MouseMessages.WmMbuttondown == (MouseMessages)wParam)
        {
            MiddleButtonDown?.Invoke((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
        }

        if (MouseMessages.WmMbuttonup == (MouseMessages)wParam)
        {
            MiddleButtonUp?.Invoke((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
        }
        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    #region WinAPI
    private const int WhMouseLl = 14;

    private enum MouseMessages
    {
        WmLbuttondown = 0x0201,
        WmLbuttonup = 0x0202,
        WmMousemove = 0x0200,
        WmMousewheel = 0x020A,
        WmRbuttondown = 0x0204,
        WmRbuttonup = 0x0205,
        WmLbuttondblclk = 0x0203,
        WmMbuttondown = 0x0207,
        WmMbuttonup = 0x0208
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Msllhookstruct
    {
        public Point pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public nint dwExtraInfo;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern nint SetWindowsHookEx(int idHook,
        MouseHookHandler lpfn, nint hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(nint hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern nint CallNextHookEx(nint hhk, int nCode, nint wParam, nint lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern nint GetModuleHandle(string lpModuleName);
    #endregion
}
