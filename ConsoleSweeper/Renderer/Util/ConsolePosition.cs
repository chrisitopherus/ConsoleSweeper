using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Renderer.Util;

public struct ConsolePosition(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
}
