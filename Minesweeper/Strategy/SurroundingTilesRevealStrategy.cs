using Minesweeper.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Strategy;

public class SurroundingTilesRevealStrategy : ICellRevealStrategy<GameCell>
{
    public List<GameCell> Reveal(GameBoard board, GameCell cell, BoardPosition position)
    {
        if (cell.Type == CellType.Mine || cell.IsEmpty)
        {
            throw new ArgumentException("The cell must be a tile for this strategy.", nameof(cell));
        }

        List<GameCell> revealedCells = new List<GameCell>();

        return revealedCells;
    }
}
