using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minesweeper.Board;
using Minesweeper.Cells;

namespace Minesweeper.Strategy;

public interface ICellRevealStrategy<T> where T : GameCell
{
    List<CellChangeInfo> Reveal(GameBoard board, T cell, BoardPosition position);
}