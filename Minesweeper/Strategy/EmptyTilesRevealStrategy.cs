using Minesweeper.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Strategy;

/// <summary>
/// Strategy for revealing all connected empty tiles and their borders.
/// </summary>W
public class EmptyTilesRevealStrategy : ICellRevealStrategy<GameCell>
{
    public List<GameCell> Reveal(GameBoard board, GameCell cell, BoardPosition position)
    {
        if (!cell.IsEmpty)
        {
            throw new ArgumentException("The cell must be empty for this strategy.", nameof(cell));
        }

        List<GameCell> revealedCells = new List<GameCell>();
        Stack<CellInfo> stack = new Stack<CellInfo>();

        // pushing the first empty tile to the stack
        stack.Push(new CellInfo(cell, position));

        while (stack.Count > 0)
        {
            CellInfo cellInfo = stack.Pop();
            GameCell currentCell = cellInfo.Cell;
            BoardPosition currentCellPosition = cellInfo.Position;

            // skip if cell is already revealed
            if (currentCell.IsRevealed) continue;

            // reveal the cell
            currentCell.Reveal();
            revealedCells.Add(currentCell);

            // if cell is not empty, its a border
            if (!currentCell.IsEmpty) continue;

            // reveal surrounding cells
            for (int offsetRow = -1; offsetRow <= 1; offsetRow++)
            {
                for (int offsetCol = -1; offsetCol <= 1; offsetCol++)
                {
                    // if its the same cell (no offsets applied) -> skip
                    if (offsetRow == 0 && offsetCol == 0) continue;

                    int neighborRow = currentCellPosition.Row + offsetRow;
                    int neighborCol = currentCellPosition.Col + offsetCol;

                    // if its not a valid position (outside of the board) -> skip
                    if (!board.IsValidBoardPosition(neighborRow, neighborCol)) continue;
                    BoardPosition neighborPosition = new BoardPosition(neighborRow, neighborCol);
                    GameCell neighborCell = board.GetCellAt(neighborPosition);

                    if (!neighborCell.IsRevealed)
                    {
                        stack.Push(new CellInfo(neighborCell, neighborPosition));
                    }
                }
            }
        }

        return revealedCells;
    }
}
