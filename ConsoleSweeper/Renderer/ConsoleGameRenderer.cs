using ConsoleSweeper.Renderer.Util;
using Minesweeper;
using Minesweeper.Board;
using Minesweeper.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Utility.Cmd;

namespace ConsoleSweeper.Renderer;

public class ConsoleGameRenderer : IGameRenderer
{
    private readonly Game game;

    private int offsetX;

    private int offsetY;

    private RenderCellVisitor renderCellVisitor = new RenderCellVisitor();

    private ConsoleBorderRenderer borderRenderer;

    private ConsoleSettings defaultSettings = ConsoleSettings.Capture();

    private ConsoleSettings renderSettings;
    public ConsoleGameRenderer(int offsetX, int offsetY, Game game)
    {
        this.game = game;
        // default settings
        this.renderSettings = new ConsoleSettings(
            ConsoleColor.White,
            ConsoleColor.Black,
            50,
            30,
            50,
            30);
        this.offsetX = offsetX + 1;
        this.offsetY = offsetY + 1;
        this.borderRenderer = new ConsoleBorderRenderer(offsetX, offsetY);
        this.Setup();
    }

    public void RenderCellsUpdate(IEnumerable<ICellInfo> updatedCellInfos)
    {
        foreach(ICellInfo cellInfo in updatedCellInfos)
        {
            if (this.game.Cursor.CurrentPosition.IsEqual(cellInfo.Position))
            {
                this.ChangeColorsForCursor(cellInfo.Cell);
            }
            else
            {
                this.renderSettings.ApplyColors();
            }

            this.RerenderCell(cellInfo.Position);
        }
    }

    public void RenderCursorUpdate(BoardPosition previousPosition, BoardPosition newPosition)
    {
        // remove cursor at old position -> redraw the cell without cursor
        this.renderSettings.ApplyColors();

        // Console.ForegroundColor = ConsoleColor.Gray;
        // Console.BackgroundColor = ConsoleColor.Black;
        this.RerenderCell(previousPosition);

        // draw cursor at new position -> redraw the cell with cursor
        GameCell cellUnderneathCursor = this.game.Board.GetCellAt(newPosition);
        this.ChangeColorsForCursor(cellUnderneathCursor);
        this.RerenderCell(newPosition);

        // color reset
        this.renderSettings.ApplyColors();
    }

    public void RenderGame()
    {
        this.borderRenderer.Render(this.game.Config.Cols + 2, this.game.Config.Rows + 2);

        // Move to the offset position
        Console.SetCursorPosition(this.offsetX, this.offsetY);

        // Get the board and iterate through it
        GameCell[,] cells = this.game.Board.GameField;
        for (int row = 0; row < cells.GetLength(0); row++)
        {
            for (int col = 0; col < cells.GetLength(1); col++)
            {
                GameCell cell = cells[row, col];
                if (this.game.Cursor.CurrentPosition.Row == row && this.game.Cursor.CurrentPosition.Col == col)
                {
                    this.ChangeColorsForCursor(cell);
                }
                else
                {
                    // color reset
                    this.renderSettings.ApplyColors();
                }

                // Render the Sprite
                this.RenderCell(cell);
            }

            Console.WriteLine();

            // adapt to offset
            Console.CursorLeft += this.offsetX;
        }
    }

    /// <summary>
    /// Setup the console for the renderer.
    /// </summary>
    private void Setup()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;
        this.renderSettings.Apply();
    }

    /// <summary>
    /// Renders a single cell.
    /// </summary>
    /// <param name="cell">The cell that should be rendered.</param>
    private void RenderCell(GameCell cell)
    {
        // Render the Sprite
        cell.Accept(this.renderCellVisitor);
    }

    /// <summary>
    /// Renders an update for a cell. (Renders it again)
    /// </summary>
    /// <param name="position"></param>
    private void RerenderCell(BoardPosition position)
    {
        Console.SetCursorPosition(position.Col + this.offsetX, position.Row + this.offsetY);
        this.RenderCell(this.game.Board.GetCellAt(position));
    }

    /// <summary>
    /// Changes to console colors for the cursor depending on the cell.
    /// </summary>
    /// <param name="cell">The cell underneath the cursor.</param>
    private void ChangeColorsForCursor(GameCell cell)
    {
        Console.ForegroundColor = cell.IsRevealed || cell.IsMarked ? ConsoleColor.Black : ConsoleColor.Magenta;
        Console.BackgroundColor = ConsoleColor.Magenta;
    }
}