using ConsoleSweeper.Renderer;
using ConsoleSweeper.Renderer.Util;
using Minesweeper;
using Minesweeper.Board;
using Minesweeper.Events;
using Minesweeper.Strategy;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleSweeper;

public class Application
{
    private readonly Game game;
    private IGameController controller;
    private IGameRenderer renderer;
    public Application()
    {
        this.game = new Game(new GameConfiguration(16, 30, 50));
        this.controller = new ConsoleGameController(this.game);
        this.renderer = new ConsoleGameRenderer(new ConsolePosition(4, 4), this.game);
        this.Setup();
    }
    public void Run()
    {
        Console.Clear();
        Console.OutputEncoding = Encoding.UTF8;
        this.game.Start();
        this.controller.Start();
        while (true)
        {
            Thread.Sleep(10);
        }
    }

    private void Setup()
    {
        this.game.GameStarted += this.OnGameStartedHandler;
        this.game.GameWon += this.OnGameWonHandler;
        this.game.GameLoss += this.OnGameLossHandler;
        this.game.CursorMoved += this.OnCursorMovedHandler;
        this.game.CellsUpdated += this.OnCellsUpdatedHandler;
        this.game.FlagAmmoUpdated += this.OnFlagAmmoUpdatedHandler;
    }

    private void OnGameStartedHandler(object? sender, GameStartedEventArgs e)
    {
        this.renderer.RenderGame();
    }

    private void OnGameWonHandler(object? sender, GameWonEventArgs e)
    {
        this.renderer.RenderWin();
    }

    private void OnGameLossHandler(object? sender, GameLossEventArgs e)
    {
        this.renderer.RenderLoss();
    }

    private void OnCursorMovedHandler(object? sender, CursorMovedEventArgs e)
    {
        this.renderer.RenderCursorUpdate(e.PreviousPosition, e.NewPosition);
    }

    private void OnCellsUpdatedHandler(object? sender, CellsUpdatedEventArgs e)
    {
        this.renderer.RenderCellsUpdate(e.UpdatedCells);
    }

    private void OnFlagAmmoUpdatedHandler(object? sender, FlagAmmoUpdatedEventArgs e)
    {
        this.renderer.RenderFlagAmmoUpdate(e.NewFlagAmmo);
    }
}