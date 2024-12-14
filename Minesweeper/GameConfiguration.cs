using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper;

public class GameConfiguration(int rows, int cols, int mineCount)
{
    private int rows = rows;
    private int cols = cols;
    private int mineCount = mineCount;

    public int Rows
    {
        get
        {
            return this.rows;
        }

        set
        {
            this.rows = value;
        }
    }

    public int Cols
    {
        get
        {
            return this.cols;
        }

        set
        {
            this.cols = value;
        }
    }

    public int MineCount
    {
        get
        {
            return this.mineCount;
        }

        set
        {
            this.mineCount = value;
        }
    }

    public void UpdateConfig(GameConfiguration newConfiguration)
    {
        this.Cols = newConfiguration.Cols;
        this.Rows = newConfiguration.Rows;
        this.MineCount = newConfiguration.MineCount;
    }
}