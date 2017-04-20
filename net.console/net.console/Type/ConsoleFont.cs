namespace net.console.Type
{
    public struct ConsoleFont
    {
        public ushort Index { get; }
        public int SizeX { get; }
        public int SizeY { get; }
        public bool Bold { get; }

        public ConsoleFont(ushort index, int sizeX = 8, int sizeY = 12, bool bold = false)
        {
            Index = index;
            SizeX = sizeX;
            SizeY = sizeY;
            Bold = bold;
        }
    }
}
