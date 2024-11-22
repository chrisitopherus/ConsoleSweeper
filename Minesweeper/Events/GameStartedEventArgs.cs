using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Events;

public class GameStartedEventArgs : EventArgs
{
    public GameStartedEventArgs(GameConfiguration configuration)
    {
        this.Rows = configuration.Rows;
        this.Cols = configuration.Cols;
        this.MineCount = configuration.MineCount;
    }

    public int Rows
    {
        get;
        private set;
    }

    public int Cols
    {
        get;
        private set;
    }

    public int MineCount
    {
        get;
        private set;
    }
}
