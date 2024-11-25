using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Renderer.Util;

public class ConsoleLabel
{
    private string text;
    private ConsolePosition position;

    public ConsoleLabel(ConsolePosition position, string text)
    {
        this.position = position;
        this.text = text;
    }

    public string Text
    {
        get
        {
            return this.text;
        }

        set
        {
            this.text = value;
        }
    }

    public ConsolePosition Position
    {
        get
        {
            return this.position;
        }

        set
        {
            this.position = value;
        }
    }

    public virtual void Render()
    {
        Console.SetCursorPosition(this.position.X, this.position.Y);
        this.RenderText();
    }

    protected void RenderText()
    {
        Console.Write(this.text);
    }

    public virtual void RenderUpdate()
    {
        this.Render();
    }
}