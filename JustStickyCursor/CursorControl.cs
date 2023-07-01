using JustStickyCursor.GlobalLowLevelHooks;
using System.Runtime.InteropServices;
using System.Timers;
using ABI.Microsoft.UI.Xaml;

namespace JustStickyCursor;

/// <summary>
/// The actual important part of the program. <br/>
/// Continuously monitors the cursor...
/// TODO: finish this
/// </summary>
public class CursorControl
{
    // TODO delete this
    private static void DebugPrint(string str)
    {
        System.Diagnostics.Debug.WriteLine(str);
    }

    /// <summary>
    /// 
    /// </summary>
    public class StickyBorder
    {
        public int Coordinate;  // The x/y (depends on isVertical) coordinate of the border
        public bool IsVertical; // False = horizontal
        // TODO: bool for direction
        public int Buffer;      // "Holds" mouse movements towards the border
        public int Stickiness;  // Max value the buffer should reach before letting the cursor through

        public bool IsActive;

        public StickyBorder(int coordinate, bool isVertical = false, int buffer = 0, int stickiness = 10)
        {
            Coordinate = coordinate;
            IsVertical = isVertical;
            Buffer = buffer;
            Stickiness = stickiness;
        }
    }

    // TODO: idea is to load a List of StickyBorders and enforce each one
    private static List<StickyBorder> _stickyBorders = new();

    #region user32 function imports
    /// <summary>
    /// Retrieves the cursor's position, in screen coordinates.
    /// </summary>
    /// <see>See MSDN documentation for further information.</see>
    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out Point point);
    
    /// <summary>
    /// Sets the cursor's position, in screen coordinates.
    /// </summary>
    /// <see>See MSDN documentation for further information.</see>
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    /// <summary>
    /// SetCursorPos wrapper/arity that takes a Point
    /// </summary>
    private static bool SetCursorPos(Point point)
    {
        return SetCursorPos(point.X, point.Y);
    }

    /// <summary>
    /// Confine the cursor to a specified Rect.
    /// </summary>
    /// <see>See MSDN documentation for further information.</see>
    [DllImport("user32.dll")]
    private static extern bool ClipCursor(Rect rect);

    /// <summary>
    /// Get the current Rect the cursor is confined to.
    /// </summary>
    /// <see>See MSDN documentation for further information.</see>
    [DllImport("user32.dll")]
    private static extern bool GetClipCursor(out Rect rect);
    #endregion

    #region user32 structs (Point, Rect)
    /// <summary>
    /// The datatype returned by GetCursorPos, apparently
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    private readonly struct Point
    {
        public readonly int X;
        public readonly int Y;

        // Constructor
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// The datatype taken by ClipCursor. <br/>
    /// MSDN docs said they were "LONG"s, but long is a different size in 64- and 32-bit! Using ints instead...
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    private readonly struct Rect
    {
        public readonly int left;
        public readonly int top;
        public readonly int right;
        public readonly int bottom;

        // Constructor
        public Rect(int l, int t, int r, int b)
        {
            left = l;
            top = t;
            right = r;
            bottom = b;
        }
    }
    #endregion

    /// <summary>
    /// Cursor's current position
    /// </summary>
    private static Point _curPos;

    // Hardcoded items for testing/MVP
    private static readonly Rect AustinMonitor1Rect = new(0, 0, 2560, 1080);
    private static readonly Rect AustinMonitor2Rect = new(337, -1080, 2256, 0);

    // The sticky border on your PC is between y:0 and y:-1
    private static StickyBorder _austinStickyBorder = new(0, stickiness:200);

    // TODO helper function IsInRect

    private static void Init()
    {
        // TODO: Somehow refer to LocalSettingsService to grab settings from the UI?
        System.Diagnostics.Debug.WriteLine("CursorControl totally loaded settings, for real");

        // TODO: Properly load list of StickyBorders
        _stickyBorders.Add(_austinStickyBorder);
    }

    private static void SetTimers() => throw new NotImplementedException();

    //public static MouseHook MouseHook = new();
    //public static KeyboardHook KeyboardHook = new();

    private static void InstallHooks() => throw new NotImplementedException();

    public static async Task CursorControlLoop(CancellationToken cancelToken)
    {
        // TODO: Initialization stuff goes here
        Init();

        //SetTimers();

        //InstallHooks();

        // Get original rect
        //GetClipCursor(out var origRect);
        //System.Diagnostics.Debug.WriteLine($"origRect is {origRect.left}, {origRect.top}, {origRect.right}, {origRect.bottom}");

        //ClipCursor(_austinMonitor1Rect);
        //GetClipCursor(out var newRect);
        //System.Diagnostics.Debug.WriteLine($" new clipped Rect is {newRect.left}, {newRect.top}, {newRect.right}, {newRect.bottom}");

        // TODO idea: System.Timers timer that re-clips the cursor every so often

        // As long as the UI hasn't requested us to stop...
        while (!cancelToken.IsCancellationRequested)
        {
            //Thread.Sleep(5000);
            // Determine where cursor is
            GetCursorPos(out _curPos);
            // TODO: what rect is that in?

            // TODO: SUPER INEFFICIENT HACK! UNVIABLE FOR RELEASE.
            //ClipCursor(AustinMonitor1Rect);
            //System.Diagnostics.Debug.WriteLine($"Clipped to austinMonitor1Rect.\nX: {_curPos.X}\nY: {_curPos.Y}\n");

            // Enforce each of the stickyBorders
            //_stickyBorders.ForEach((border) =>
            foreach (var border in _stickyBorders)
            {
                // Probably good for performance?
                var x = _curPos.X;
                var y = _curPos.Y;

                // Border is active as long as buffer hasn't reached Stickiness
                DebugPrint(border.Buffer.ToString());
                border.IsActive = border.Buffer < border.Stickiness;

                // TODO: Too tired to logic it out... If inactive, don't set activity until cursor is back
                // (likely need to not rely only on buffer)

                // If inactive and on the "right side", reset buffer and go active
                if (!border.IsActive)
                {
                    border.Buffer = 0;
                    //border.IsActive = true;
                    continue;
                }

                if (border.IsVertical)
                {
                    // Cursor x something
                }
                else if (y < border.Coordinate) // Horizontal border
                {
                    // Cursor y something
                    border.Buffer -= y;
                    SetCursorPos(x, border.Coordinate);
                }
            }
            //);
        }
    }
}