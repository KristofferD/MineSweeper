using System;
using System.Drawing;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class MyUserControl : UserControl
    {
        public MyUserControl(int designTime = 0)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.BackColor = System.Drawing.Color.White;
            this.Name = "MyUserControl";
            this.ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw a simple rectangle
            using (var pen = new Pen(Color.Black))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            }
        }
    }
}
