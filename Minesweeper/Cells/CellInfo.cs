using Minesweeper.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Cells;

public class CellInfo : ICellInfo
{
    private readonly GameCell cell;
    private readonly BoardPosition position;
    public CellInfo(GameCell cell, BoardPosition position)
    {
        this.cell = cell;
        this.position = position;
    }

    public GameCell Cell => cell;

    public BoardPosition Position => position;
}
