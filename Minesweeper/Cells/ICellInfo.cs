using Minesweeper.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Cells;

public interface ICellInfo
{
    public GameCell Cell { get; }

    public BoardPosition Position { get; }
}
