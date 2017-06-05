namespace RazerDeathadderFix
{
    using System;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Diagnostics;

    public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    public partial class MainForm : Form
    {
        private const long MaxAllowedMilisecondsForStopwatch = 30000;

        private const long DifferenceTicksBetweenEachClick = 1000000;

        //Declare the mouse hook constant.
        public const int WH_MOUSE_LL = 14;

        //Declare the hook handle as an int.
        private static int hHook = 0;

        private static readonly Stopwatch Stopwatch = new Stopwatch();

        private static long LastEllapsedTime = 0;

        private static long BypassedLButtonClicks = 0;

        private static long LButtonClicks = 0;

        // @CallbackOnCollectedDelegate without reference, garbage collector will throw an exception
        private static readonly LowLevelMouseProc Proc = MouseHookCallBack;

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
                return (IntPtr)CallNextHookEx(hHook, nCode, wParam, lParam);
            }

            MouseMessages mouseEvent = (MouseMessages)wParam;
            if (mouseEvent == MouseMessages.WM_LBUTTONDOWN)
            {
                long currentTime = Stopwatch.Elapsed.Ticks;
                if (LastEllapsedTime > 0)
                {
                    long timeDifference = currentTime - LastEllapsedTime;
                    if (timeDifference < DifferenceTicksBetweenEachClick)
                    {
                        LastEllapsedTime = currentTime;
                        mouseEventPrinter.Items.Add($"BYPASSED => {mouseEvent}  -  {timeDifference}  -  {DateTime.Now}");
                        bypassedLButtonLabel.Text = $@"Bypassed clicks: {++BypassedLButtonClicks}";
                        allLButtonLabel.Text = $@"All LButton: {mouseEventPrinter.Items.Count}";

                        if (scrollBottomCheckBox.Checked)
                        {
                            ScrollToBottomListBox();
                        }

                        return (IntPtr)1;
                    }
                }

                mouseEventPrinter.Items.Add($"{mouseEvent}  -   {currentTime}   -   {LastEllapsedTime}  -  {DateTime.Now}");
                lButtonLabel.Text = $@"LButton: {++LButtonClicks}";
                allLButtonLabel.Text = $@"All LButton: {mouseEventPrinter.Items.Count}";
                LastEllapsedTime = currentTime;

                if (scrollBottomCheckBox.Checked)
                {
                    ScrollToBottomListBox();
                }
            }

            return (IntPtr)CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        private void hookInvokerButton_Click(object sender, EventArgs e)
        {
            if (hHook == 0)
            {
                hHook = SetWindowsHookEx(WH_MOUSE_LL, Proc, (IntPtr)0, 0);

                if (hHook == 0)
                {
                    throw new Exception("SetWindowsHookEx Failed");
                }

                Stopwatch.Start();
                this.resetStopWatchTimer.Start();
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
                Stopwatch.Stop();
                this.resetStopWatchTimer.Stop();
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

        private void clearMouseEventPrinterButton_Click(object sender, EventArgs e)
        {
            bypassedLButtonLabel.Text = @"Bypassed LButton: NaN";
            allLButtonLabel.Text = @"All LButton: NaN";
            lButtonLabel.Text = @"LButton: NaN";
            LastEllapsedTime = 0;
            BypassedLButtonClicks = 0;
            LButtonClicks = 0;
            mouseEventPrinter.Items.Clear();
        }

        private void resetStopWatchTimer_Tick(object sender, EventArgs e)
        {
            if (Stopwatch.ElapsedMilliseconds > MaxAllowedMilisecondsForStopwatch)
            {
                LastEllapsedTime = 0;
                Stopwatch.Restart();
            }
        }

        private static void ScrollToBottomListBox()
        {
            int visibleItems = mouseEventPrinter.ClientSize.Height / mouseEventPrinter.ItemHeight;
            mouseEventPrinter.TopIndex = Math.Max(mouseEventPrinter.Items.Count - visibleItems + 1, 0);
        }
    }
}
