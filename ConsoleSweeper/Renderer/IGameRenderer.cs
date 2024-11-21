using Minesweeper.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleSweeper.Renderer;

public interface IGameRenderer
{
    void DrawCursorUpdate(BoardPosition previousPosition, BoardPosition newPosition);
    void DrawCellsUpdate(List<BoardPosition> updatedCellsPositions);
    void DrawGame();
}