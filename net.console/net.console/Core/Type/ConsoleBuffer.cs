using System;

namespace net.console.Core.Type
{
    internal sealed class ConsoleBuffer
    {
        public int Width { get; }
        public int Height { get; }

        private ConsoleElement?[][] _elements;

        public ConsoleBuffer(int width, int height)
        {
            Width = width;
            Height = height;
            RebuildBuffer();
        }

        public void Clear()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    this[x, y] = null;
                }
            }
        }

        public ConsoleElement? this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Width)
                    throw new ArgumentException("x must be between 0 and " + Width);
                if (y < 0 || y >= Height)
                    throw new ArgumentException("y must be between 0 and " + Height);

                return _elements[x][y];
            }
            set
            {
                if (x < 0 || x >= Width)
                    throw new ArgumentException("x must be between 0 and " + Width);
                if (y < 0 || y >= Height)
                    throw new ArgumentException("y must be between 0 and " + Height);

                _elements[x][y] = value;
            }
        }

        protected void RebuildBuffer()
        {
            _elements = new ConsoleElement?[Width][];
            for (var i = 0; i < Width; i++)
            {
                _elements[i] = new ConsoleElement?[Height];
            }
        }
    }
}
