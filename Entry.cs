using System;
using System.Windows.Forms;

namespace RazerDeathadderFix
{
    static class Entry
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
