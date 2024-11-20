using Minesweeper.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper.Board;

public class Tile : GameCell
{
    private uint nearbyMineCount = 0;

    public Tile(uint nearbyMineCount)
        : base(CellType.Tile)
    {
        this.NearbyMineCount = nearbyMineCount;
    }

    public uint NearbyMineCount
    {
        get
        {
            return this.nearbyMineCount;
        }

        private set
        {
            if (value > 8) throw new ArgumentOutOfRangeException(nameof(NearbyMineCount), "The mine count must not be greater than 8.");
            this.nearbyMineCount = value;
        }
    }

    public override bool IsEmpty
    {
        get
        {
            return this.nearbyMineCount == 0;
        }
    }

    public override void Accept(ICellVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"{(this.NearbyMineCount > 0 ? this.NearbyMineCount : " ")}";
    }
}