using System;

namespace net.console.Core
{
    internal sealed class ConsoleDefaultOutputLayer : ConsoleOutputLayer
    {
        public override void Write(string value)
        {
            Console.Write(value);
        }

        public override void WriteLine()
        {
            Console.WriteLine();
        }

        public override void Clear()
        {
            Console.Clear();
        }

        public override int BufferWidth => Console.BufferWidth;
        public override int BufferHeight => Console.BufferHeight;

        public override int CursorLeft
        {
            get { return Console.CursorLeft; }
            set { Console.CursorLeft = value; }
        }

        public override int CursorTop
        {
            get { return Console.CursorTop; }
            set { Console.CursorTop = value; }
        }

        public override ConsoleColor ForegroundColor
        {
            get { return Console.ForegroundColor; }
            set { Console.ForegroundColor = value; }
        }

        public override ConsoleColor BackgroundColor
        {
            get { return Console.BackgroundColor; }
            set { Console.BackgroundColor = value; }
        }

        public override void ResetColor()
        {
            Console.ResetColor();
        }
    }
}