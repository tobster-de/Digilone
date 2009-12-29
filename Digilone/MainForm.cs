using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using GameLibrary;

namespace Digilone
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Board board = new Board();
            board.InitStandardBegin();

            //for (int i = 1; i < 31; i++)
            //{
            //    board.NewMarble(MarbleColor.White, new OuterRingCoordinates(i));
            //}
            
            boardControl.Board = board;
        }
    }
}
