using ConsoleSweeper.KeyMapping;
using ConsoleSweeper.Menu;
using Minesweeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Monitoring;

namespace ConsoleSweeper;

public class ConsoleGameController : IGameController
{
    private Game game;
    private GameMenu menu;
    private KeyboardWatcher keyboardWatcher;
    private InputHandler inputHandler;

    public ConsoleGameController(Game game, GameMenu menu)
    {
        this.game = game;
        this.menu = menu;
        this.keyboardWatcher = new KeyboardWatcher();
        this.inputHandler = new InputHandler();
    }

    public void Start()
    {
        this.keyboardWatcher.KeyPressed += this.OnKeyboardWatcherKeyPressedHandler;
        this.keyboardWatcher.Start();
    }

    public void Stop()
    {
        this.keyboardWatcher.KeyPressed -= this.OnKeyboardWatcherKeyPressedHandler;
        this.keyboardWatcher.Stop();

    }

    private void OnKeyboardWatcherKeyPressedHandler(object? sender, KeyPressedEventArgs e)
    {
        GameCommand? command = this.inputHandler.GetCommand(e.Key);
        if (command == null) return;

        switch (command)
        {
            case GameCommand.MoveCursorLeft:
                this.game.MoveCursor(CursorMoveDirection.Left);
                break;
            case GameCommand.MoveCursorRight:
                this.game.MoveCursor(CursorMoveDirection.Right);
                break;
            case GameCommand.MoveCursorUp:
                if (this.game.CurrentState == GameState.Menu)
                {
                    this.menu.PrevItem();
                }
                else
                {
                    this.game.MoveCursor(CursorMoveDirection.Up);
                }
                break;
            case GameCommand.MoveCursorDown:
                if (this.game.CurrentState == GameState.Menu)
                {
                    this.menu.NextItem();
                }
                else
                {
                    this.game.MoveCursor(CursorMoveDirection.Down);
                }
                break;
            case GameCommand.ToggleMark:
                this.game.TryToggleMark();
                break;
            case GameCommand.Reveal:
                this.game.TryReveal();
                break;
            case GameCommand.Restart:
                this.game.Restart();
                break;
            case GameCommand.Menu:
                // If game has not even started -> escape should do nothing
                if (this.game.CurrentState == GameState.NotStarted)
                {
                    return;
                }

                if (this.menu.IsOpen)
                {
                    this.menu.Close();
                }
                else
                {
                    this.menu.Open();
                }
                break;
            case GameCommand.Select:
                this.menu.ExecuteCurrentItem();
                break;
            default:
                throw new Exception($"Unhandled Command: {nameof(command)}");
        }
    }
}
