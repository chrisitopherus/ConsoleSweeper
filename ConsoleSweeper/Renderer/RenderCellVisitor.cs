using Minesweeper.Cells;
using Minesweeper.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Renderer;

public class RenderCellVisitor : ICellVisitor
{
    public void Visit(Mine mine)
    {
        char sprite;
        if (mine.IsRevealed)
        {
            sprite = Sprites.Mine;
        } else if (mine.IsMarked)
        {
            sprite = Sprites.Flag;
        }
        else
        {
            sprite = Sprites.Unrevealed;
        }

        Console.Write(sprite);
    }

    public void Visit(Tile tile)
    {
        char sprite;
        if (tile.IsRevealed)
        {
            if (tile.IsEmpty)
            {
                sprite = Sprites.Empty;
            }
            else
            {
                sprite = Convert.ToChar(tile.NearbyMineCount + '0');
            }
        }
        else if (tile.IsMarked)
        {
            sprite = Sprites.Flag;
        }
        else
        {
            sprite = Sprites.Unrevealed;
        }

        Console.Write(sprite);
    }
}
