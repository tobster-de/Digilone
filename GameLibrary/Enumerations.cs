using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    /// <summary>
    /// Color of a marble - also used for players
    /// </summary>
    public enum MarbleColor : int
    {
        Black = 0,
        White = 1
    }

    /// <summary>
    /// Direction of Movement
    /// </summary>
    /// <remarks>
    ///    1   2
    ///     \ /
    ///  6 - o - 3
    ///     / \
    ///    5   4
    /// </remarks>
    public enum Direction : int
    {
        None = 0, 
        Left = 6, 
        Right = 3, 
        UpLeft = 1, 
        UpRight = 2, 
        DownLeft = 5, 
        DownRight = 4,
        Clockwise = 10,
        Counterclockwise = 11
    }

    /// <summary>
    /// Type of move
    /// </summary>
    public enum MoveType
    {
        None, 
        Line, 
        Broad
    }

}
