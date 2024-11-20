using Minesweeper.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper;

public class GameCursor
{
    private GameBoard board;
    private BoardPosition currentPosition;

    public GameCursor(GameBoard board)
    {
        this.board = board;
        this.currentPosition = new BoardPosition(0, 0);
    }

    public BoardPosition CurrentPosition
    {
        get
        {
            return this.currentPosition;
        }

        private set
        {
            if (!this.board.IsValidBoardPosition(value))
            {
                throw new ArgumentOutOfRangeException(nameof(this.CurrentPosition), "The position must be inside the board.");
            }

            this.currentPosition = value;
        }
    }

    /// <summary>
    /// Jump with the cursor to a specific row and column.
    /// </summary>
    /// <param name="row">The row number, starting with 0.</param>
    /// <param name="col">The column number, starting with 1.</param>
    /// <returns>The new board position.</returns>
    public void Jump(int row, int col)
    {
        this.CurrentPosition = new BoardPosition(row, col);
    }

    public void MoveUp()
    {
        this.Move(-1, 0);
    }

    public void MoveDown()
    {
        this.Move(1, 0);
    }

    public void MoveLeft()
    {
        this.Move(0, -1);
    }

    public void MoveRight()
    {
        this.Move(0, 1);
    }

    private void Move(int rowDelta, int colDelta)
    {
        // Example: 4x4, current row = 0, we move up -> -1 : 0 + -1 + 4 = 3, 3 % 4 = 3 -> so it jumped to the bottom
        int newRow = (currentPosition.Row + rowDelta + this.board.Config.Rows) % this.board.Config.Rows;
        int newCol = (currentPosition.Col + colDelta + this.board.Config.Cols) % this.board.Config.Cols;
        this.CurrentPosition = new BoardPosition(newRow, newCol);
    }
}
