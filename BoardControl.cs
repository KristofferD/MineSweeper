using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Minesweeper
{
    public class BoardControl : Panel
    {
        private Board board;
        private CellControl[,] cellControls;

        public event EventHandler GameOver;

        public BoardControl(Board board)
        {
            this.board = board;
            cellControls = new CellControl[board.Rows, board.Columns];
            Dock = DockStyle.Fill;

            InitializeCells();
            SetCellNeighbors();
        }

        private void InitializeCells()
        {
            for (int row = 0; row < board.Rows; row++)
            {
                for (int col = 0; col < board.Columns; col++)
                {
                    var cell = board.Cells[row, col];
                    var cellControl = new CellControl
                    {
                        IsMine = cell.IsMine,
                        AdjacentMines = cell.AdjacentMines
                    };
                    cellControls[row, col] = cellControl;
                    cellControl.MouseDown += CellControl_MouseDown;
                    Controls.Add(cellControl, col, row);
                }
            }
        }

        private void SetCellNeighbors()
        {
            for (int row = 0; row < board.Rows; row++)
            {
                for (int col = 0; col < board.Columns; col++)
                {
                    var cell = board.Cells[row, col];
                    var cellControl = cellControls[row, col];

                    // Set the neighbor cells for this cell
                    var neighbors = new List<Cell>();
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (i == 0 && j == 0)
                            {
                                continue;
                            }

                            var neighborRow = row + i;
                            var neighborCol = col + j;

                            if (neighborRow >= 0 && neighborRow < board.Rows && neighborCol >= 0 && neighborCol < board.Columns)
                            {
                                neighbors.Add(board.Cells[neighborRow, neighborCol]);
                            }
                        }
                    }
                    cell.Neighbors = neighbors.ToArray();
                    cellControl.Cell = cell;
                }
            }
        }

        private List<Cell> GetNeighbors(Cell cell)
        {
            List<Cell> neighbors = new List<Cell>();

            int row = cell.Row;
            int col = cell.Column;

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < board.Rows && j >= 0 && j < board.Columns && (i != row || j != col))
                    {
                        neighbors.Add(board.Cells[i, j]);
                    }
                }
            }

            return neighbors;
        }

        private void CellControl_Revealed(object sender, EventArgs e)
        {
            var cellControl = (CellControl)sender;
            var cell = cellControl.Cell;

            if (cell.IsMine)
            {
                // Game Over!
                cellControl.BackColor = System.Drawing.Color.Red;
                cellControl.Image = Properties.Resources.Mine;
                GameOver?.Invoke(this, EventArgs.Empty);
            }
            else if (cell.AdjacentMines == 0)
            {
                // Recursively reveal neighboring cells
                cellControl.BackColor = System.Drawing.Color.LightGray;
                cellControl.RevealNeighbors();
            }
            else
            {
                cellControl.BackColor = System.Drawing.Color.LightGray;
                cellControl.Reveal();
            }
        }

        private void CellControl_Flagged(object sender, EventArgs e)
        {
            var cellControl = (CellControl)sender;
            var cell = cellControl.Cell;

            cell.IsFlagged = !cell.IsFlagged;

            if (cell.IsFlagged)
            {
                cellControl.Image = Properties.Resources.Flag;
            }
            else
            {
                cellControl.Image = Properties.Resources.CellCovered;
            }
        }

        private void RevealCell(int row, int col)
        {
            var cell = board.Cells[row, col];

            if (cell.IsRevealed)
            {
                return;
            }

            cell.IsRevealed = true;
            cell.Invalidate();

            if (cell.AdjacentMines == 0)
            {
                // Reveal all adjacent cells with no adjacent mines
                for (int i = Math.Max(row - 1, 0); i <= Math.Min(row + 1, board.Rows - 1); i++)
                {
                    for (int j = Math.Max(col - 1, 0); j <= Math.Min(col + 1, board.Columns - 1); j++)
                    {
                        if (i != row || j != col)
                        {
                            RevealCell(i, j);
                        }
                    }
                }
            }
        }


        private void Cell_MouseDown(object sender, MouseEventArgs e)
        {
            var cell = (CellControl)sender;

            if (e.Button == MouseButtons.Left)
            {
                // Left mouse button was clicked
                if (!cell.IsFlagged && !cell.IsRevealed)
                {
                    if (cell.IsMine)
                    {
                        // Game over - reveal all mines
                        RevealMines();
                        MessageBox.Show("Game over!");
                    }
                    else
                    {
                        // Reveal the cell and any adjacent cells with no adjacent mines
                        RevealCell(cell.Row, cell.Col);
                        CheckForWin();
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Right mouse button was clicked
                if (!cell.IsRevealed)
                {
                    cell.IsFlagged = !cell.IsFlagged;
                    cell.Invalidate();
                }
            }
        }

        private void CheckForWin()
        {
            bool allNonMinesRevealed = true;
            foreach (var cell in board.Cells)
            {
                if (!cell.IsMine && !cellControl[cell.Row, cell.Column].IsRevealed)
                {
                    allNonMinesRevealed = false;
                    break;
                }
            }
            if (allNonMinesRevealed)
            {
                MessageBox.Show("You win!");
            }
        }


        public void Reset()
        {
            board.Reset();

            foreach (var cellControl in cellControls)
            {
                cellControl.Reset();
            }
        }
    }
}