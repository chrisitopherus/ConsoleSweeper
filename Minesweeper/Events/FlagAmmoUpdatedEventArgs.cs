using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Events;

public class FlagAmmoUpdatedEventArgs(int flagAmmo) : EventArgs
{
    public int NewFlagAmmo { get; } = flagAmmo;
}
