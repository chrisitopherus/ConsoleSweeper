using Minesweeper.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper.Board;

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
            return this.isMarked;
        }

        set
        {
            this.isMarked = value;
        }
    }

    public bool IsRevealed
    {
        get
        {
            return this.isRevealed;
        }

        set
        {
            this.isRevealed = value;
        }
    }
    public abstract void Accept(ICellVisitor visitor);

    public void Reveal()
    {
        this.IsRevealed = true;
        this.IsMarked = false;
    }

    public bool ToggleMark()
    {
        if (!this.IsRevealed)
        {
            this.IsMarked = !IsMarked;
        }

        return this.IsMarked;
    }
}