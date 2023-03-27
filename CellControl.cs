using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public class CellControl : UserControl
    {
        private const int ImageSize = 25;

        private readonly Image _cellCovered = Properties.Resources.CellCovered;
        private readonly Image _cellFlag = Properties.Resources.CellFlag;
        private readonly Image _mineExploded = Properties.Resources.MineExploded;
        private readonly Image[] _cellNumbers = {
            Properties.Resources.Cell0,
            Properties.Resources.Cell1,
            Properties.Resources.Cell2,
            Properties.Resources.Cell3,
            Properties.Resources.Cell4,
            Properties.Resources.Cell5,
            Properties.Resources.Cell6,
            Properties.Resources.Cell7,
            Properties.Resources.Cell8,
        };

        public bool IsMine { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsRevealed { get; set; }
        public int AdjacentMines { get; set; }

        public event EventHandler LeftClicked;
        public event EventHandler RightClicked;

        public CellControl()
        {
            // Set some default properties for the control
            Size = new Size(ImageSize, ImageSize);
            Margin = new Padding(0);
            Padding = new Padding(0);
            MouseClick += CellControl_MouseClick;

            // Create a new PictureBox to represent the cell's cover image
            var coverImage = new PictureBox
            {
                Dock = DockStyle.Fill,
                Image = _cellCovered
            };
            Controls.Add(coverImage);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (IsRevealed)
            {
                // If the cell is a mine, show the exploded mine image
                if (IsMine)
                {
                    e.Graphics.DrawImage(_mineExploded, new Point(0, 0));
                }
                // Otherwise, show the number of adjacent mines or an empty cell
                else if (AdjacentMines > 0 && AdjacentMines < 9)
                {
                    e.Graphics.DrawImage(_cellNumbers[AdjacentMines], new Point(0, 0));
                }
                else
                {
                    e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(0, 0, ImageSize, ImageSize));
                }
            }
            else if (IsFlagged)
            {
                // If the cell is flagged, draw a flag image over the cover image
                e.Graphics.DrawImage(_cellFlag, new Point(0, 0));
            }
        }

        private void CellControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                OnLeftClicked();
            }
            else if (e.Button == MouseButtons.Right)
            {
                OnRightClicked();
            }
        }

        protected virtual void OnLeftClicked()
        {
            LeftClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnRightClicked()
        {
            RightClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
