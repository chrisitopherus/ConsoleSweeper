using ConsoleSweeper.Menu.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.Menu.Util;

public struct MenuItemInfo(IMenuItem item, bool isSelected)
{
    public readonly IMenuItem Item => item;

    public readonly bool IsSelected => isSelected;

}
