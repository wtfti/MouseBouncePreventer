﻿namespace RazerDeathadderFix
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using Interfaces;

    public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    public partial class MainForm : Form
    {
        private static readonly IMousePreventer mousePreventer = null;

        //Declare the mouse hook constant.
        public const int WH_MOUSE_LL = 14;

        //Declare the hook handle as an int.
        private static int hHook = 0;

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

            bool prevent = mousePreventer.IsMouseBounce(wParam);

            if (prevent)
            {
                return (IntPtr) 1;
            }

            return (IntPtr)CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        private void hookInvokerButton_Click(object sender, EventArgs e)
        {
            var logger = new Logger(new Point(12, 170), new Size(483, 316));
            this.Controls.Add(logger);
//            if (hHook == 0)
//            {
//                hHook = SetWindowsHookEx(WH_MOUSE_LL, Proc, (IntPtr)0, 0);
//
//                if (hHook == 0)
//                {
//                    throw new Exception("SetWindowsHookEx Failed");
//                }
//
//                this.hookInvokerButton.Text = @"UnHook!";
//            }
//            else
//            {
//                bool ret = UnhookWindowsHookEx(hHook);
//                //If the UnhookWindowsHookEx function fails.
//                if (ret == false)
//                {
//                    throw new Exception("UnhookWindowsHookEx Failed");
//                }
//
//                hHook = 0;
//                mouseEventPrinter.Items.Add("Unhook successfully");
//                this.hookInvokerButton.Text = @"HOOK!";
//            }
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
        }
    }
}
