using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Menu.Events;

public class OnMenuIndexChangedEventArgs : EventArgs
{
    private readonly int prevIndex;
    private readonly int nextIndex;
    public OnMenuIndexChangedEventArgs(int prevIndex, int newIndex)
    {
        this.prevIndex = prevIndex;
        this.nextIndex = newIndex;
    }

    public int PrevIndex => this.prevIndex;
    public int NextIndex => this.nextIndex;
}
