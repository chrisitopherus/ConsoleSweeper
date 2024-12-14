using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Menu.Interfaces;

public interface IMenuItem
{
    void Execute();

    string Label { get; }
    bool IsActive { get; }

}
