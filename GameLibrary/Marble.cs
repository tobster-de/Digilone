using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    /// <summary>
    /// Marble
    /// </summary>
    public class Marble
    {
        #region Fields

        private MarbleColor _color;
        private ICoordinates _coord;
        //private bool _out;
        private bool _selected;

        #endregion

        #region Properties

        /// <summary>
        /// Coordinates of the marble on the board
        /// </summary>
        public ICoordinates Coordinates
        {
            get
            {
                return _coord;
            }
            private set
            {
                _coord = value;
            }
        }

        /// <summary>
        /// Color of this marble
        /// </summary>
        public MarbleColor Color
        {
            get
            {
                return _color;
            }
            private set
            {
                _color = value;
            }
        }

        /// <summary>
        /// Determines whether the marble is out
        /// </summary>
        public bool IsOut
        {
            get
            {
                return Rules.MarbleIsOut(Coordinates); 
                    //_out;
            }
            //private set
            //{
            //    _out = value;
            //}
        }

        /// <summary>
        /// States whether this marble is selected
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
            }
        }
        
        #endregion

        #region Construction

        /// <summary>
        /// Create a new marble object
        /// </summary>
        /// <param name="color">Color of the marble</param>
        /// <param name="coordinates">Coordinates to place the marble</param>
        public Marble(MarbleColor color, ICoordinates coordinates)
        {
            Color = color;
            Coordinates = coordinates;
            //IsOut = Rules.MarbleIsOut(coordinates);
        }

        #endregion

        #region Public Implementation

        /// <summary>
        /// Moves the marble on the board
        /// </summary>
        /// <param name="direction">The direction to move</param>
        public void Move(Direction direction)
        {
            //Coordinates.Modify(direction);
            //IsOut = Rules.MarbleIsOut(Coordinates);
            Coordinates = Rules.NextCoordinates(Coordinates, direction);
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return String.Format("{0} marble at {1}", Color, Coordinates);
        }

        #endregion
    }
}
