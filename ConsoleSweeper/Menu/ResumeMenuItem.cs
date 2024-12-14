using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleSweeper.Interfaces;
using ConsoleSweeper.Menu.Interfaces;
using Minesweeper;

namespace ConsoleSweeper.Menu;

public class ResumeMenuItem : IMenuItem
{
    private readonly IGameResumable subject;
    public ResumeMenuItem(IGameResumable subject)
    {
        this.subject = subject;
    }

    public string Label => "Resume";

    public bool IsActive => this.subject.Game.CurrentState != GameState.NotStarted;

    public void Execute()
    {
        this.subject.ResumeGame();
    }
}
