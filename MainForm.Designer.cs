namespace RazerDeathadderFix
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            mouseEventPrinter = new System.Windows.Forms.ListBox();
            this.hookInvokerButton = new System.Windows.Forms.Button();
            this.clearMouseEventPrinterButton = new System.Windows.Forms.Button();
            this.countBypassedClicksLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mouseEventPrinter
            // 
            mouseEventPrinter.FormattingEnabled = true;
            mouseEventPrinter.Location = new System.Drawing.Point(12, 80);
            mouseEventPrinter.Name = "mouseEventPrinter";
            mouseEventPrinter.Size = new System.Drawing.Size(365, 316);
            mouseEventPrinter.TabIndex = 0;
            // 
            // hookInvokerButton
            // 
            this.hookInvokerButton.Location = new System.Drawing.Point(12, 12);
            this.hookInvokerButton.Name = "hookInvokerButton";
            this.hookInvokerButton.Size = new System.Drawing.Size(98, 23);
            this.hookInvokerButton.TabIndex = 1;
            this.hookInvokerButton.Text = "HOOK!";
            this.hookInvokerButton.UseVisualStyleBackColor = true;
            this.hookInvokerButton.Click += new System.EventHandler(this.hookInvokerButton_Click);
            // 
            // clearMouseEventPrinterButton
            // 
            this.clearMouseEventPrinterButton.Location = new System.Drawing.Point(133, 12);
            this.clearMouseEventPrinterButton.Name = "clearMouseEventPrinterButton";
            this.clearMouseEventPrinterButton.Size = new System.Drawing.Size(70, 23);
            this.clearMouseEventPrinterButton.TabIndex = 5;
            this.clearMouseEventPrinterButton.Text = "CLear";
            this.clearMouseEventPrinterButton.UseVisualStyleBackColor = true;
            this.clearMouseEventPrinterButton.Click += new System.EventHandler(this.clearMouseEventPrinterButton_Click);
            // 
            // countBypassedClicksLabel
            // 
            this.countBypassedClicksLabel.AutoSize = true;
            this.countBypassedClicksLabel.Location = new System.Drawing.Point(12, 54);
            this.countBypassedClicksLabel.Name = "countBypassedClicksLabel";
            this.countBypassedClicksLabel.Size = new System.Drawing.Size(35, 13);
            this.countBypassedClicksLabel.TabIndex = 6;
            this.countBypassedClicksLabel.Text = "Bypassed clicks: NaN";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 410);
            this.Controls.Add(this.countBypassedClicksLabel);
            this.Controls.Add(this.clearMouseEventPrinterButton);
            this.Controls.Add(this.hookInvokerButton);
            this.Controls.Add(mouseEventPrinter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button hookInvokerButton;
        private System.Windows.Forms.Button clearMouseEventPrinterButton;
        private System.Windows.Forms.Label countBypassedClicksLabel;
        private static System.Windows.Forms.ListBox mouseEventPrinter;
    }
}

