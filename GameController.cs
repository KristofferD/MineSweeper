using Minesweeper;
using System;

namespace MineSweeper
{
    public static class CellStateExtensions
    {
        public static bool IsRevealed(this CellState state)
        {
            return state >= CellState.Revealed;
        }

        public static bool IsMine(this CellState state)
        {
            return state == CellState.Mine;
        }

        public static bool IsFlagged(this CellState state)
        {
            return state == CellState.Flagged;
        }
    }

    public class GameController
    {
        private readonly int _boardWidth;
        private readonly int _boardHeight;
        private readonly int _numMines;
        private CellState[,] _board;
        private bool _gameOver;

        public event EventHandler<EventArgs> GameOver;



        public int FlagsRemaining => _numMines - CountFlags();

        public GameController(int boardWidth, int boardHeight, int numMines)
        {
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;
            _numMines = numMines;

            // Initialize the board with empty cells
            _board = new CellState[boardWidth, boardHeight];
            for (int x = 0; x < boardWidth; x++)
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    _board[x, y] = new CellState();
                }
            }

            // Add mines to the board
            var random = new Random();
            int numPlacedMines = 0;
            while (numPlacedMines < numMines)
            {
                int x = random.Next(boardWidth);
                int y = random.Next(boardHeight);
                if (!_board[x, y].IsMine())
                {
                    _board[x, y] = CellState.Mine;
                    numPlacedMines++;
                }
            }
        }

        public CellState GetCellState(int x, int y)
        {
            return _board[x, y];
        }

        public void RevealCell(int x, int y)
        {
            var cell = _board[x, y];

            if (cell.IsFlagged() || cell.IsRevealed())
            {
                // Cell is already flagged or revealed, do nothing
                return;
            }

            _board[x, y] = CellState.Revealed;

            if (_board[x, y].IsMine())
            {
                // Game over, the player has clicked on a mine
                _gameOver = true;
                OnGameOver();
                return;
            }

            if (_board[x, y].AdjacentMines == 0)
            {
                // Recursively reveal all adjacent cells with no adjacent mines
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int nx = x + dx;
                        int ny = y + dy;
                        if (nx >= 0 && nx < _boardWidth && ny >= 0 && ny < _boardHeight)
                        {
                            RevealCell(nx, ny);
                        }
                    }
                }
            }
        }


        public void FlagCell(int x, int y)
        {
            var cell = _board[x, y];

            if (cell.IsFlagged || cell.IsRevealed)
            {
                // Cell is already revealed or flagged, do nothing
                return;
            }

            // Use the IsFlagged property or field directly
            cell.IsFlagged = true;
        }
    }

        public bool IsGameOver()
        {
            if (_gameOver)
            {
                return true;
            }

            // Check if all non-mine cells have been revealed or flagged
            for (int x = 0; x < _boardWidth; x++)
            {
                for (int y = 0; y < _boardHeight; y++)
                {
                    var cell = _board[x, y];
                    if (!cell.IsMine && !cell.IsRevealed && !cell.IsFlagged)
                    {
                        return false;
                    }
                }
            }

            // If we get here, all non-mine cells have been revealed or flagged and the game is over
            _gameOver = true;
            OnGameOver();
            return true;
        }


        private int CountFlags()
        {
            int count = 0;
            for (int x = 0; x < _boardWidth; x++)
            {
                for (int y = 0; y < _boardHeight; y++)
                {
                    if (_board[x, y].IsFlagged)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private void OnGameOver()
        {
            GameOver?.Invoke(this, EventArgs.Empty);
        }

 

    }
}