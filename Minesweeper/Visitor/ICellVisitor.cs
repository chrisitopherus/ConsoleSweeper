using Minesweeper.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper.Visitor;

/// <summary>
/// Defines a cell visitor.
/// </summary>
public interface ICellVisitor
{
    /// <summary>
    /// Visits a mine.
    /// </summary>
    /// <param name="mine">The mine that is visited.</param>
    void Visit(Mine mine);

    /// <summary>
    /// Visits a tile.
    /// </summary>
    /// <param name="tile">The tile that is visited.</param>
    void Visit(Tile tile);
}