using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Renderer.Util;

public class ConsoleLabelWIthValue<TValue> : ConsoleLabel where TValue : IConvertible
{
    private TValue? value;
    private int maxValueLength = 0;

    public ConsoleLabelWIthValue(ConsolePosition position, string text, TValue value, int maxValueLength)
        : base(position, text)
    {
        this.value = value;
        this.maxValueLength = maxValueLength;
    }

    public TValue? Value
    {
        get
        {
            return this.value;
        }

        set
        {
            this.value = value;
        }
    }

    public override void Render()
    {
        Console.SetCursorPosition(this.Position.X, this.Position.Y);
        this.RenderText();
        this.RenderValue();
    }

    public override void RenderUpdate()
    {
        if (this.value == null) throw new InvalidOperationException("Cant render a value update without a value.");
        Console.SetCursorPosition(this.Position.X + this.Text.Length, this.Position.Y);
        Console.Write(this.value.ToString()?.PadRight(this.maxValueLength));
    }

    private void RenderValue()
    {
        Console.Write(this.value);
    }
}
