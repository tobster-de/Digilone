using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    /// <summary>
    /// Class for handling with coodinates on the outer ring
    /// </summary>
    /// <remarks>
    ///                      30  29  28  27  26
    ///                      /   /   /   /   /
    ///            1 -  o   o   o   o   o   o  
    ///          2 -  o   .   .   .   .   .   o  - 25
    ///        3 -  o   .   .   .   .   .   .   o  - 24
    ///      4 -  o   .   .   .   .   .   .   .   o  - 23
    ///    5 -  o   .   .   .   .   .   .   .   .   o  - 22
    ///  6 -  o   .   .   .   .   .   .   .   .   .   o  - 21
    ///    7 -  o   .   .   .   .   .   .   .   .   o  - 20
    ///      8 -  o   .   .   .   .   .   .   .   o  - 19
    ///        9 -  o   .   .   .   .   .   .   o  - 18
    ///         10 -  o   .   .   .   .   .   o  - 17
    ///                 o   o   o   o   o   o
    ///                /   /   /   /   /   /
    ///               11  12  13  14  15  16
    /// </remarks>    
    public class OuterRingCoordinates : ICoordinates
    {
        #region Fields

        private int _position;
        //private int _row;
        //private int _diag;

        #endregion

        #region Properties

        /// <summary>
        /// Row coordinate
        /// </summary>
        public int Position
        {
            get
            {
                return _position;
            }
            private set
            {
                _position = value;
            }
        }

        /// <summary>
        /// Row coordinate
        /// </summary>
        public int Row
        {
            get
            {
                return PositionToRow(_position);
            }
        }

        /// <summary>
        /// Diagonal coordinate
        /// </summary>
        public int Diagonal
        {
            get
            {
                return PositionToDiag(_position);
            }
        }

        /// <summary>
        /// States whether this coordinates are valid ring coordinates
        /// </summary>
        public bool Valid
        {
            get
            {
                return _position > 0 && _position < 31;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">Row coordinate</param>
        /// <param name="diagonal">Diagonal coordinate</param>
        public OuterRingCoordinates(int row, int diagonal)
        {
            for (int i = 1; i < 31; i++)
            {
                if (PositionToRow(i) == row && PositionToDiag(i) == diagonal)
                {
                    _position = i;
                }
            }
        }

        public OuterRingCoordinates(int position)
        {
            _position = position % 31;
        }

        /// <summary>
        /// Copy-Constructor
        /// </summary>
        /// <param name="coord">Original object</param>
        public OuterRingCoordinates(ICoordinates coordinates)
        {
            for (int i = 1; i < 31; i++)
            {
                if (PositionToRow(i) == coordinates.Row && PositionToDiag(i) == coordinates.Diagonal)
                {
                    _position = i;
                }
            }
        }

        #endregion

        #region Public Implementation

        /// <summary>
        /// Modify coordinates on the board. Do not directly use this to move a marble.
        /// </summary>
        /// <param name="direction">The direction to move</param>
        public void Modify(Direction direction)
        {
            if (direction == Direction.Clockwise)
            {
                Position = (Position + 30) % 31;
            }
            if (direction == Direction.Counterclockwise)
            {
                Position = (Position + 1) % 31;
            }
        }

        #endregion

        #region Private Implementation

        private static int PositionToRow(int position)
        {
            if (position < 11)
            {
                return position - 1;
            }
            if (position < 17)
            {
                return 10;
            }
            if (position < 26)
            {
                return 26 - position;
            }
            return 0;
        }

        private static int PositionToDiag(int position)
        {
            if (position < 7)
            {
                return 0;
            }
            if (position < 17)
            {
                return position - 6;
            }
            if (position < 22)
            {
                return 10;
            }
            return 31 - position;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            string s = String.Empty;
            s = s + String.Format("Outer Ring: D={0}, R={1} (P={2})", Diagonal, Row, Position);
            return s;
        }

        public override bool Equals(object obj)
        {
            if (obj is OuterRingCoordinates)
            {
                OuterRingCoordinates other = obj as OuterRingCoordinates;
                return other.Position == Position;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Diagonal.GetHashCode() * 87 + Row.GetHashCode() * 31;
        }

        #endregion
    }
}
