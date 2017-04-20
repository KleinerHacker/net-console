using System;

namespace net.console.Core.Type
{
    internal struct ConsoleElement
    {
        public char Value { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public ConsoleElement(char value, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Value = value;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
    }
}
