using Minesweeper.Board;
using Minesweeper.Cells;
using Minesweeper.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper;

public class LogicTestDrawer
{
    private EmptyTilesRevealStrategy emptyTilesRevealStrategy = new EmptyTilesRevealStrategy();
    public void Draw(GameBoard board)
    {
        for (int row = 0; row < board.GameField.GetLength(0); row++)
        {
            for (int col = 0; col < board.GameField.GetLength(1); col++)
            {
                GameCell cell = board.GameField[row, col];
                if (!cell.IsRevealed)
                {
                    Console.Write("█");
                } else
                {
                    Console.Write(cell);
                }
            }

            Console.WriteLine();
        }
    }

    public void Reveal(GameBoard board, GameCell cell, BoardPosition position)
    {
        if (!cell.IsEmpty)
        {
            cell.Reveal();
            return;
        }

        // use empty tile strategy
        var cells = this.emptyTilesRevealStrategy.Reveal(board, cell, position);

        foreach (var cellInfo in cells)
        {
            cellInfo.Cell.Reveal();
        }
    }
}
