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
    void RenderFlagAmmoUpdate(int flagAmmo);
    void RenderGame();
    void RenderLoss();
    void RenderWin();
    void RenderMenu();
    public void UnrenderMenu();
    void RenderMenuUpdate(int prevIndex, int newIndex);
}