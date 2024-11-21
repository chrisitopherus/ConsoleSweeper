using ConsoleSweeper.Renderer;
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
        this.game = new Game(new GameConfiguration(16, 30, 55));
        this.controller = new ConsoleGameController(this.game);
        this.renderer = new ConsoleGameRenderer(this.game);
        this.Setup();
    }
    public void Run()
    {
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
    }

    private void OnGameStartedHandler(object? sender, GameStartedEventArgs e)
    {
        this.renderer.DrawGame();
    }

    private void OnGameWonHandler(object? sender, GameWonEventArgs e)
    {
        Console.WriteLine("Game won.");
    }

    private void OnGameLossHandler(object? sender, GameLossEventArgs e)
    {
        Console.WriteLine("Game lost.");
    }

    private void OnCursorMovedHandler(object? sender, CursorMovedEventArgs e)
    {
        this.renderer.DrawCursorUpdate(e.PreviousPosition, e.NewPosition);
    }

    private void OnCellsUpdatedHandler(object? sender, CellsUpdatedEventArgs e)
    {
        Console.WriteLine("Cells updated.");
        Console.Clear();
        this.renderer.DrawGame();
    }
}