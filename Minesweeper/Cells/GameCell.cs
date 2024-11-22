using Minesweeper.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper.Cells;

public abstract class GameCell(CellType type)
{
    private bool isMarked = false;
    private bool isRevealed = false;

    public abstract bool IsEmpty { get; }

    public CellType Type => type;

    public bool IsMarked
    {
        get
        {
            return isMarked;
        }

        set
        {
            isMarked = value;
        }
    }

    public bool IsRevealed
    {
        get
        {
            return isRevealed;
        }

        set
        {
            isRevealed = value;
        }
    }
    public abstract void Accept(ICellVisitor visitor);

    public void Reveal()
    {
        IsRevealed = true;
        IsMarked = false;
    }

    public bool ToggleMark()
    {
        if (!IsRevealed)
        {
            IsMarked = !IsMarked;
        }

        return IsMarked;
    }
}