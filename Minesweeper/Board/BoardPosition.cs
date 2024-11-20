using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper.Board
{
    public struct BoardPosition
    {
        private int row;
        private int col;

        public BoardPosition(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        public int Row
        {
            get
            {
                return this.row;
            }

            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(Row), "The row value must be positive");
                this.row = value;
            }
        }

        public int Col
        {
            get
            {
                return this.col;
            }

            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(Col), "The column value must be positive");
                this.col = value;
            }
        }
    }
}