using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minesweeper.Board;
using Minesweeper.Cells;

namespace Minesweeper.Strategy;

public interface ICellRevealStrategy
{
    List<CellChangeInfo> Reveal(GameBoard board, ICellInfo cellInfo);
}