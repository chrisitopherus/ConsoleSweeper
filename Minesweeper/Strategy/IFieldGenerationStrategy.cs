using Minesweeper.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper.Strategy;

public interface IFieldGenerationStrategy
{
    GameCell[,] Generate();
}