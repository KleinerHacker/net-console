using System;

namespace net.console.Core
{
    internal abstract class ConsoleOutputLayer
    {
        public abstract int CursorLeft { get; set; }
        public abstract int CursorTop { get; set; }

        public void SetCursorPosition(int left, int top)
        {
            CursorLeft = left;
            CursorTop = top;
        }

        public abstract void Write(string value);

        public void WriteLine(string value)
        {
            Write(value);
            WriteLine();
        }

        public abstract void WriteLine();

        public abstract void Clear();

        public abstract ConsoleColor ForegroundColor { get; set; }
        public abstract ConsoleColor BackgroundColor { get; set; }
        public abstract void ResetColor();

        public abstract int BufferWidth { get; }
        public abstract int BufferHeight { get; }
    }
}
