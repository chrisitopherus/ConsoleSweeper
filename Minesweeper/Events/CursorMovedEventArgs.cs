using Minesweeper.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper.Events;

public class CursorMovedEventArgs : EventArgs
{
    private BoardPosition previousPosition;
    private BoardPosition newPosition;
    public CursorMovedEventArgs(BoardPosition previousPosition, BoardPosition newPosition)
    {
        this.previousPosition = previousPosition;
        this.newPosition = newPosition;
    }

    public BoardPosition PreviousPosition => this.previousPosition;

    public BoardPosition NewPosition => this.newPosition;
}