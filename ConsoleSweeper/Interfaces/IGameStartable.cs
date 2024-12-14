using Minesweeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Interfaces;

public interface IGameStartable
{
    void StartGame(GameConfiguration configuration);
}
