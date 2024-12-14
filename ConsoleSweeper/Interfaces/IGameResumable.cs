using Minesweeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Interfaces;

public interface IGameResumable
{
    Game Game { get; }
    void ResumeGame();
}
