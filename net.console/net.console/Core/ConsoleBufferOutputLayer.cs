using System;
using net.console.Core.Type;

namespace net.console.Core
{
    internal sealed class ConsoleBufferOutputLayer : ConsoleOutputLayer
    {
        public ConsoleBuffer Buffer { get; }

        private int _cursorLeft;
        private int _cursorTop;

        public ConsoleBufferOutputLayer(int width, int height)
        {
            Buffer = new ConsoleBuffer(width, height);
            CursorLeft = Math.Min(width, Console.CursorLeft);
            CursorTop = Math.Min(height, Console.CursorTop);
            ForegroundColor = Console.ForegroundColor;
            BackgroundColor = Console.BackgroundColor;
        }

        public override void Write(string value)
        {
            foreach (var c in value)
            {
                if (CursorLeft >= BufferWidth - 1 && CursorTop >= BufferHeight - 1)
                    return; //Out of view

                Buffer[CursorLeft, CursorTop] = new ConsoleElement(c, ForegroundColor, BackgroundColor);

                CursorLeft++;
            }
        }

        public override void WriteLine()
        {
            SetCursorPosition(0, CursorTop + 1);
        }

        public override void Clear()
        {
            Buffer.Clear();
        }

        public override int BufferWidth => Buffer.Width;
        public override int BufferHeight => Buffer.Height;

        public override int CursorLeft
        {
            get { return _cursorLeft; }
            set
            {
                _cursorLeft = value;
                while (_cursorLeft >= BufferWidth)
                {
                    _cursorLeft -= BufferWidth;
                    CursorTop++;
                }
            }
        }

        public override int CursorTop
        {
            get { return _cursorTop; }
            set
            {
                _cursorTop = value;
                if (_cursorTop >= BufferHeight)
                {
                    _cursorTop = BufferHeight - 1;
                }
            }
        }

        public override ConsoleColor ForegroundColor { get; set; }
        public override ConsoleColor BackgroundColor { get; set; }

        public override void ResetColor()
        {
            Console.ResetColor();
            ForegroundColor = Console.ForegroundColor;
            BackgroundColor = Console.BackgroundColor;
        }
    }
}