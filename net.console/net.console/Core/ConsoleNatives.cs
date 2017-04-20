using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace net.console.Core
{
    internal static class ConsoleNatives
    {
        #region Native Constants

        public const int STD_OUT_HANDLE = -11;
        public const int STD_IN_HANDLE = -10;
        public const int STD_ERR_HANDLE = -12;

        #endregion

        #region Native Methods

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle GetStdHandle(int handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(SafeFileHandle handle);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern short GetNumberOfConsoleFonts();

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleFont(SafeFileHandle hOutput, ushort fontIndex);

        //[DllImport("Kernel32.dll", SetLastError = true)]
        //public static extern bool SetCurrentConsoleFontEx(SafeFileHandle hOutput, bool bMaximumWindow, FontInfoEx fontInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetCurrentConsoleFont(SafeFileHandle hOutput, bool hMaxWindow, out FontInfo fontInfo);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool WriteConsoleOutput(
            SafeFileHandle hConsoleOutput,
            CharInfo[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion);

        #endregion

        #region Native Structs

        /*[StructLayout(LayoutKind.Sequential)]
        public struct FontInfoEx
        {
            public ulong cbSize;
            public ushort nFont;
            public ConsoleCommon.Coord dwFontSize;
            public uint FontFamily;
            public uint FontWeight;
            public char[] FaceName;

            public FontInfoEx(ushort nFont, ConsoleCommon.Coord dwFontSize, uint fontWeight) : this()
            {
                this.nFont = nFont;
                this.dwFontSize = dwFontSize;
                FontWeight = fontWeight;
                FaceName = new char[0];
                cbSize = sizeof(ushort) + sizeof(short) * 2 + sizeof(uint) * 2 + 0;
            }
        }*/

        [StructLayout(LayoutKind.Sequential)]
        public struct FontInfo
        {
            public ushort nFont;
            public Coord dwFontSize;
        }

        [StructLayout(LayoutKind.Sequential, Size = sizeof(short) + sizeof(short))]
        public struct Coord
        {
            public short X, Y;

            public Coord(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left, Top, Right, Bottom;

            public SmallRect(short left, short top, short right, short bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }

        #endregion
    }
}