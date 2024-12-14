using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper;

public enum GameState
{
    /// <summary>
    /// Initial State - If the game was created but never started.
    /// </summary>
    NotStarted,
    /// <summary>
    /// Menu State - If the game is paused and the menu is active.
    /// </summary>
    Menu,
    /// <summary>
    /// Running State - If the game is running and being played.
    /// </summary>
    Running,
    /// <summary>
    /// Win State - If the game was a win.
    /// </summary>
    Win,
    /// <summary>
    /// Loss State - If the game was a loss.
    /// </summary>
    Loss
}
