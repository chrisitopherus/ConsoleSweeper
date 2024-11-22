using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Renderer.Util;

public class ConsoleBorderRenderer
{
    private ConsolePosition origin;
    StringBuilder sb = new StringBuilder();

    public ConsoleBorderRenderer(int startX, int startY)
    {
        this.origin = new ConsolePosition(startX, startY);
    }

    public void Render(int width, int height)
    {
        // move to origin
        Console.SetCursorPosition(origin.X, origin.Y);
        Console.WriteLine(this.CreateTopBorder(width));
        for (int i = 0; i < height - 2; i++)
        {
            // left border
            Console.CursorLeft = origin.X;
            Console.Write(Sprites.BorderVertical);

            // right border
            Console.CursorLeft = origin.X + width - 1;
            Console.Write(Sprites.BorderVertical);

            Console.WriteLine();
        }

        Console.CursorLeft = origin.X;
        Console.Write(this.CreateBottomBorder(width));
    }

    private string CreateTopBorder(int width)
    {
        sb.Append(Sprites.BorderTopLeftCorner);
        sb.Append(string.Concat(Enumerable.Repeat(Sprites.BorderHorizontal, width - 2)));
        sb.Append(Sprites.BorderTopRightCorner);

        string topBorder = sb.ToString();
        sb.Clear();
        return topBorder;
    }

    private string CreateBottomBorder(int width)
    {
        sb.Append(Sprites.BorderBottomLeftCorner);
        sb.Append(string.Concat(Enumerable.Repeat(Sprites.BorderHorizontal, width - 2)));
        sb.Append(Sprites.BorderBottomRightCorner);

        string bottomBorder = sb.ToString();
        sb.Clear();
        return bottomBorder;
    }
}
