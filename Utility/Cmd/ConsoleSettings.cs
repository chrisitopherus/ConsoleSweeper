using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Cmd
{
    public class ConsoleSettings
    {
        public ConsoleSettings(
            ConsoleColor foregroundColor,
            ConsoleColor backgroundColor,
            int windowWidth,
            int windowHeight,
            int bufferWidth,
            int bufferHeight
            )
        {
            ForegroundColor = foregroundColor;
            BackroundColor = backgroundColor;
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            BufferWidth = bufferWidth;
            BufferHeight = bufferHeight;
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
                    Console.WindowWidth,
                    Console.WindowHeight,
                    Console.BufferWidth,
                    Console.BufferHeight
                );
        }

        public void Apply()
        {
            this.ApplyColors();
            Console.WindowWidth = this.WindowWidth;
            Console.WindowHeight = this.WindowHeight;
            Console.BufferWidth = this.BufferWidth;
            Console.BufferHeight = this.BufferHeight;
        }

        public void ApplyColors()
        {
            Console.ForegroundColor = this.ForegroundColor;
            Console.BackgroundColor = this.BackroundColor;
        }

        public void ApplyFgColor()
        {
            Console.ForegroundColor = this.ForegroundColor;
        }

        public void ApplyBgColor()
        {
            Console.BackgroundColor = this.BackroundColor;
        }
    }
}