using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using net.console.Core;
using net.console.Core.Type;
using net.console.Type;

namespace net.commons
{
    public static class ConsoleEx
    {
        private static ConsoleOutputLayer _outputLayer = new ConsoleDefaultOutputLayer();

        #region Output

        public static void WriteLine(string line, ConsoleColor? foreground = null, ConsoleColor? background = null)
        {
            Write(line, foreground, background);
            Console.WriteLine();
        }

        public static void Write(string line, ConsoleColor? foreground = null, ConsoleColor? background = null)
        {
            if (foreground.HasValue)
            {
                Console.ForegroundColor = foreground.Value;
            }
            if (background.HasValue)
            {
                Console.BackgroundColor = background.Value;
            }

            ConsoleAnsiEscape.WriteWithAnsiEscapeCode(line, _outputLayer);
        }

        public static void Clear()
        {
            _outputLayer.Clear();
        }

        public static ConsoleColor ForegroundColor
        {
            get { return _outputLayer.ForegroundColor; }
            set { _outputLayer.ForegroundColor = value; }
        }

        public static ConsoleColor BackgroundColor
        {
            get { return _outputLayer.BackgroundColor; }
            set { _outputLayer.BackgroundColor = value; }
        }

        public static void ResetColor()
        {
            _outputLayer.ResetColor();
        }

        #endregion

        #region Input

        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        public static int Read()
        {
            return Console.Read();
        }

        public static ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public static bool CapsLock => Console.CapsLock;

        public static bool KeyAvailable => Console.KeyAvailable;

        public static bool NumberLock => Console.NumberLock;

        public static bool TreatControlCAsInput => Console.TreatControlCAsInput;

        #endregion

        #region Font

        private static ConsoleFont _font;

        public static int BasicFontCount { get; } = ConsoleNatives.GetNumberOfConsoleFonts();

        public static ushort BasicFont
        {
            get
            {
                ConsoleNatives.FontInfo info;
                if (!ConsoleNatives.GetCurrentConsoleFont(ConsoleNatives.GetStdHandle(ConsoleNatives.STD_OUT_HANDLE), false, out info))
                    throw new InvalidOperationException("Unable to get console font");

                return info.nFont;
            }
            set { ConsoleNatives.SetConsoleFont(ConsoleNatives.GetStdHandle(ConsoleNatives.STD_OUT_HANDLE), value); }
        }

        /*public static ConsoleFont Font
        {
            get { return _font; }
            set
            {
                _font = value;
                ConsoleFonts.SetFont(value.Index, value.SizeX, value.SizeY, value.Bold);
            }
        }*/

        #endregion

        #region Buffer

        public static int BufferWidth
        {
            get { return _outputLayer.BufferWidth; }
            set
            {
                if (IsBufferMode)
                    throw new InvalidOperationException("Buffer Size not editable if buffer mode is enabled");

                Console.BufferWidth = value;
            }
        }

        public static int BufferHeight
        {
            get { return _outputLayer.BufferHeight; }
            set
            {
                if (IsBufferMode)
                    throw new InvalidOperationException("Buffer Size not editable if buffer mode is enabled");

                Console.BufferHeight = value;
            }
        }

        public static void SetBufferSize(int width, int height)
        {
            if (IsBufferMode)
                throw new InvalidOperationException("Buffer Size not editable if buffer mode is enabled");

            Console.SetBufferSize(width, height);
        }

        public static void BeginBufferMode()
        {
            BeginBufferMode(Console.BufferWidth, Console.BufferHeight);
        }

        public static void BeginBufferMode(int bufferWidth, int bufferHeight)
        {
            _outputLayer = new ConsoleBufferOutputLayer(bufferWidth, bufferHeight);
        }

        public static void EndBufferMode()
        {
            var layer = _outputLayer as ConsoleBufferOutputLayer;
            if (layer == null)
                throw new InvalidOperationException("Buffer Mode is not running");

            WriteBuffer(layer.Buffer);

            //Do back
            Console.SetCursorPosition(layer.CursorLeft, layer.CursorTop);
            Console.ForegroundColor = layer.ForegroundColor;
            Console.BackgroundColor = layer.BackgroundColor;

            _outputLayer = new ConsoleDefaultOutputLayer();
        }

        public static bool IsBufferMode => _outputLayer is ConsoleBufferOutputLayer;

        private static void WriteBuffer(ConsoleBuffer buffer)
        {
            var buf = new ConsoleNatives.CharInfo[buffer.Width * buffer.Height];
            var rect = new ConsoleNatives.SmallRect(0, 0, (short) buffer.Width, (short) buffer.Height);

            for (var y = 0; y < buffer.Height; y++)
            {
                for (var x = 0; x < buffer.Width; x++)
                {
                    if (!buffer[x, y].HasValue)
                        continue;

                    buf[x + y * buffer.Width].Char.UnicodeChar = buffer[x, y].Value.Value;
                    buf[x + y * buffer.Width].Attributes = (short) ((int) buffer[x, y].Value.ForegroundColor | ((int) buffer[x, y].Value.BackgroundColor << 4));
                }
            }

            ConsoleNatives.WriteConsoleOutput(ConsoleNatives.GetStdHandle(ConsoleNatives.STD_OUT_HANDLE), buf,
                new ConsoleNatives.Coord((short) buffer.Width, (short) buffer.Height), new ConsoleNatives.Coord(0, 0), ref rect);
        }

        #endregion

        #region Window

        public static int WindowLeft
        {
            get { return Console.WindowLeft; }
            set { Console.WindowLeft = value; }
        }

        public static int WindowTop
        {
            get { return Console.WindowTop; }
            set { Console.WindowTop = value; }
        }

        public static int WindowWidth
        {
            get { return Console.WindowWidth; }
            set { Console.WindowWidth = value; }
        }

        public static int WindowHeight
        {
            get { return Console.WindowHeight; }
            set { Console.WindowHeight = value; }
        }

        public static int LargestWindowWidth => Console.LargestWindowWidth;

        public static int LargestWindowHeight => Console.LargestWindowHeight;

        public static void SetWindowPosition(int left, int top)
        {
            Console.SetWindowPosition(left, top);
        }

        public static void SetWindowSize(int width, int height)
        {
            Console.SetWindowSize(width, height);
        }

        #endregion

        #region Cursor

        public static int CursorSize
        {
            get { return Console.CursorSize; }
            set { Console.CursorSize = value; }
        }

        public static bool CursorVisible
        {
            get { return Console.CursorVisible; }
            set { Console.CursorVisible = value; }
        }

        public static int CursorLeft
        {
            get { return _outputLayer.CursorLeft; }
            set { _outputLayer.CursorLeft = value; }
        }

        public static int CursorTop
        {
            get { return _outputLayer.CursorTop; }
            set { _outputLayer.CursorTop = value; }
        }

        public static void SetCursorPosition(int left, int top)
        {
            _outputLayer.SetCursorPosition(left, top);
        }

        #endregion

        public static void Beep()
        {
            Console.Beep();
        }

        public static void Beep(int frequence, int duration)
        {
            Console.Beep(frequence, duration);
        }
    }
}