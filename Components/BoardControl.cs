using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

using GameLibrary;

namespace Components
{
    public partial class BoardControl : UserControl
    {
        #region Fields

        private Image _whiteMarble;
        private Image _blackMarble;
        private Image _selectMarble;
        private Image _moveUpRight;
        private Image _moveUpLeft;
        private Image _moveLeft;
        private Image _moveRight;
        private Image _moveDownRight;
        private Image _moveDownLeft;

        private Board _board;
        private MarbleColor _currentPlayer;
        private BoardCoordinates _lastCoord;
        private Direction _movementDir;
        private List<BoardCoordinates> _dragChain;

        #endregion

        #region Properties

        [DefaultValue(false), EditorBrowsable(EditorBrowsableState.Never)]
        new public bool DoubleBuffered
        {
            get
            {
                return base.DoubleBuffered;
            }
            set
            {
                base.DoubleBuffered = value;
            }
        }

        [DefaultValue((Image)null), RefreshProperties(RefreshProperties.Repaint)]
        [Category("Data"), Description("Bestimmt das Bild für eine weisse Kugel.")]
        public Image WhiteMarble
        {
            get
            {
                return _whiteMarble;
            }
            set
            {
                if (_whiteMarble != value)
                {
                    _whiteMarble = value;
                }
            }
        }

        [DefaultValue((Image)null), RefreshProperties(RefreshProperties.Repaint)]
        [Category("Data"), Description("Bestimmt das Bild für eine schwarze Kugel.")]
        public Image BlackMarble
        {
            get
            {
                return _blackMarble;
            }
            set
            {
                if (_blackMarble != value)
                {
                    _blackMarble = value;
                }
            }
        }

        [DefaultValue((Image)null), RefreshProperties(RefreshProperties.Repaint)]
        [Category("Data"), Description("Bestimmt das Bild für eine gewählte Kugel.")]
        public Image SelectMarble
        {
            get
            {
                return _selectMarble;
            }
            set
            {
                if (_selectMarble != value)
                {
                    _selectMarble = value;
                }
            }
        }

        [DefaultValue((Image)null), RefreshProperties(RefreshProperties.Repaint)]
        [Category("Data"), Description("Bestimmt das Bild für die Bewegung nach rechts oben.")]
        public Image MoveUpRightMarble
        {
            get
            {
                return _moveUpRight;
            }
            set
            {
                if (_moveUpRight != value)
                {
                    _moveUpRight = value;
                }
            }
        }

        [DefaultValue((Image)null), RefreshProperties(RefreshProperties.Repaint)]
        [Category("Data"), Description("Bestimmt das Bild für die Bewegung nach links oben.")]
        public Image MoveUpLeftMarble
        {
            get
            {
                return _moveUpLeft;
            }
            set
            {
                if (_moveUpLeft != value)
                {
                    _moveUpLeft = value;
                }
            }
        }

        [DefaultValue((Image)null), RefreshProperties(RefreshProperties.Repaint)]
        [Category("Data"), Description("Bestimmt das Bild für die Bewegung nach links.")]
        public Image MoveLeftMarble
        {
            get
            {
                return _moveLeft;
            }
            set
            {
                if (_moveLeft != value)
                {
                    _moveLeft = value;
                }
            }
        }

        [DefaultValue((Image)null), RefreshProperties(RefreshProperties.Repaint)]
        [Category("Data"), Description("Bestimmt das Bild für die Bewegung nach rechts.")]
        public Image MoveRightMarble
        {
            get
            {
                return _moveRight;
            }
            set
            {
                if (_moveRight != value)
                {
                    _moveRight = value;
                }
            }
        }

        [DefaultValue((Image)null), RefreshProperties(RefreshProperties.Repaint)]
        [Category("Data"), Description("Bestimmt das Bild für die Bewegung nach rechts unten.")]
        public Image MoveDownRightMarble
        {
            get
            {
                return _moveDownRight;
            }
            set
            {
                if (_moveDownRight != value)
                {
                    _moveDownRight = value;
                }
            }
        }

        [DefaultValue((Image)null), RefreshProperties(RefreshProperties.Repaint)]
        [Category("Data"), Description("Bestimmt das Bild für die Bewegung nach links unten.")]
        public Image MoveDownLeftMarble
        {
            get
            {
                return _moveDownLeft;
            }
            set
            {
                if (_moveDownLeft != value)
                {
                    _moveDownLeft = value;
                }
            }
        }

        [Browsable(false)]
        public Board Board
        {
            get
            {
                return _board;
            }
            set
            {
                if (_board != value)
                {
                    _board = value;
                    Invalidate();
                }
            }
        }

        #endregion

        public BoardControl()
        {
            InitializeComponent();
            _movementDir = Direction.None;
            _currentPlayer = MarbleColor.Black;
            _dragChain = new List<BoardCoordinates>();
        }

        #region Private Implementation

