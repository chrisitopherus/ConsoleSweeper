using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.KeyMapping;

public static class KeyMapping
{
    private static readonly Dictionary<ConsoleKey, GameCommand> keyToCommandMap = new()
    {
        // Cursor Control
        { ConsoleKey.LeftArrow, GameCommand.MoveCursorLeft },
        { ConsoleKey.A, GameCommand.MoveCursorLeft },
        { ConsoleKey.RightArrow, GameCommand.MoveCursorRight },
        { ConsoleKey.D, GameCommand.MoveCursorRight },
        { ConsoleKey.UpArrow, GameCommand.MoveCursorUp },
        { ConsoleKey.W, GameCommand.MoveCursorUp },
        { ConsoleKey.DownArrow, GameCommand.MoveCursorDown },
        { ConsoleKey.S, GameCommand.MoveCursorDown },

        // Interaction Control
        { ConsoleKey.Spacebar, GameCommand.Reveal },
        { ConsoleKey.F, GameCommand.ToggleMark },
        { ConsoleKey.M, GameCommand.ToggleMark },

        // Special Interaction Control
        { ConsoleKey.R, GameCommand.Restart },
        { ConsoleKey.Escape, GameCommand.Menu }
    };

    public static Dictionary<ConsoleKey, GameCommand> KeyToCommandMap
    {
        get
        {
            return keyToCommandMap;
        }
    }
}
