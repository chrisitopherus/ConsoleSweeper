using Minesweeper.Board;
using Minesweeper.Cells;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Strategy;

public class SurroundingTilesRevealStrategy() : ICellRevealStrategy
{
    private readonly EmptyTilesRevealStrategy emptyTilesRevealStrategy = new EmptyTilesRevealStrategy();
    public List<CellChangeInfo> Reveal(GameBoard board, ICellInfo cellInfo)
    {
        if (cellInfo.Cell.Type == CellType.Mine || cellInfo.Cell.IsEmpty || !cellInfo.Cell.IsRevealed)
        {
            throw new ArgumentException("The cell must be a non empty revealed tile for this strategy.", nameof(cellInfo.Cell));
        }

        List<CellChangeInfo> revealedCells = [];

        foreach (ICellInfo neighborCellInfo in board.GetNeighborsOfCell(cellInfo))
        {
            // skip if cell is already revealed
            if (neighborCellInfo.Cell.IsRevealed) continue;

            // reveal the cell depending on the type of the cell
            revealedCells.AddRange(this.RevealCell(neighborCellInfo, board));
        }

        return revealedCells;
    }

    /// <summary>
    /// Has to be called before the cell was revealed, because it determines the <see cref="CellChangeType"/> based on the current state.
    /// </summary>
    /// <param name="cell">The cell that is about to be revealed.</param>
    /// <returns>The <see cref="CellChangeType"/>.</returns>
    private CellChangeType DetermineRevealChangeType(GameCell cell)
    {
        return cell.IsMarked ? CellChangeType.UnmarkedAndRevealed : CellChangeType.Revealed;
    }

    private List<CellChangeInfo> RevealCell(ICellInfo cellInfo, GameBoard board)
    {
        // cell is already revealed -> nothing to do
        if (cellInfo.Cell.IsRevealed) return [];

        switch (cellInfo.Cell.Type)
        {
            case CellType.Mine:
                // if mine is marked -> no explosion
                if (cellInfo.Cell.IsMarked)
                {
                    return [];
                }
                else // mine
                {
                    CellChangeType changeType = this.DetermineRevealChangeType(cellInfo.Cell);
                    cellInfo.Cell.Reveal();
                    return [new CellChangeInfo(cellInfo, changeType)];
                }
            case CellType.Tile:
                // empty tile -> use reveal strategy
                if (cellInfo.Cell.IsEmpty)
                {
                    return this.emptyTilesRevealStrategy.Reveal(board, cellInfo);
                }
                else // not empty tile -> reveal
                {
                    CellChangeType changeType = this.DetermineRevealChangeType(cellInfo.Cell);
                    cellInfo.Cell.Reveal();
                    return [new CellChangeInfo(cellInfo, changeType)];
                }
            default:
                return [];
        }
    }
}
