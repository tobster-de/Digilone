using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    /// <summary>
    /// Class for handling with board coodinates
    /// </summary>
    /// <remarks>
    ///                 1   2   3   4   5    Diagonal
    ///                /   /   /   /   /  6
    ///   Row    1 -  o   o   o   o   o  /  7
    ///        2 -  o   o   o   o   o   o  /  8
    ///      3 -  o   o   o   o   o   o   o  /  9
    ///    4 -  o   o   o   o   o   o   o   o  /
    ///  5 -  o   o   o   o   o   o   o   o   o
    ///    6 -  o   o   o   o   o   o   o   o
    ///      7 -  o   o   o   o   o   o   o
    ///        8 -  o   o   o   o   o   o
    ///          9 -  o   o   o   o   o
    /// </remarks>    
    public class BoardCoordinates : ICoordinates
    {
        #region Fields

        private int _row;
        private int _diag;

        #endregion

        #region Properties

        /// <summary>
        /// Row coordinate
        /// </summary>
        public int Row
        {
            get
            {
                return _row;
            }
            private set
            {
                _row = value;
            }
        }

        /// <summary>
        /// Diagonal coordinate
        /// </summary>
        public int Diagonal
        {
            get
            {
                return _diag;
            }
            private set
            {
                _diag = value;
            }
        }

        /// <summary>
        /// States whether this coordinates are valid board coordinates
        /// </summary>
        public bool Valid
        {
            get
            {
                //this statement itself may be not easy to understand
                //it is retrieved applying the de morgan laws several 
                //times on the statement below
                return
                    (_diag > 0 && (_row < 6 || _diag > _row - 5)) &&
                    (_diag < 10 && (_row > 4 || _diag < _row + 5)) &&
                    (_row > 0) && (_row < 10);

                //DO NOT DELETE THIS! The way is the goal!
                //!(
                ////move to the left
                //(_diag < 1 || (_row > 5 && _diag < _row - 4)) ||
                ////move to the right
                //(_diag > 9 || (_row < 5 && _diag > _row + 4)) ||
                ////move upwards, move downwards
                //(_row < 1) || (_row > 9)
                //);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">Row coordinate</param>
        /// <param name="diagonal">Diagonal coordinate</param>
        public BoardCoordinates(int row, int diagonal)
        {
            Row = row;
            Diagonal = diagonal;
        }

        /// <summary>
        /// Copy-Constructor
        /// </summary>
        /// <param name="coord">Original object</param>
        public BoardCoordinates(ICoordinates coordinates)
        {
            Row = coordinates.Row;
            Diagonal = coordinates.Diagonal;
        }

        #endregion

        #region Public Implementation

        /// <summary>
        /// Modify coordinates on the board. Do not directly use this to move a marble.
        /// </summary>
        /// <param name="direction">The direction to move</param>
        public void Modify(Direction direction)
        {
            if (direction == Direction.Left || direction == Direction.UpLeft)
            {
                Diagonal--;
            }
            if (direction == Direction.UpRight || direction == Direction.UpLeft)
            {
                Row--;
            }
            if (direction == Direction.Right || direction == Direction.DownRight)
            {
                Diagonal++;
            }
            if (direction == Direction.DownRight || direction == Direction.DownLeft)
            {
                Row++;
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            string s = String.Empty;
            if (!Valid)
            {
                s = "Invalid ";
            }
            s = s + String.Format("Coord: D={0}, R={1}", Diagonal, Row);
            return s;
        }

        public override bool Equals(object obj)
        {
            if (obj is BoardCoordinates)
            {
                BoardCoordinates other = obj as BoardCoordinates;
                return other.Diagonal == Diagonal && other.Row == Row;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Diagonal.GetHashCode() * 61 + Row.GetHashCode();
        }

        #endregion
    }
}
