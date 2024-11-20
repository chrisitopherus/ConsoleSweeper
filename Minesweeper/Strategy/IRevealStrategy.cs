using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minesweeper.Board;

namespace Minesweeper.Strategy;

public interface ICellRevealStrategy<T> where T : GameCell
{
    List<GameCell> Reveal(GameBoard board, T cell, BoardPosition position);
}