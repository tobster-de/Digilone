using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    /// <summary>
    /// This class represents the game board
    /// </summary>
    public class Board
    {
        #region Fields

        //static private Board _instance = null;
        private List<Marble> _marbles = new List<Marble>();

        #endregion

        #region Properties

        ///// <summary>
        ///// The singleton instance of a board
        ///// </summary>
        //static public Board Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            _instance = new Board();
        //        }
        //        return _instance;
        //    }
        //}

        /// <summary>
        /// Marbles that are currently on the board
        /// </summary>
        public List<Marble> Marbles
        {
            get
            {
                return new List<Marble>(_marbles);
            }
        }

        #endregion

        #region Construction

        public Board()
        {

        }

        #endregion

        #region Public Implementation

        /// <summary>
        /// Clears the Board
        /// </summary>
        public void ClearBoard()
        {
            _marbles.Clear();
        }

        /// <summary>
        /// Add a marble
        /// </summary>
        /// <param name="color">Color of the new marble</param>
        /// <param name="coordinates">Coordinates of the new marble</param>
        public void NewMarble(MarbleColor color, ICoordinates coordinates)
        {
            _marbles.Add(new Marble(color, coordinates));
        }

        /// <summary>
        /// Initializes the board with the standard formation
        /// </summary>
        public void InitStandardBegin()
        {
            ClearBoard();
            for (int i = 1; i <= 5; i++)
            {
                NewMarble(MarbleColor.Black, new BoardCoordinates(1, i));
                NewMarble(MarbleColor.White, new BoardCoordinates(9, i + 4));
            }
            for (int i = 1; i <= 6; i++)
            {
                NewMarble(MarbleColor.Black, new BoardCoordinates(2, i));
                NewMarble(MarbleColor.White, new BoardCoordinates(8, i + 3));
            }
            for (int i = 1; i <= 3; i++)
            {
                NewMarble(MarbleColor.Black, new BoardCoordinates(3, i + 2));
                NewMarble(MarbleColor.White, new BoardCoordinates(7, i + 4));
            }
        }

        /// <summary>
        /// Performs a valid move on the board 
        /// </summary>
        /// <param name="selected">marbles that are selected</param>
        /// <param name="direction">direction of push movement</param>
        /// <returns></returns>
        public bool DoMove(List<Marble> selected, Direction direction)
        {
            if (Rules.MoveIsValid(this, selected, direction))
            {
                MoveType mt = Rules.DetermineMoveType(selected, direction);
                if (mt == MoveType.Line)
                {
                    Marble current = selected[0], next = null;
                    while (current != null)
                    {
                        //get marble at the destination location of current marble
                        ICoordinates nc = Rules.NextCoordinates(current.Coordinates, direction);
                        next = GetMarble(nc, false);
                        //move the current marble afterwards, to prevent two marbles at same location
                        current.Move(direction);
                        if (current.IsOut)
                        {
                            //marble was thrown out, get the direction of r
                            direction = Rules.ConvertDirection(current.Coordinates as OuterRingCoordinates, direction);
                        }
                        //repeat until there is no marble left in the line
                        current = next;
                    }
                    return true;
                }
                else if (mt == MoveType.Broad)
                {
                    //simply move all selected marbles in the desired direction
                    foreach (Marble m in selected)
                    {
                        m.Move(direction);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the marble at given coordinates
        /// </summary>
        /// <param name="coord">coordinates of interest</param>
        /// <returns></returns>
        public Marble GetMarble(ICoordinates coordinates, bool onboard)
        {
            if ((coordinates is BoardCoordinates && onboard) || !onboard)
            {
                foreach (Marble m in _marbles)
                {
                    if (m.Coordinates.Equals(coordinates))
                        if (!onboard || (onboard && m.Coordinates is BoardCoordinates))
                        {
                            return m;
                        }
                }
            }
            return null;
        }

        #endregion

    }
}
