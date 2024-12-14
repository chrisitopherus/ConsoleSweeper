using ConsoleSweeper.Menu;
using ConsoleSweeper.Renderer.Border;
using ConsoleSweeper.Renderer.Menu;
using ConsoleSweeper.Renderer.Util;
using Minesweeper;
using Minesweeper.Board;
using Minesweeper.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Utility.Cmd;

namespace ConsoleSweeper.Renderer;

public class ConsoleGameRenderer : IGameRenderer
{
    private readonly Game game;
    private readonly GameMenu gameMenu;

    private ConsolePosition origin;
    private ConsolePosition borderOrigin;
    private ConsoleLabelWIthValue<int> flagAmmoLabel;
    private ConsoleLabel cursorControlsLabel;
    private ConsoleLabel interactionControlsLabel;
    private ConsoleLabel restartLabel;

    private ConsoleLabel[] labels;

    private RenderCellVisitor renderCellVisitor = new RenderCellVisitor();

    private ConsoleBorderRenderer borderRenderer;
    private GameMenuRenderer gameMenuRenderer;

    private ConsoleSettings defaultSettings = ConsoleSettings.Capture();

    private ConsoleSettings renderSettings;
    public ConsoleGameRenderer(ConsolePosition gameOrigin, Game game, GameMenu gameMenu, ConsoleSettings renderSettings)
    {
        this.game = game;
        this.gameMenu = gameMenu;

        // Render settings
        this.renderSettings = renderSettings;

        // Renderer
        this.origin = new ConsolePosition(gameOrigin.X, gameOrigin.Y);
        this.borderOrigin = new ConsolePosition(gameOrigin.X - 1, gameOrigin.Y - 1);
        this.borderRenderer = new ConsoleBorderRenderer(this.borderOrigin);
        this.gameMenuRenderer = new GameMenuRenderer(this.gameMenu, origin, this.renderSettings);

        // Labels - MUST BE BENEATH RENDERER INIT
        this.flagAmmoLabel = new ConsoleLabelWIthValue<int>(
            new ConsolePosition(this.borderOrigin.X + 1, this.borderOrigin.Y - 1),
            "Flags: ",
            this.game.FlagAmmo,
            this.game.Config.MineCount.ToString().Length);

        this.cursorControlsLabel = new ConsoleLabel(
            new ConsolePosition(this.borderOrigin.X, this.borderOrigin.Y + this.game.Config.Rows + 2),
            "Cursor: W,A,S,D or ↑,←,↓,→");

        this.interactionControlsLabel = new ConsoleLabel(
            new ConsolePosition(this.borderOrigin.X, this.borderOrigin.Y + this.game.Config.Rows + 3),
            "Mark/Flag: F | Reveal: Space");

        this.restartLabel = new ConsoleLabel(
            new ConsolePosition(this.borderOrigin.X, this.borderOrigin.Y + this.game.Config.Rows + 4),
            "Restart after W/L: R");

        this.labels = [this.flagAmmoLabel, this.cursorControlsLabel, this.interactionControlsLabel, this.restartLabel];

        // Setup
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
        // render game depending on state:
        switch (this.game.CurrentState)
        {
            case GameState.Running:
                this.RenderRunningGame();
                break;
            case GameState.Win:
                this.RenderWin();
                this.RenderGameFieldWithLabels();
                break;
            case GameState.Loss:
                this.RenderLoss();
                this.RenderGameFieldWithLabels();
                break;
            default:
                this.RenderRunningGame();
                break;
        }
    }

    public void RenderFlagAmmoUpdate(int flagAmmo)
    {
        this.renderSettings.ApplyColors();
        int maxLength = this.game.Config.MineCount.ToString().Length;
        this.flagAmmoLabel.Value = flagAmmo;
        this.flagAmmoLabel.RenderUpdate();
    }

    /// <summary>
    /// Rerenders the border to display a loss.
    /// </summary>
    public void RenderLoss()
    {
        this.renderSettings.ApplyColors();
        Console.ForegroundColor = ConsoleColor.Red;
        this.RenderBorder();
        this.renderSettings.ApplyColors();
    }

    /// <summary>
    /// Rerenders the border to display a win.
    /// </summary>
    public void RenderWin()
    {
        this.renderSettings.ApplyColors();
        Console.ForegroundColor = ConsoleColor.Green;
        this.RenderBorder();
        this.renderSettings.ApplyColors();
    }

    public void RenderMenu()
    {
        this.gameMenuRenderer.Render();
    }

    public void UnrenderMenu()
    {
        this.gameMenuRenderer.Unrender();
    }

    public void RenderMenuUpdate(int prevIndex, int newIndex)
    {
        this.gameMenuRenderer.RenderMenuUpdate(prevIndex, newIndex);
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

    private void RenderGameFieldWithLabels()
    {
        // Move to the offset position
        Console.SetCursorPosition(this.origin.X, this.origin.Y);

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
            Console.CursorLeft += this.origin.X;
        }

        // Render labels
        this.RenderLabels();
    }

    private void RenderRunningGame()
    {
        // color reset
        this.renderSettings.ApplyColors();
        this.RenderBorder();

        this.RenderGameFieldWithLabels();
        
    }

    private void RenderBorder()
    {
        this.borderRenderer.Render(this.game.Config.Cols + 2, this.game.Config.Rows + 2);
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
        Console.SetCursorPosition(position.Col + this.origin.X, position.Row + this.origin.Y);
        this.RenderCell(this.game.Board.GetCellAt(position));
    }

    private void RenderLabels()
    {
        this.renderSettings.ApplyColors();
        foreach (ConsoleLabel label in this.labels)
        {
            label.Render();
        }
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