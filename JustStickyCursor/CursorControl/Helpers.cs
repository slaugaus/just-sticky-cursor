namespace JustStickyCursor.CursorControl;

public class Helpers
{
    // TODO delete this
    public static void DebugPrint(string str)
    {
        System.Diagnostics.Debug.WriteLine(str);
    }

    /// <summary>
    /// TODO: Return whether a Point is in a Rect.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="rect"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static bool IsInRect(CursorControl.Point point, CursorControl.Rect rect) =>
        throw new NotImplementedException();

    /// <summary>
    /// Negate n if b is false.
    /// </summary>
    /// <param name="n">An int</param>
    /// <param name="b">A bool; if false, negates n</param>
    /// <returns></returns>
    public static int NegateMaybe(int n, bool b)
    {
        return (b ? n : -n);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cursorCoord"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool IsOnGoodSide(int cCoord, StickyBorder b)
    {
        var bCoord = b.Coordinate;

        // -hor: y < coord
        // +hor: y > coord
        // -ver: x < coord
        // +ver: x > coord

        if (b.Direction)
        {
            return cCoord > bCoord;
        }
        // else
        return cCoord < bCoord;
    }
}