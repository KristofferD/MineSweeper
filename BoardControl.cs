using Minesweeper;
using System.Resources;
using System.Windows.Forms;
using System;

public class BoardControl : UserControl
{
    private GameController gameController;
    private CellControl[,] cellControls;

    public BoardControl(int rows, int cols, int numMines)
    {
        CreateGameBoard(rows, cols, numMines);
        CreateCellControls(rows, cols);
    }

    private void CreateGameBoard(int rows, int cols, int numMines)
    {
        gameController = new GameController(rows, cols, numMines);
    }

    private void CreateCellControls(int rows, int cols)
    {
        cellControls = new CellControl[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                var cellControl = new CellControl(row, col);
                cellControl.LeftClick += OnCellLeftClick;
                cellControl.RightClick += OnCellRightClick;
                Controls.Add(cellControl);
                cellControls[row, col] = cellControl;
            }
        }
    }

    private void OnCellLeftClick(object sender, EventArgs e)
    {
        var cellControl = (CellControl)sender;
        gameController.RevealCell(cellControl.Row, cellControl.Col);
        UpdateBoard();
    }

    private void OnCellRightClick(object sender, EventArgs e)
    {
        var cellControl = (CellControl)sender;
        gameController.ToggleFlag(cellControl.Row, cellControl.Col);
        UpdateBoard();
    }

    private void UpdateBoard()
    {
        bool gameOver = gameController.CheckForGameOver();
        if (gameOver)
        {
            string message;
            if (gameController.GameWon)
            {
                message = "Congratulations, you won!";
            }
            else
            {
                message = "Sorry, you lost. Better luck next time!";
            }
            MessageBox.Show(message);
            DisableAllCellControls();
            return;
        }

        for (int row = 0; row < gameController.Rows; row++)
        {
            for (int col = 0; col < gameController.Columns; col++)
            {
                UpdateCellState(row, col);
            }
        }

        // Update timer, mine counter, etc.
    }

    private void DisableAllCellControls()
    {
        foreach (CellControl cellControl in cellControls)
        {
            cellControl.Enabled = false;
        }
    }

    private void UpdateCellState(int row, int col, CellState cellState)
    {
        // Get the cell control for the specified row and column
        CellControl cellControl = cellControls[row, col];

        // Set the cell state based on the game state
        switch (cellState)
        {
            case CellState.Hidden:
                cellControl.BackgroundImage = Resources.hidden;
                cellControl.Enabled = true;
                break;
            case CellState.Flagged:
                cellControl.BackgroundImage = Resources.flag;
                cellControl.Enabled = true;
                break;
            case CellState.QuestionMark:
                cellControl.BackgroundImage = Resources.question;
                cellControl.Enabled = true;
                break;
            case CellState.Revealed:
                int adjacentMines = gameController.GetAdjacentMines(row, col);
                cellControl.Enabled = false;
                cellControl.SetAdjacentMines(adjacentMines);

                switch (adjacentMines)
                {
                    case 0:
                        cellControl.BackgroundImage = Resources.zero;
                        break;
                    case 1:
                        cellControl.BackgroundImage = Resources.one;
                        break;
                    case 2:
                        cellControl.BackgroundImage = Resources.two;
                        break;
                    case 3:
                        cellControl.BackgroundImage = Resources.three;
                        break;
                    case 4:
                        cellControl.BackgroundImage = Resources.four;
                        break;
                    case 5:
                        cellControl.BackgroundImage = Resources.five;
                        break;
                    case 6:
                        cellControl.BackgroundImage = Resources.six;
                        break;
                    case 7:
                        cellControl.BackgroundImage = Resources.seven;
                        break;
                    case 8:
                        cellControl.BackgroundImage = Resources.eight;
                        break;
                    default:
                        throw new InvalidOperationException("Invalid number of adjacent mines.");
                }
                break;
            case CellState.Mine:
                cellControl.Enabled = false;
                cellControl.BackgroundImage = Resources.mine;
                break;
            default:
                throw new InvalidOperationException("Invalid cell state.");
        }
    }

    public void NewGame(int rows, int cols, int numMines)
    {
        CreateGameBoard(rows, cols, numMines);
        CreateCellControls(rows, cols);
        ResetBoard();
    }

    public void ResetBoard()
    {
        gameController.Reset();
        ResetCellControls();
    }

    private void ResetCellControls()
    {
        foreach (CellControl cellControl in cellControls)
        {
            cellControl.Reset();
        }
    }

    public void ToggleSound()
    {
        // Toggle the sound option
        SoundManager.SoundEnabled = !SoundManager.SoundEnabled;

        // Update the sound icon in each cell control
        foreach (CellControl cellControl in cellControls)
        {
            cellControl.UpdateSoundIcon();
        }
    }

    public void ToggleMusic()
    {
        // Toggle the music option
        SoundManager.MusicEnabled = !SoundManager.MusicEnabled;

        // If music is now enabled, start playing it
        if (SoundManager.MusicEnabled)
        {
            SoundManager.PlayMusic();
        }
        else // Otherwise, stop playing it
        {
            SoundManager.StopMusic();
        }

        // Update the music icon in each cell control
        foreach (CellControl cellControl in cellControls)
        {
            cellControl.UpdateMusicIcon();
        }
    }

}