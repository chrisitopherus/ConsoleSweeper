using Minesweeper.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper.Events;

public class CellsUpdatedEventArgs : EventArgs
{
    public CellsUpdatedEventArgs(List<CellChangeInfo> updatedCells)
    {
        this.UpdatedCells = updatedCells;
    }

    public List<CellChangeInfo> UpdatedCells { get; }
}