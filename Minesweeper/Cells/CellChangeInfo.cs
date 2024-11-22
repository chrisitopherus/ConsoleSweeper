using Minesweeper.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Cells;

public class CellChangeInfo : CellInfo, ICellInfo
{
    private readonly CellChangeType changeType;
    public CellChangeInfo(GameCell cell, BoardPosition position, CellChangeType changeType)
        : base(cell, position)
    {
        this.changeType = changeType;
    }

    public CellChangeInfo(ICellInfo info, CellChangeType changeType)
        : base(info.Cell, info.Position)
    {
        this.changeType = changeType;
    }

    public CellChangeType ChangeType => this.changeType;

}
