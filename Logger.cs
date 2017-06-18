namespace RazerDeathadderFix
{
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using Interfaces;

    public class Logger : ListBox, ILogger
    {
        public Logger(Point drawingPoints, Size size)
        {
            this.FormattingEnabled = true;
            this.Location = drawingPoints;
            this.Name = "mouseEventPrinter";
            this.Size = size;
            this.TabIndex = 0;
        }

        public void Log(string message)
        {
            this.Items.Add(message);
//            bypassedLButtonLabel.Text = $@"Bypassed clicks: {++BypassedLButtonClicks}";
//            allLButtonLabel.Text = $@"All LButton: {mouseEventPrinter.Items.Count}";
//
//            //after if
//            mouseEventPrinter.Items.Add($"{mouseEvent}  -   {currentTime}   -   {LastEllapsedTime}  -  {DateTime.Now}");
//            lButtonLabel.Text = $@"LButton: {++LButtonClicks}";
//            allLButtonLabel.Text = $@"All LButton: {mouseEventPrinter.Items.Count}";
        }

        private void ScrollToBottomListBox()
        {
            int visibleItems = this.ClientSize.Height / this.ItemHeight;
            this.TopIndex = Math.Max(this.Items.Count - visibleItems + 1, 0);
        }
    }
}
