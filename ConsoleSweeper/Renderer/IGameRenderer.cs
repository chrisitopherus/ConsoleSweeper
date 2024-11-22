using Minesweeper.Board;
using Minesweeper.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleSweeper.Renderer;

public interface IGameRenderer
{
    void RenderCursorUpdate(BoardPosition previousPosition, BoardPosition newPosition);
    void RenderCellsUpdate(List<CellInfo> updatedCellInfos);
    void RenderGame();
}