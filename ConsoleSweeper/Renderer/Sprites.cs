using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Renderer;

public static class Sprites
{
    // Cell Sprites
    public static char Mine => 'X';
    public static char Flag => '?';
    public static char Empty => ' ';
    public static char Unrevealed => '█';

    // Border Sprites
    public static char BorderTopLeftCorner => '╔';
    public static char BorderHorizontal => '═';

    public static char BorderVertical => '║';
    public static char BorderTopRightCorner => '╗';
    public static char BorderBottomLeftCorner => '╚';

    public static char BorderBottomRightCorner => '╝';
}
