using Minesweeper;
using Minesweeper.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Utility.Cmd;

namespace ConsoleSweeper.Renderer;

public class ConsoleGameRenderer : IGameRenderer
{
    private Game game;

    private int offsetX = 0;

    private int offsetY = 0;

    private RenderCellVisitor renderCellVisitor = new RenderCellVisitor();

    private ConsoleSettings defaultSettings = ConsoleSettings.Capture();
    public ConsoleGameRenderer(Game game)
    {
        this.game = game;
        this.Setup();
    }

    public void DrawCellsUpdate(List<BoardPosition> updatedCellsPositions)
    {
        throw new NotImplementedException();
    }

    public void DrawCursorUpdate(BoardPosition previousPosition, BoardPosition newPosition)
    {
        Console.SetCursorPosition(previousPosition.Col + this.offsetX, previousPosition.Row + this.offsetY);
        Console.ForegroundColor = this.defaultSettings.ForegroundColor;
        Console.BackgroundColor = this.defaultSettings.BackroundColor;
        this.DrawCell(this.game.Board.GetCellAt(previousPosition));
        Console.SetCursorPosition(newPosition.Col + this.offsetX, newPosition.Row + this.offsetY);
        GameCell newCell = this.game.Board.GetCellAt(newPosition);
        Console.ForegroundColor = newCell.IsRevealed ? ConsoleColor.Black : ConsoleColor.Yellow;
        Console.BackgroundColor = ConsoleColor.Yellow;
        this.DrawCell(this.game.Board.GetCellAt(newPosition));
        Console.ForegroundColor = this.defaultSettings.ForegroundColor;
        Console.BackgroundColor = this.defaultSettings.BackroundColor;
    }

    public void DrawGame()
    {
        // Get the board and iterate through it
        GameCell[,] cells = this.game.Board.GameField;
        for (int row = 0; row < cells.GetLength(0); row++)
        {
            for (int col = 0; col < cells.GetLength(1); col++)
            {
                GameCell cell = cells[row, col];
                if (this.game.Cursor.CurrentPosition.Row == row && this.game.Cursor.CurrentPosition.Col == col)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = this.defaultSettings.ForegroundColor;
                    Console.BackgroundColor = this.defaultSettings.BackroundColor;
                }

                // Render the Sprite
                this.DrawCell(cell);
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Setup the console for the renderer.
    /// </summary>
    private void Setup()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;
    }

    private void DrawCell(GameCell cell)
    {
        // Render the Sprite
        cell.Accept(this.renderCellVisitor);
    }
}