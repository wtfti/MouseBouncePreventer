namespace RazerDeathadderFix
{
    using System;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Diagnostics;

    public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    public partial class MainForm : Form
    {
        private const long DifferenceBetweenEachClick = 1500000;

        //Declare the hook handle as an int.
        private static int hHook = 0;

        private static readonly Stopwatch Stopwatch = new Stopwatch();

        private static long LastEllapsedTime = 0;

        private static long bypassedClicks = 0;

        //Declare the mouse hook constant.
        //For other hook types, you can obtain these values from Winuser.h in the Microsoft SDK.
        public const int WH_MOUSE_LL = 14;

        //This is the Import for the SetWindowsHookEx function.
        //Use this function to install a thread-specific hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hInstance, int threadId);

        //This is the Import for the UnhookWindowsHookEx function.
        //Call this function to uninstall the hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //This is the Import for the CallNextHookEx function.
        //Use this function to pass the hook information to the next hook procedure in chain.
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        public MainForm()
        {
            this.InitializeComponent();
        }

        public static IntPtr MouseHookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return (IntPtr) CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            
            MouseMessages mouseEvent = (MouseMessages) wParam;
            if (mouseEvent == MouseMessages.WM_LBUTTONDOWN)
            {
                long currentTime = Stopwatch.Elapsed.Ticks;
                if (LastEllapsedTime != 0)
                {
                    long timeDifference = currentTime - LastEllapsedTime;
                    if (timeDifference < DifferenceBetweenEachClick)
                    {
                        LastEllapsedTime = currentTime;
                        bypassedClicks++;
                        mouseEventPrinter.Items.Add($"{mouseEvent}  -  {timeDifference}  -  {DateTime.Now}");
                        return (IntPtr) 1;
                    }
                }
            
                LastEllapsedTime = currentTime;
            }
            
            return (IntPtr) CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        private void hookInvokerButton_Click(object sender, EventArgs e)
        {
            if (hHook == 0)
            {
                hHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookCallBack, (IntPtr)0, 0);

                if (hHook == 0)
                {
                    throw new Exception("SetWindowsHookEx Failed");
                }
                
                Stopwatch.Start();
                this.countBypassedClicksLabel.Text = $@"Bypassed clicks: {bypassedClicks}";
                this.hookInvokerButton.Text = @"UnHook!";
            }
            else
            {
                bool ret = UnhookWindowsHookEx(hHook);
                //If the UnhookWindowsHookEx function fails.
                if (ret == false)
                {
                    throw new Exception("UnhookWindowsHookEx Failed");
                }

                hHook = 0;
                mouseEventPrinter.Items.Add("Unhook successfully");
                this.hookInvokerButton.Text = @"HOOK!";
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (hHook != 0)
            {
                bool ret = UnhookWindowsHookEx(hHook);

                if (ret == false)
                {
                    throw new Exception("Unhook fails :(");
                }
            }
        }

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,

            WM_LBUTTONUP = 0x0202,

            WM_MOUSEMOVE = 0x0200,

            WM_MOUSEWHEEL = 0x020A,

            WM_RBUTTONDOWN = 0x0204,

            WM_RBUTTONUP = 0x0205

        }

        private void clearMouseEventPrinterButton_Click(object sender, EventArgs e)
        {
            mouseEventPrinter.Items.Clear();
        }
    }
}
