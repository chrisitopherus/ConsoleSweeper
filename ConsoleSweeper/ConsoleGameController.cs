using ConsoleSweeper.KeyMapping;
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
    private KeyboardWatcher keyboardWatcher;
    private InputHandler inputHandler;

    public ConsoleGameController(Game game)
    {
        this.game = game;
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
                this.game.MoveCursor(CursorMoveDirection.Up);
                break;
            case GameCommand.MoveCursorDown:
                this.game.MoveCursor(CursorMoveDirection.Down);
                break;
            case GameCommand.ToggleMark:
                this.game.TryToggleMark();
                break;
            case GameCommand.Reveal:
                this.game.TryReveal();
                break;
        }
    }
}
