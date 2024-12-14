using ConsoleSweeper.Interfaces;
using ConsoleSweeper.Menu;
using ConsoleSweeper.Menu.Events;
using ConsoleSweeper.Renderer;
using ConsoleSweeper.Renderer.Util;
using Minesweeper;
using Minesweeper.Board;
using Minesweeper.Events;
using Minesweeper.Strategy;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility.Cmd;

namespace ConsoleSweeper;

public class Application : IStopable, IGameStartable, IGameResumable
{
    private bool isRunning = false;
    private readonly Game game;
    private readonly GameMenu gameMenu;
    private ConsoleSettings renderSettings;
    private IGameController controller;
    private IGameRenderer renderer;
    public Application()
    {
        this.renderSettings = new ConsoleSettings(
            ConsoleColor.White,
            ConsoleColor.Black,
            50,
            30,
            50,
            30);
        this.game = new Game(new GameConfiguration(16, 30, 50));
        this.gameMenu = new GameMenu();
        this.controller = new ConsoleGameController(this.game, this.gameMenu);
        this.renderer = new ConsoleGameRenderer(new ConsolePosition(4, 4), this.game, this.gameMenu, this.renderSettings);
        this.MenuInit();
        this.Setup();
    }

    public Game Game => this.game;
    public int Run()
    {
        this.isRunning = true;
        Console.Clear();
        Console.OutputEncoding = Encoding.UTF8;

        // show starting menu
        this.gameMenu.Open();

        this.controller.Start();
        while (this.isRunning)
        {
            Thread.Sleep(10);
        }

        return 0;
    }

    public void Stop()
    {
        this.isRunning = false;
    }

    public void StartGame(GameConfiguration configuration)
    {
        this.game.UpdateConfiguration(configuration);
        this.gameMenu.Close();
        this.game.Start();
    }

    public void ResumeGame()
    {
        this.game.CloseMenuState();
        this.gameMenu.Close();
    }

    private void MenuInit()
    {
        this.gameMenu
            .Add(new ResumeMenuItem(this))
            .Add(new NewGameMenuItem(this, this.renderSettings))
            .Add(new ExitMenuItem(this));
    }

    private void Setup()
    {
        // game events
        this.game.GameStarted += this.OnGameStartedHandler;
        this.game.GameWon += this.OnGameWonHandler;
        this.game.GameLoss += this.OnGameLossHandler;
        this.game.CursorMoved += this.OnCursorMovedHandler;
        this.game.CellsUpdated += this.OnCellsUpdatedHandler;
        this.game.FlagAmmoUpdated += this.OnFlagAmmoUpdatedHandler;

        // menu events
        this.gameMenu.OnMenuOpened += this.OnMenuOpenedHandler;
        this.gameMenu.OnMenuClosed += this.OnMenuClosedHandler;
        this.gameMenu.OnMenuIndexChanged += this.OnMenuIndexChanged;
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

    private void OnMenuOpenedHandler(object? sender, OnMenuOpenedEventArgs e)
    {
        this.renderer.RenderMenu();
    }

    private void OnMenuClosedHandler(object? sender, OnMenuClosedEventArgs e)
    {
        this.renderer.UnrenderMenu();
        if (this.game.CurrentState != GameState.NotStarted)
        {
            this.renderer.RenderGame();
        }
    }

    private void OnMenuIndexChanged(object? sender, OnMenuIndexChangedEventArgs e)
    {
        this.renderer.RenderMenuUpdate(e.PrevIndex, e.NewIndex);
    }
}