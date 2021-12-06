using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Runtime.Serialization;
using System.Threading;

namespace SMT
{

    class Program
    {
        //Настрйоки для изменение парметров консоли
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal unsafe struct CONSOLE_FONT_INFO_EX
        {
            internal uint cbSize;
            internal uint nFont;
            internal COORD dwFontSize;
            internal int FontFamily;
            internal int FontWeight;
            internal fixed char FaceName[LF_FACESIZE];
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            internal short X;
            internal short Y;

            internal COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }
        private const int STD_OUTPUT_HANDLE = -11;
        private const int TMPF_TRUETYPE = 4;
        private const int LF_FACESIZE = 32;
        private static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(
            IntPtr consoleOutput,
            bool maximumWindow,
            ref CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int dwType);


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int SetConsoleFont(
            IntPtr hOut,
            uint dwFontNum
            );
        public static void SetConsoleFont(string fontName, short size)
        {
            unsafe
            {
                IntPtr hnd = GetStdHandle(STD_OUTPUT_HANDLE);
                if (hnd != INVALID_HANDLE_VALUE)
                {
                    CONSOLE_FONT_INFO_EX info = new CONSOLE_FONT_INFO_EX();
                    info.cbSize = (uint)Marshal.SizeOf(info);

                    // Set console font to Lucida Console.
                    CONSOLE_FONT_INFO_EX newInfo = new CONSOLE_FONT_INFO_EX();
                    newInfo.cbSize = (uint)Marshal.SizeOf(newInfo);
                    newInfo.FontFamily = TMPF_TRUETYPE;
                    IntPtr ptr = new IntPtr(newInfo.FaceName);
                    Marshal.Copy(fontName.ToCharArray(), 0, ptr, fontName.Length);

                    // Get some settings from current font.
                    newInfo.dwFontSize = new COORD(0, size);
                    newInfo.FontWeight = 700;
                    SetCurrentConsoleFontEx(hnd, false, ref newInfo);
                }
            }
        }
        [STAThread]
        static void Main(string[] args)
        {
            //Добавление параметров реестр
            RegistryKey currentUserKey = Registry.CurrentUser;
            RegistryKey ConsoleStyle = currentUserKey.CreateSubKey("Console");
            ConsoleStyle.SetValue("Font", "Gothic");
            ConsoleStyle.SetValue("Size", "20");
            ConsoleStyle.SetValue("Foreground", "Red");
            ConsoleStyle.SetValue("Background", "Gray");

            currentUserKey = Registry.CurrentUser;
            ConsoleStyle = currentUserKey.OpenSubKey("Console");


            //Вытягивание параметров
            string Font = ConsoleStyle.GetValue("Font").ToString();
            string Size = ConsoleStyle.GetValue("Size").ToString();
            short  size = Convert.ToInt16(Size);
            string Background = ConsoleStyle.GetValue("Background").ToString();
            string Foreground = ConsoleStyle.GetValue("Foreground").ToString();


            ConsoleColor ForegroundC = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Foreground, true);
            ConsoleColor BackgroundC = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Background, true);
            ConsoleStyle.Close();

            //Применение выбранных параметров
            Console.ForegroundColor = ForegroundC;
            Console.BackgroundColor = BackgroundC;
            SetConsoleFont(Font, size);


            Console.WriteLine("Something text");

            //Буфер обмена
            bool flag = true;
            //Работа с вытягиванием текста из буфера обмена
            while (flag)
            {
                if (Clipboard.ContainsText() == true)
                {
                    string someText = Clipboard.GetText();
                    Console.WriteLine(someText);
                    switch (someText)
                    {
                        case "1":
                            MessageBox.Show("Один", "Сообщение");
                            flag = false;
                            break;
                        case "2":
                            MessageBox.Show("Два", "Сообщение");
                            break;
                        case "3":
                            MessageBox.Show("Три", "Сообщение");
                            break;
                        case "4":
                            MessageBox.Show("Четыре", "Сообщение");
                            break;
                        case "5":
                            MessageBox.Show("Пять", "Сообщение");
                            break;
                        case "6":
                            MessageBox.Show("Шесть", "Сообщение");
                            break;
                        case "7":
                            MessageBox.Show("Семь", "Сообщение");
                            break;
                        case "8":
                            MessageBox.Show("Восемь", "Сообщение");
                            break;
                        case "9":
                            MessageBox.Show("Девять", "Сообщение");
                            break;

                        default:
                            MessageBox.Show(someText, "Сообщение");
                            break;
                    }
                    Thread.Sleep(1000);
                }
            }

            //Положить текст в буфер обмена

            //string smtText = "ddddd";
            //if (smtText != "")
            //    Clipboard.SetText(smtText);


            Console.ReadKey();
        }
    }
}
