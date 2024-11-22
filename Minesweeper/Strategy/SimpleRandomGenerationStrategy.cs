using Minesweeper.Board;
using Minesweeper.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper.Strategy;

public class SimpleRandomGenerationStrategy : IFieldGenerationStrategy
{
    private readonly Random randomInstance = new();
    private GameConfiguration config;

    public SimpleRandomGenerationStrategy(GameConfiguration configuration)
    {
        this.config = configuration;
    }
    public GameCell[,] Generate()
    {
        GameCell[,] gameField = new GameCell[this.config.Rows, this.config.Cols];
        HashSet<(int, int)> minePositions = this.GetMinePositions();
        gameField = this.FillField(gameField, minePositions);

        return gameField;
    }

    private HashSet<(int, int)> GetMinePositions()
    {
        HashSet<(int, int)> minePositions = [];

        while (minePositions.Count < this.config.MineCount)
        {
            int row = this.randomInstance.Next(0, this.config.Rows);
            int col = this.randomInstance.Next(0, this.config.Cols);
            minePositions.Add((row, col));
        }

        return minePositions;
    }

    private GameCell[,] FillField(GameCell[,] gameField, HashSet<(int, int)> minePositions)
    {
        for (int row = 0; row < gameField.GetLength(0); row++)
        {
            for (int col = 0; col < gameField.GetLength(1); col++)
            {
                if (minePositions.Contains((row, col)))
                {
                    // It is a mine
                    gameField[row, col] = new Mine();
                }
                else
                {
                    // It is a tile
                    uint nearbyMineCount = this.DetermineNearbyMineCount(gameField, new BoardPosition(row, col), minePositions);
                    gameField[row, col] = new Tile(nearbyMineCount);
                }
            }
        }

        return gameField;
    }

    private uint DetermineNearbyMineCount(GameCell[,] gameField, BoardPosition position, HashSet<(int, int)> minePositions)
    {
        uint mineCount = 0;
        int[] offsets = [-1, 0, 1];
        for (int i = 0; i < offsets.Length; i++)
        {
            for (int j = 0; j < offsets.Length; j++)
            {
                // same cell -> skip
                if (offsets[i] == 0 && offsets[j] == 0) continue;
                int neighborRow = position.Row + offsets[i];
                int neighborCol = position.Col + offsets[j];

                // is outside of field -> skip
                if (!this.IsValidFieldPosition(neighborRow, neighborCol)) continue;

                if (minePositions.Contains((neighborRow, neighborCol)))
                {
                    mineCount++;
                }
            }
        }

        return mineCount;
    }

    private bool IsValidFieldPosition(int row, int col)
    {
        return row >= 0 && row < this.config.Rows && col >= 0 && col < this.config.Cols;
    }
}