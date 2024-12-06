using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.KeyMapping;

public enum GameCommand
{
    ToggleMark,
    Reveal,
    MoveCursorLeft,
    MoveCursorRight,
    MoveCursorUp,
    MoveCursorDown,
    Restart,
    Menu,
    Select
}
