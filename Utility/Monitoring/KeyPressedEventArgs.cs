using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Monitoring
{
    public class KeyPressedEventArgs(ConsoleKey key)
    {
        public ConsoleKey Key => key;
    }
}