        /// <summary>
        /// Draws the marbles on the board graphics object
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawMarbles(Graphics graphics)
        {
            foreach (Marble m in _board.Marbles)
            {
                Image marbleImage = null;
                if (m.IsSelected)
                {
                    if (_movementDir == Direction.None)
                    {
                        marbleImage = SelectMarble;
                    }
                    else
                    {
                        switch (_movementDir)
                        {
                            case Direction.UpLeft:
                                marbleImage = MoveUpLeftMarble;
                                break;
                            case Direction.UpRight:
                                marbleImage = MoveUpRightMarble;
                                break;
                            case Direction.Left:
                                marbleImage = MoveLeftMarble;
                                break;
                            case Direction.Right:
                                marbleImage = MoveRightMarble;
                                break;
                            case Direction.DownLeft:
                                marbleImage = MoveDownLeftMarble;
                                break;
                            case Direction.DownRight:
                                marbleImage = MoveDownRightMarble;
                                break;
                        }
                    }
                }
                else
                {
                    switch (m.Color)
                    {
                        case MarbleColor.Black:
                            marbleImage = BlackMarble;
                            break;
                        case MarbleColor.White:
                            marbleImage = WhiteMarble;
                            break;
                    }
                }
                if (marbleImage != null)
                {
                    if (m.IsOut)
                    {
                        Point location = GetMarbleLocation(m.Coordinates);
                        location.Offset(-marbleImage.Width / 2, -marbleImage.Height / 2);
                        graphics.DrawImage(marbleImage, location);
                    }
                    else
                    {
                        Point location = GetMarbleLocation(m.Coordinates);
                        location.Offset(-marbleImage.Width / 2, -marbleImage.Height / 2);
                        graphics.DrawImage(marbleImage, location);
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the distance between two points
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <returns>Calculated distance between two points</returns>
        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private Point GetMarbleLocation(ICoordinates coord)
        {
            if (coord is BoardCoordinates)
            {
                return BoardCoordinatesToPoint(coord.Row, coord.Diagonal);
            }
            else
            {
                return OuterRingCoordinatesToPoint(coord.Row, coord.Diagonal);
            }
        }

        private Point OuterRingCoordinatesToPoint(int row, int diag)
        {
            int x = (int)(diag * 40 - row * 20 + 160);
            int y = (int)(row * 34.5 + 60);
            return new Point(x, y);
        }

        private Point BoardCoordinatesToPoint(int row, int diag)
        {
            int x = diag * 36 - row * 18 + 169;
            int y = (int)(row * 31.5 + 75);
            return new Point(x, y);
        }

        private BoardCoordinates PointToCoordinates(Point point)
        {
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    BoardCoordinates coord = new BoardCoordinates(i, j);
                    if (coord.Valid && Distance(point, GetMarbleLocation(coord)) < 18)
                    {
                        return coord;
                    }
                }
            }
            return null;
        }

        private bool HandleFieldEnter(BoardCoordinates coord)
        {
            if (_board == null || coord == null || !coord.Valid)
            {
                return false;
            }
            Marble marble = _board.GetMarble(coord, true);
            if (marble != null && marble.Color == _currentPlayer)
            {
                //marble belongs to current player
                if (!_dragChain.Contains(coord))
                {
                    //marble is not selected -> select the marble
                    if (_dragChain.Count >= 3)
                    {
                        return false;
                    }
                    if (_dragChain.Count > 0 && !Rules.IsNextTo(_dragChain[_dragChain.Count - 1], coord))
                    {
                        return false;
                    }
                    if (_dragChain.Count > 1 && Rules.MoveDirection(_dragChain[0], _dragChain[1]) != Rules.MoveDirection(_dragChain[1], coord))
                    {
                        return false;
                    }
                    marble.IsSelected = true;
                    _dragChain.Add(marble.Coordinates as BoardCoordinates);
                    Invalidate();
                    return true;
                }
                else
                {
                    //a marble is already selected -> deselect the marbles selected after this one
                    int index = _dragChain.IndexOf(coord);
                    if (index < _dragChain.Count - 1)
                    {
                        for (int i = _dragChain.Count - 1; i > index; i--)
                        {
                            Marble deselect = _board.GetMarble(_dragChain[i], true);
                            deselect.IsSelected = false;
                            Invalidate();
                            _dragChain.RemoveAt(i);
                        }
                        return true;
                    }
                    else
                    {
                        _movementDir = Direction.None;
                        Invalidate();
                    }
                }
            }
            else if (_dragChain.Count > 0)
            {
                //no or a contestant marble at the given location -> check validity of turn
                List<Marble> marbles = new List<Marble>();
                foreach (BoardCoordinates c in _dragChain)
                {
                    Marble m = _board.GetMarble(c, true);
                    marbles.Add(m);
                }
                Direction dir = Rules.MoveDirection(_dragChain[_dragChain.Count - 1], coord);
                if (Rules.MoveIsValid(_board, marbles, dir))
                {
                    //set the direction indicator
                    _movementDir = dir;
                    Invalidate();
                    return true;
                }
                else if (_movementDir != Direction.None)
                {
                    //clear direction indicator
                    _movementDir = Direction.None;
                    Invalidate();
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Overrides

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            if (_board != null) DrawMarbles(e.Graphics);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            BoardCoordinates pointCoord = PointToCoordinates(e.Location);
            if (_board == null || pointCoord != null && pointCoord.Valid)
            {
                if (HandleFieldEnter(pointCoord))
                {
                    _lastCoord = pointCoord;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_board == null || e.Button != MouseButtons.Left || _dragChain.Count == 0)
            {
                return;
            }
            BoardCoordinates pointCoord = PointToCoordinates(e.Location);
            if (pointCoord != null && pointCoord.Valid && !pointCoord.Equals(_lastCoord))
            {
                _lastCoord = pointCoord;
                HandleFieldEnter(pointCoord);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_board == null || e.Button != MouseButtons.Left || _dragChain.Count == 0)
            {
                return;
            }
            List<Marble> marbles = new List<Marble>();
            foreach (BoardCoordinates coord in _dragChain)
            {
                Marble m = _board.GetMarble(coord, true);
                m.IsSelected = false;
                marbles.Add(m);
            }
            if (_board.DoMove(marbles, _movementDir))
            {
                _currentPlayer = (MarbleColor)((((byte)_currentPlayer) + 1) % 2);
            }
            _dragChain.Clear();
            _movementDir = Direction.None;
            Invalidate();
        }

        #endregion
    }
}
