using Minesweeper.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper.Cells;

public class Mine : GameCell
{
    public Mine()
        : base(CellType.Mine)
    {
    }

    public override bool IsEmpty => false;

    public override void Accept(ICellVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override string ToString()
    {
        return "X";
    }
}