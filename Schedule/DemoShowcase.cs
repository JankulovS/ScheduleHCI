using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Schedule
{
    public class DemoCanceledException : Exception
    {

    }

    class DemoShowcase
    {
        // window offset
        static int windowX;
        static int windowY;

        // voodoo black magic stuff
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        public static Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }


        public static void LeftMouseClick()
        {
            //Point startPoint = GetMousePosition();

            //int xpos = (int)startPoint.X + windowX;
            //int ypos = (int)startPoint.Y + windowY;

            //SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void RightMouseClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        public static void MouseMove(int xpos, int ypos)
        {
            xpos += windowX;
            ypos += windowY;


            Point startPoint = GetMousePosition();
            double startX = startPoint.X;
            double startY = startPoint.Y;

            // lerp to position
            for (int i = 0; i < 40; i++)
            {
                double t = i / 40.0;

                double newX = (1 - t) * startX + t * xpos;
                double newY = (1 - t) * startY + t * ypos;

                SetCursorPos((int)newX, (int)newY);
                Wait(25);
                Console.WriteLine(newX + " " + newY);


            }
            //SetCursorPos((int)newX, (int)newY);
        }

        public static void MouseHoldDown()
        {

            //Point startPoint = GetMousePosition();

            //int xpos = (int)startPoint.X + windowX;
            //int ypos = (int)startPoint.Y + windowY;

            //SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        }

        public static void MouseRelease()
        {
            //Point startPoint = GetMousePosition();

            //int xpos = (int)startPoint.X + windowX;
            //int ypos = (int)startPoint.Y + windowY;

            //SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
            if (IsKeyPressed())
            {
                throw new DemoCanceledException();
            }
        }

        public static bool IsKeyPressed()
        {
            if (KeysDown().Any())
            {
                return true;
            }
            return false;
        }

        public static IEnumerable<Key> KeysDown()
        {
            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                if (key != Key.None && Keyboard.IsKeyDown(key))
                    yield return key;
            }
        }

        public static void StartDemo()
        {

            windowX = (int)MainWindow._mainWindow.Left;
            windowY = (int)MainWindow._mainWindow.Top;
            Thread thread = new Thread(Demo);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        [STAThread]
        private static void Demo()
        {
            try
            {
                // open menu and select classrooms
                MouseMove(50, 60);
                LeftMouseClick();
                Wait(250);
                MouseMove(50, 145);
                LeftMouseClick();
                Wait(250);

                // select classroom
                MouseMove(50, 110);
                LeftMouseClick();
                Wait(250);

                // open menu and select subjects
                MouseMove(65, 60);
                LeftMouseClick();
                Wait(250);
                MouseMove(50, 80);
                LeftMouseClick();
                Wait(250);

                // drag over one subject
                MouseMove(50, 110);
                MouseHoldDown();
                Wait(250);
                MouseMove(350, 200);
                MouseRelease();
                Wait(250);

                // drag over second subject
                MouseMove(50, 135);
                MouseHoldDown();
                Wait(250);
                MouseMove(350, 160);
                MouseRelease();
                Wait(250);

                // move first to other place
                MouseMove(350, 200);
                Wait(125);
                MouseHoldDown();
                Wait(250);
                MouseMove(350, 310);
                Wait(150);
                MouseRelease();
                Wait(1250);

                // change day
                MouseMove(350, 50);
                LeftMouseClick();
                Wait(250);

                // drag over one
                MouseMove(50, 155);
                MouseHoldDown();
                Wait(250);
                MouseMove(350, 180);
                MouseRelease();
                Wait(250);

                // drag over one
                MouseMove(50, 155);
                MouseHoldDown();
                Wait(250);
                MouseMove(370, 200);
                MouseRelease();
                Wait(250);

                return;
            }
            catch (DemoCanceledException)
            {
                MouseRelease();
                return;
            }
        }
    }
}
