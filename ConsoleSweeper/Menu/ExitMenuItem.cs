using ConsoleSweeper.Interfaces;
using ConsoleSweeper.Menu.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Menu;

public class ExitMenuItem : IMenuItem
{
    private readonly IStopable subject;
    public ExitMenuItem(IStopable subject)
    {
        this.subject = subject;
    }

    public string Label => "Exit";

    public bool IsActive => true;

    public void Execute()
    {
        this.subject.Stop();
    }
}
