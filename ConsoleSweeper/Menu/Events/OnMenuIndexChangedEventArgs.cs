using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Menu.Events;

public class OnMenuIndexChangedEventArgs : EventArgs
{
    private readonly int prevIndex;
    private readonly int newIndex;
    public OnMenuIndexChangedEventArgs(int prevIndex, int newIndex)
    {
        this.prevIndex = prevIndex;
        this.newIndex = newIndex;
    }

    public int PrevIndex => this.prevIndex;
    public int NewIndex => this.newIndex;
}
