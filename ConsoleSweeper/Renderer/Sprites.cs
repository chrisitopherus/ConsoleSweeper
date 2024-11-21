using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Renderer;

public static class Sprites
{
    public static char Mine => 'X';
    public static char Flag => '?';
    public static char Empty => ' ';
    public static char Unrevealed => '█';
}
