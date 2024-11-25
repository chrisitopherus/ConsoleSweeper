using Minesweeper.Board;
using Minesweeper.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Strategy;

/// <summary>
/// Strategy for revealing all connected empty tiles and their borders.
/// </summary>W
public class EmptyTilesRevealStrategy : ICellRevealStrategy
{
    public List<CellChangeInfo> Reveal(GameBoard board, ICellInfo cellInfo)
    {
        if (!cellInfo.Cell.IsEmpty)
        {
            throw new ArgumentException("The cell must be empty for this strategy.", nameof(cellInfo.Cell));
        }

        List<CellChangeInfo> revealedCells = [];
        Stack<ICellInfo> stack = new Stack<ICellInfo>();

        // pushing the first empty tile to the stack
        stack.Push(cellInfo);

        while (stack.Count > 0)
        {
            ICellInfo currentCellInfo = stack.Pop();

            // skip if cell is already revealed
            // if (currentCellInfo.Cell.IsRevealed) continue;

            CellChangeType cellChangeType = this.DetermineChangeType(currentCellInfo.Cell);
            // reveal the cell
            currentCellInfo.Cell.Reveal();
            revealedCells.Add(new CellChangeInfo(currentCellInfo, cellChangeType));

            // if cell is not empty, its a border
            if (!currentCellInfo.Cell.IsEmpty) continue;

            // reveal surrounding cells
            foreach (ICellInfo neighborCellInfo in board.GetNeighborsOfCell(currentCellInfo))
            {
                if (!neighborCellInfo.Cell.IsRevealed)
                {
                    stack.Push(neighborCellInfo);
                }
            }
        }

        return revealedCells;
    }

    private CellChangeType DetermineChangeType(GameCell cell)
    {
        return cell.IsMarked ? CellChangeType.UnmarkedAndRevealed : CellChangeType.Revealed;
    }
}
