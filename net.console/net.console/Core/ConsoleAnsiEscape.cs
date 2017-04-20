using System;
using System.Text.RegularExpressions;
using net.console.Util.Extension;

namespace net.console.Core
{
    internal static class ConsoleAnsiEscape
    {
        private static int curX = 0;
        private static int curY = 0;

        public static void WriteWithAnsiEscapeCode(string value, ConsoleOutputLayer outputLayer)
        {
            var match = Regex.Match(value, "\x1B\\x5B(((?<value>[0-9]+);)*(?<value>[0-9]+))?(?<type>.)");
            if (match.Success)
            {
                outputLayer.Write(value.Substring(0, match.Index));

                while (match != null && match.Success)
                {
                    var valueGroup = match.Groups["value"];
                    var typeGroup = match.Groups["type"];

                    switch (typeGroup.Value)
                    {
                        case "H":
                        case "f":
                            JumpCursor(valueGroup.Captures, outputLayer);
                            break;
                        case "A":
                            JumpCursor(valueGroup.Captures, 1, outputLayer);
                            break;
                        case "B":
                            JumpCursor(valueGroup.Captures, 3, outputLayer);
                            break;
                        case "C":
                            JumpCursor(valueGroup.Captures, 2, outputLayer);
                            break;
                        case "D":
                            JumpCursor(valueGroup.Captures, 0, outputLayer);
                            break;
                        case "s":
                            curX = outputLayer.CursorLeft;
                            curY = outputLayer.CursorTop;
                            break;
                        case "u":
                            outputLayer.CursorLeft = curX;
                            outputLayer.CursorTop = curY;
                            break;
                        case "J":
                            outputLayer.Clear();
                            outputLayer.SetCursorPosition(0, 0);
                            break;
                        case "K":
                            ClearLine(valueGroup.Captures, outputLayer);
                            break;
                        case "m":
                            UpdateGraphicMode(valueGroup.Captures, outputLayer);
                            break;
                    }

                    var startIndex = match.Index + match.Length;
                    match = match.NextMatch();
                    var endIndex = match.Success ? match.Index : value.Length;

                    outputLayer.Write(value.Substring(startIndex, endIndex - startIndex));
                }
            }
            else
            {
                outputLayer.Write(value);
            }
        }

        private static void ClearLine(CaptureCollection captures, ConsoleOutputLayer outputLayer)
        {
            if (captures.Count <= 0 || String.IsNullOrEmpty(captures[0].Value) || captures[0].Value == "0")
            {
                var x = outputLayer.CursorLeft;
                outputLayer.Write(" ".Repeat(outputLayer.BufferWidth - x));
                outputLayer.CursorLeft = x;
            }
            else if (captures[0].Value == "1")
            {
                var x = outputLayer.CursorLeft;
                outputLayer.CursorLeft = 0;
                outputLayer.Write(" ".Repeat(x));
            }
            else if (captures[0].Value == "2")
            {
                var x = outputLayer.CursorLeft;
                outputLayer.CursorLeft = 0;
                Console.Write(" ".Repeat(outputLayer.BufferWidth));
                outputLayer.CursorLeft = x;
            }
        }

        private static void JumpCursor(CaptureCollection captures, ConsoleOutputLayer outputLayer)
        {
            int x = 0, y = 0;
            if (captures.Count > 0 && !String.IsNullOrEmpty(captures[0].Value))
            {
                Int32.TryParse(captures[0].Value, out x);
            }
            if (captures.Count > 1 && !String.IsNullOrEmpty(captures[1].Value))
            {
                Int32.TryParse(captures[1].Value, out y);
            }

            outputLayer.SetCursorPosition(x, y);
        }

        private static void JumpCursor(CaptureCollection captures, int direction, ConsoleOutputLayer outputLayer)
        {
            int change = 1;
            if (captures.Count > 0 && !String.IsNullOrEmpty(captures[0].Value))
            {
                Int32.TryParse(captures[0].Value, out change);
            }

            switch (direction)
            {
                case 0:
                    if (outputLayer.CursorLeft - change >= 0)
                    {
                        outputLayer.CursorLeft -= change;
                    }
                    break;
                case 1:
                    if (outputLayer.CursorTop - change >= 0)
                    {
                        outputLayer.CursorTop -= change;
                    }
                    break;
                case 2:
                    if (outputLayer.CursorLeft + change < outputLayer.BufferWidth)
                    {
                        outputLayer.CursorLeft += change;
                    }
                    break;
                case 3:
                    if (outputLayer.CursorTop + change < outputLayer.BufferHeight)
                    {
                        outputLayer.CursorTop += change;
                    }
                    break;
            }
        }

        private static void UpdateGraphicMode(CaptureCollection captures, ConsoleOutputLayer outputLayer)
        {
            foreach (Capture capture in captures)
            {
                if (capture.Value == "0")
                {
                    outputLayer.ResetColor();
                }
                else if (capture.Value == "1")
                {
                    outputLayer.ForegroundColor = (ConsoleColor)((int)outputLayer.ForegroundColor ^ 0x08);
                }
                else if (capture.Value.StartsWith("3"))
                {
                    byte value;
                    if (!Byte.TryParse(capture.Value.Substring(1), out value))
                        return;
                    outputLayer.ForegroundColor = (ConsoleColor)(ConvertColor(value) | 0x08);
                }
                else if (capture.Value.StartsWith("4"))
                {
                    byte value;
                    if (!Byte.TryParse(capture.Value.Substring(1), out value))
                        return;
                    outputLayer.BackgroundColor = (ConsoleColor)ConvertColor(value);
                }
            }
        }

        private static byte ConvertColor(byte value)
        {
            byte result = 0x00;

            if ((value & 0x01) == 0x01)
            {
                //Change first byte
                result |= 0x04;
            }
            if ((value & 0x04) == 0x04)
            {
                //Chnage last byte
                result |= 0x01;
            }

            //Set middle byte
            result |= (byte)(value & 0x02);

            return result;
        }
    }
}
