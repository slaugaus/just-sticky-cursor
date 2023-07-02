namespace JustStickyCursor.CursorControl;

/// <summary>
/// 
/// </summary>
public class StickyBorder
{
    public short Coordinate;// The x/y (depends on isVertical) coordinate of the border
    public bool IsVertical; // False = horizontal
    public bool Direction;  // Which end of the line does the border affect? (True for pos, False for neg)
    public int Buffer;      // "Holds" mouse movements towards the border
    // TODO: instead of building a buffer, DMT might prevent movement unless the mouse is fast enough?
    public int Stickiness;  // Max value the buffer should reach before letting the cursor through

    public bool IsActive;

    public StickyBorder(short coordinate, bool direction, bool isVertical = false, int buffer = 0, int stickiness = 10)
    {
        Coordinate = coordinate;
        Direction = direction;
        IsVertical = isVertical;
        Buffer = buffer;
        Stickiness = stickiness;
    }
}