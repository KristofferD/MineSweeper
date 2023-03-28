using MineSweeper;
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
        public int Row { get; set; }
        public int Col { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsMine { get; set; }
        public int NumberOfSurroundingMines { get; set; }

   
            public Cell(int row, int col)
            {
                Row = row;
                Col = col;
            }
        
        public void AdjacentMines(Cell[,] grid, int rows, int cols)
        {

            for (int i = Math.Max(0, rows - 1); i <= Math.Min(rows + 1, grid.GetLength(0) - 1); i++)
            {
                for (int j = Math.Max(0, cols - 1); j <= Math.Min(cols + 1, grid.GetLength(1) - 1); j++)
                {
                    if (grid[i, j].IsMine)
                    {
                        NumberOfSurroundingMines++;
                    }
                }
            }

            
        }
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
        readonly int row;
        readonly int col;

        public int Row
        {
            get { return row; }
        }

        public int Col
        {
            get { return col; }
        }


        public event EventHandler LeftClick;
        public event EventHandler RightClick;

        public class CellInfo
        {
            public int Row { get; }
            public int Col { get; }
            public bool IsMine { get; set; }
            public bool IsRevealed { get; set; }
            public bool IsFlagged { get; set; }

            public CellInfo(int row, int col)
            {
                Row = row;
                Col = col;
            }
        }



        private new void Invalidate()
        {
            base.Invalidate();
        }


        public CellControl()
        {
            InitializeComponent();
        }

        public CellControl(int row, int col)
        {
            this.row = row;
            this.col = col;

            _cell = new Cell(row, col);
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
                if (_cell.IsMine)
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
