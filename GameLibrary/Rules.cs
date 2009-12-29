using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    /// <summary>
    /// Class encapsulating the rules of the game
    /// </summary>
    public static class Rules
    {
        /// <summary>
        /// Checks if a marble has left a valid position
        /// </summary>
        /// <param name="coordinates">Coordinates of a marble</param>
        /// <returns>Value that states if the marble was thrown out</returns>
        /// <remarks>See <see cref="BoardCoordinates"/> for information about valid positions</remarks>
        public static bool MarbleIsOut(ICoordinates coordinates)
        {
            return coordinates is OuterRingCoordinates;
        }

        /// <summary>
        /// Determines if a turn of a player may be valid
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static bool MoveIsValid(Board board, List<Marble> selected, Direction direction)
        {
            if (direction == Direction.None)
            {
                return false;
            }
            MoveType mt = DetermineMoveType(selected, direction);
            if (mt == MoveType.Line)
            {
                MarbleColor currentPlayer = selected[0].Color;
                List<MarbleColor> moveline = new List<MarbleColor>();
                foreach (Marble m in selected)
                {
                    moveline.Add(m.Color);
                }
                Marble next = selected[selected.Count - 1];
                while (next != null)
                {
                    next = board.GetMarble(NextCoordinates(next.Coordinates, direction), true);
                    if (next != null) moveline.Add(next.Color);
                }
                int i = 0, mcol = 0, hcol = 0, ncol = 0;
                while (i < moveline.Count && moveline[i] == currentPlayer)
                {
                    mcol++;
                    i++;
                }
                while (i < moveline.Count && moveline[i] != currentPlayer)
                {
                    hcol++;
                    i++;
                }
                while (i < moveline.Count && moveline[i] == currentPlayer)
                {
                    ncol++;
                    i++;
                }
                // move max. three own marbles; push fewer amount of enemy marbles than own
                // and have no other own marbles behind them
                return (mcol <= 3 && mcol > hcol && ncol == 0);
            }
            else if (mt == MoveType.Broad)
            {
                // move max. three marbles; all destination slots must be free
                bool free = selected.Count < 4;
                if (free) foreach (Marble m in selected)
                    {
                        ICoordinates dest = NextCoordinates(m.Coordinates, direction);
                        free = free && board.GetMarble(dest, true) == null;
                    }
                return free;
            }
            return false;
        }

        /// <summary>
        /// Determines the type of the movement depending on the move direction and the direction of the selection
        /// </summary>
        /// <param name="selected">selected marbles</param>
        /// <param name="direction">movement direction</param>
        /// <returns>type of turn movement</returns>
        public static MoveType DetermineMoveType(List<Marble> selected, Direction direction)
        {
            if (selected.Count == 1)
            {
                return MoveType.Line;
            }
            bool line = true;
            for (int i = 0; i < selected.Count - 1; i++)
            {
                ICoordinates next = NextCoordinates(selected[i].Coordinates, direction);
                line = line && next.Equals(selected[i + 1].Coordinates);
            }
            if (line)
            {
                return MoveType.Line;
            }
            return MoveType.Broad;
        }

        /// <summary>
        /// Determines the coordinates in a desired direction
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static ICoordinates NextCoordinates(ICoordinates coordinates, Direction direction)
        {
            if (coordinates is BoardCoordinates)
            {
                BoardCoordinates board = new BoardCoordinates(coordinates);
                board.Modify(direction);
                if (board.Valid) return board;
                else return new OuterRingCoordinates(board);
            }
            else if (coordinates is OuterRingCoordinates)
            {
                if (direction != Direction.Clockwise && direction != Direction.Counterclockwise)
                {
                    direction = ConvertDirection(coordinates as OuterRingCoordinates, direction);
                }
                ICoordinates outer = new OuterRingCoordinates(coordinates);
                outer.Modify(direction);
                return outer;
            }
            return null;
        }

        /// <summary>
        /// Determines if two coordinates are next to each other
        /// </summary>
        /// <param name="src">First coordinates object</param>
        /// <param name="dest">Second coordinate object</param>
        /// <returns></returns>
        public static bool IsNextTo(BoardCoordinates source, BoardCoordinates destination)
        {
            if (source == null || destination == null)
            {
                return false;
            }
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (NextCoordinates(source, dir).Equals(destination))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines the direction based upon two coordinates
        /// </summary>
        /// <param name="src">First coordinates object</param>
        /// <param name="dest">Second coordinate object</param>
        /// <returns></returns>
        public static Direction MoveDirection(BoardCoordinates source, BoardCoordinates destination)
        {
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (Rules.NextCoordinates(source, dir).Equals(destination))
                {
                    return dir;
                }
            }
            return Direction.None;
        }

        /// <summary>
        /// Converts the pushing direction into a direction of outer ring movement
        /// </summary>
        /// <param name="coord">the coordinates a marble got pushed out to</param>
        /// <param name="direction">the direction the push was done</param>
        /// <returns></returns>
        public static Direction ConvertDirection(OuterRingCoordinates coordinates, Direction direction)
        {
            if (direction == Direction.Clockwise || direction == Direction.Counterclockwise)
            {
                return direction;
            }
            Direction clw = Direction.None, ccw = Direction.None;
            if (coordinates.Position < 6)
            {
                clw = Direction.UpRight;    //2
                ccw = Direction.DownLeft;   //5
            }
            else if (coordinates.Position < 11)
            {
                clw = Direction.UpLeft;     //1
                ccw = Direction.DownRight;  //4
            }
            else if (coordinates.Position < 16)
            {
                clw = Direction.Left;       //6
                ccw = Direction.Right;      //3
            }
            else if (coordinates.Position < 21)
            {
                clw = Direction.DownLeft;   //5
                ccw = Direction.UpRight;    //2
            }
            else if (coordinates.Position < 26)
            {
                clw = Direction.DownRight;  //4
                ccw = Direction.UpLeft;     //1
            }
            else
            {
                clw = Direction.Right;      //3
                ccw = Direction.Left;       //6
            }
            int clwCount = 0, ccwCount = 0;
            Direction mod = direction;
            while (mod != clw)
            {
                clwCount++;
                if ((byte)mod + 1 == 7) mod = (Direction)1;
                else mod = mod + 1;
            }
            mod = direction;
            while (mod != ccw)
            {
                ccwCount++;
                if ((byte)mod - 1 == 0) mod = (Direction)6;
                else mod = mod - 1;
            }
            if (clwCount < ccwCount)
            {
                return Direction.Clockwise;
            }
            return Direction.Counterclockwise;
        }

    }
}
