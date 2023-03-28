using System;
using System.Drawing;
using System.Windows.Forms;

namespace MineSweeper
{
    public enum CellState
    {
        Covered,
        Flagged,
        Revealed,
        QuestionMark,
        Mine
    }
    public class Cell
    {
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public bool HasMine { get; set; }
        public int NumberOfSurroundingMines { get; set; }

        public int AdjacentMines(Cell[,] grid, int rows, int cols)
        {
            int adjacentMines = 0;

            for (int i = Math.Max(0, rows - 1); i <= Math.Min(rows + 1, grid.GetLength(0) - 1); i++)
            {
                for (int j = Math.Max(0, cols - 1); j <= Math.Min(cols + 1, grid.GetLength(1) - 1); j++)
                {
                    if (grid[i, j].HasMine)
                    {
                        adjacentMines++;
                    }
                }
            }

            return adjacentMines;
        }
    }

    private void Invalidate()
    {
        base.Invalidate();
    }
    public partial class CellControl : UserControl
    {
        private readonly Image _cellCovered = Image.FromFile("CellCovered.png");
        private readonly Image _cellFlag = Image.FromFile("CellFlag.png");
        private readonly Image _mineExploded = Image.FromFile("MineExploded.png");
        private readonly Image[] _cellNumbers = {
            Image.FromFile("Cell0.png"),
            Image.FromFile("Cell1.png"),
            Image.FromFile("Cell2.png"),
            Image.FromFile("Cell3.png"),
            Image.FromFile("Cell4.png"),
            Image.FromFile("Cell5.png"),
            Image.FromFile("Cell6.png"),
            Image.FromFile("Cell7.png"),
            Image.FromFile("Cell8.png"),
        };

        private Cell _cell;
        private bool _isLeftMouseDown;
        private bool _isRightMouseDown;

        public Cell Cell
        {
            get { return _cell; }
            set
            {
                if (_cell != value)
                {
                    _cell = value;
                    Invalidate();
                }
            }
        }


        public CellControl()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.BackColor = System.Drawing.Color.DarkGray;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CellControl";
            this.Size = new System.Drawing.Size(32, 32);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CellControl_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CellControl_MouseUp);
            this.ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_cell.IsFlagged && !_cell.IsRevealed)
            {
                e.Graphics.DrawImage(_cellFlag, 0, 0, Width, Height);
                return;
            }

            if (_cell.IsRevealed)
            {
                if (_cell.HasMine)
                {
                    e.Graphics.DrawImage(_mineExploded, 0, 0, Width, Height);
                    return;
                }

                int number = _cell.NumberOfSurroundingMines;
                if (number >= 0 && number < _cellNumbers.Length)
                {
                    e.Graphics.DrawImage(_cellNumbers[number], 0, 0, Width, Height);
                    return;
                }
            }

            e.Graphics.DrawImage(_cellCovered, 0, 0, Width, Height);
        }

        private void CellControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isLeftMouseDown = true;
            }
            else if (e.Button == MouseButtons.Right)
            {
                _isRightMouseDown = true;
            }
        }

        private void CellControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isLeftMouseDown = false;

                if (ClientRectangle.Contains(e.Location))
                {
                    OnLeftMouseClick();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                _isRightMouseDown = false;

                if (ClientRectangle.Contains(e.Location))
                {
                    OnRightMouseClick();
                }
            }
        }

        private void OnLeftMouseClick()
        {
            if (!_cell.IsFlagged && !_cell.IsRevealed)
            {
                CellRevealed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                CellFlagged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnRightMouseClick()
        {
            if (!_cell.IsRevealed)
            {
                _cell.IsFlagged = !_cell.IsFlagged;
                Invalidate();

                CellFlagged?.Invoke(this, EventArgs.Empty);
            }
        }


        public event EventHandler CellRevealed;
        public event EventHandler CellFlagged;
    }
}
