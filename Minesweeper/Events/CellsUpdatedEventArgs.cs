using Minesweeper.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper.Events;

public class CellsUpdatedEventArgs : EventArgs
{
    public CellsUpdatedEventArgs(List<CellInfo> updatedCells)
    {
        this.UpdatedCells = updatedCells;
    }

    public List<CellInfo> UpdatedCells { get; }
}