using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Console
{
    public class ConsoleSettings
    {
        public ConsoleSettings(
            ConsoleColor foregroundColor,
            ConsoleColor backgroundColor,
            int bufferWidth,
            int bufferHeight,
            int windowWidth,
            int windowHeight)
        {
            ForegroundColor = foregroundColor;
            BackroundColor = backgroundColor;
            BufferWidth = bufferWidth;
            BufferHeight = bufferHeight;
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
        }

        public ConsoleColor ForegroundColor
        {
            get;
            set;
        }

        public ConsoleColor BackroundColor
        {
            get;
            set;
        }

        public int BufferWidth
        {
            get;
            set;
        }

        public int BufferHeight
        {
            get;
            set;
        }

        public int WindowWidth
        {
            get;
            set;
        }

        public int WindowHeight
        {
            get;
            set;
        }

        public static ConsoleSettings Capture()
        {
            return new ConsoleSettings(
                    Console.ForegroundColor,
                    Console.BackgroundColor,
                    Console.BufferWidth,
                    Console.BufferHeight,
                    Console.WindowWidth,
                    Console.WindowHeight
                );
        }

        public void Apply()
        {
            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackroundColor;
            Console.BufferWidth = BufferWidth;
            Console.BufferHeight = BufferHeight;
            Console.WindowWidth = WindowWidth;
            Console.WindowHeight = WindowHeight;
        }
    }
}