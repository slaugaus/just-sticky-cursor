using JustStickyCursor.GlobalLowLevelHooks;
using System.Runtime.InteropServices;
using System.Timers;
using ABI.Microsoft.UI.Xaml;

namespace JustStickyCursor.CursorControl;

/// <summary>
/// The actual important part of the program. <br/>
/// Continuously monitors the cursor...
/// TODO: finish this
/// </summary>
public class CursorControl
{
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
    public readonly struct Point
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
    public readonly struct Rect
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

    // The sticky border you want is between y:0 and y:-1... but here are some that can be shown on 1 screen
    private static StickyBorder _demoBorderYBottom = new(540, true, stickiness: 500);
    private static StickyBorder _demoBorderXLeft = new(960, false, isVertical: true, stickiness: 500);

    private static void Init()
    {
        // TODO: Somehow refer to LocalSettingsService to grab settings from the UI?
        System.Diagnostics.Debug.WriteLine("CursorControl totally loaded settings, for real");

        // TODO: Properly load list of StickyBorders
        _stickyBorders.Add(_demoBorderYBottom);
        _stickyBorders.Add(_demoBorderXLeft);
    }

    //public static MouseHook MouseHook = new();
    //public static KeyboardHook KeyboardHook = new();

    private static void EnforceBorder(StickyBorder border)
    {
        // Probably good for performance?
        var x = _curPos.X;
        var y = _curPos.Y;

        // The cursor coordinate that matters
        var cCoord = (border.IsVertical ? x : y);

        var bCoord = border.Coordinate;
        var stickiness = border.Stickiness;

        // Border is active as long as buffer hasn't reached Stickiness
        border.IsActive = border.Buffer < stickiness;
        Helpers.DebugPrint(border.Buffer.ToString());

        // If inactive and on the "right side", reset buffer (which will go active)
        if (border.Buffer >= stickiness && Helpers.IsOnGoodSide(cCoord, border))
        {
            border.Buffer = 0;
            return;
        }

        // TODO: account for direction in this mess
        if (border.IsActive)
        {
            if (border.IsVertical)
            {
                if (x > bCoord)
                {
                    // Buffer however many pixels the cursor went over by
                    border.Buffer += x - bCoord;
                    //border.Buffer++;  // ignores cursor speed - not sure which one I like better
                    SetCursorPos(bCoord, y);
                }
            }
            else // horizontal
            {
                if (y < bCoord) // Crossed
                {
                    // Buffer however many pixels the cursor went over by
                    border.Buffer += bCoord - y;
                    //border.Buffer++;  // ignores cursor speed - not sure which one I like better
                    SetCursorPos(x, bCoord);
                }
                // If cursor has moved away from the border, reset it
                else if (y > bCoord)
                {
                    border.Buffer = 0;
                }
            }
        }
    }

    public static async Task CursorControlLoop(CancellationToken cancelToken)
    {
        // TODO: Initialization stuff goes here
        Init();

        // Get original rect
        //GetClipCursor(out var origRect);
        //System.Diagnostics.Debug.WriteLine($"origRect is {origRect.left}, {origRect.top}, {origRect.right}, {origRect.bottom}");

        //ClipCursor(_austinMonitor1Rect);
        //GetClipCursor(out var newRect);
        //System.Diagnostics.Debug.WriteLine($" new clipped Rect is {newRect.left}, {newRect.top}, {newRect.right}, {newRect.bottom}");

        // TODO idea: System.Timers timer that re-clips the cursor every so often?

        // As long as the UI hasn't requested us to stop...
        while (!cancelToken.IsCancellationRequested)
        {
            //Thread.Sleep(5000);
            // Determine where cursor is
            GetCursorPos(out _curPos);
            // TODO: what rect is that in?

            // TODO: SUPER INEFFICIENT HACK (if in the while loop)! UNVIABLE FOR RELEASE.
            //ClipCursor(AustinMonitor1Rect);
            //System.Diagnostics.Debug.WriteLine($"Clipped to austinMonitor1Rect.\nX: {_curPos.X}\nY: {_curPos.Y}\n");

            // Enforce each of the stickyBorders
            _stickyBorders.ForEach(EnforceBorder);
        }
    }
}