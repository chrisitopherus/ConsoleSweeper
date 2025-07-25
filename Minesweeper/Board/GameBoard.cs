﻿using Minesweeper.Cells;
using Minesweeper.Strategy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Minesweeper.Board;

public class GameBoard
{
    private GameConfiguration config;
    private GameCell[,] gameField = new GameCell[0,0];
    private IFieldGenerationStrategy strategy;

    public GameBoard(GameConfiguration configuration)
    {
        this.config = configuration;
        this.strategy = new SimpleRandomGenerationStrategy(configuration);
    }

    public GameConfiguration Config
    {
        get
        {
            return this.config;
        }

        private set
        {
            this.config = value;
        }
    }

    public GameCell[,] GameField
    {
        get
        {
            return this.gameField;
        }

        private set
        {
            this.gameField = value;
        }
    }

    public IFieldGenerationStrategy GenerationStrategy
    {
        get
        {
            return this.strategy;
        }

        set
        {
            this.strategy = value;
        }
    }

    public void Generate()
    {
        this.gameField = this.strategy.Generate();
    }

    public GameCell GetCellAt(BoardPosition position)
    {
        if (!this.IsValidBoardPosition(position))
        {
            throw new ArgumentOutOfRangeException(nameof(position), "The position must be inside the board.");
        }

        return this.GameField[position.Row, position.Col];
    }

    public GameCell GetCellAt(int row, int col)
    {
        if (!this.IsValidBoardPosition(row, col))
        {
            throw new ArgumentException("The position must be inside the board.");
        }

        return this.GameField[row, col];
    }

    /// <summary>
    /// Tries to reveal the cell.
    /// </summary>
    /// <param name="cell">The cell to reveal.</param>
    /// <returns>Whether the cell was updated.</returns>
    public bool RevealCell(GameCell cell)
    {
        if (cell.IsRevealed) return false;

        cell.Reveal();
        return true;
    }

    /// <summary>
    /// Tries to reveal the cell.
    /// </summary>
    /// <param name="position">The position of the cell to reveal.</param>
    /// <returns>Whether the cell was updated.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Is raised if the position is not in range of the board.
    /// </exception>
    public bool RevealCell(BoardPosition position)
    {
        if (!this.IsValidBoardPosition(position))
        {
            throw new ArgumentOutOfRangeException(nameof(position), "The position must be inside the board.");
        }

        GameCell cell = this.GetCellAt(position);
        return this.RevealCell(cell);
    }

    /// <summary>
    /// Tries to toggle the marking of the cell.
    /// </summary>
    /// <param name="cell">The cell on which to toggle the mark.</param>
    /// <returns>Whether the cell was updated.</returns>
    public bool ToggleCellMark(GameCell cell)
    {
        bool cellMarkStateBefore = cell.IsMarked;
        cell.ToggleMark();
        return cellMarkStateBefore != cell.IsMarked;
    }

    /// <summary>
    /// Tries to toggle the marking of the cell.
    /// </summary>
    /// <param name="position">The position of the cell to toggle the mark.</param>
    /// <returns>Whether the cell was updated.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Is raised if the position is not in range of the board.
    /// </exception>
    public bool ToggleCellMark(BoardPosition position)
    {
        if (!this.IsValidBoardPosition(position))
        {
            throw new ArgumentOutOfRangeException(nameof(position), "The position must be inside the board.");
        }

        GameCell cell = this.GetCellAt(position);
        return this.ToggleCellMark(cell);
    }

    public bool IsValidBoardPosition(BoardPosition position)
    {
        return position.Row >= 0 && position.Row < this.config.Rows && position.Col >= 0 && position.Col < this.config.Cols;
    }

    public bool IsValidBoardPosition(int row, int col)
    {
        return row >= 0 && row < this.config.Rows && col >= 0 && col < this.config.Cols;
    }

    public IEnumerable<ICellInfo> GetNeighborsOfCell(ICellInfo cellInfo)
    {
        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int colOffset = -1; colOffset <= 1; colOffset++)
            {
                int neighborRow = cellInfo.Position.Row + rowOffset;
                int neighborCol = cellInfo.Position.Col + colOffset;

                // if its not a valid position (outside of the board) -> skip
                if (!this.IsValidBoardPosition(neighborRow, neighborCol)) continue;

                // Get the neighbor cell and create cell info
                BoardPosition neighborPosition = new BoardPosition(neighborRow, neighborCol);
                GameCell neighborCell = this.GetCellAt(neighborPosition);
                yield return new CellInfo(neighborCell, neighborPosition);
            }
        }
    }

    public IEnumerable<ICellInfo> GetFilteredCells(Predicate<ICellInfo> predicate)
    {
        for (int row = 0; row < this.Config.Rows; row++)
        {
            for (int col = 0; col < this.Config.Cols; col++)
            {
                // Get the neighbor cell and create cell info
                BoardPosition neighborPosition = new BoardPosition(row, col);
                GameCell cell = this.GetCellAt(neighborPosition);
                ICellInfo cellInfo = new CellInfo(cell, neighborPosition);
                if (predicate(cellInfo))
                {
                    yield return cellInfo;
                }
            }
        }
    }
}