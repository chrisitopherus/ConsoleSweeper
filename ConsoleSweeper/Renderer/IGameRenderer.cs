using Minesweeper.Board;
using Minesweeper.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleSweeper.Renderer;

public interface IGameRenderer
{
    void RenderCursorUpdate(BoardPosition previousPosition, BoardPosition newPosition);
    void RenderCellsUpdate(IEnumerable<ICellInfo> updatedCellInfos);
    void RenderGame();
}