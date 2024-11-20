using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSweeper.KeyMapping;

public class InputHandler
{
    public GameCommand? GetCommand(ConsoleKey key)
    {
        if (KeyMapping.KeyToCommandMap.TryGetValue(key, out GameCommand command))
        {
            return command;
        }

        return null;
    }
}
