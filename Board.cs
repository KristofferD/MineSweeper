using System;

public class Board
{
    private readonly Tile[,] _tiles;

    public Board(int rows, int columns, int bombs)
    {
        // initialize the board with the given number of rows, columns, and bombs
        _tiles = new Tile[rows, columns];

        // add bombs to random tiles
        var random = new Random();
        for (int i = 0; i < bombs; i++)
        {
            int row = random.Next(rows);
            int col = random.Next(columns);

            // add bomb to the tile
            _tiles[row, col] = new Tile(true);
        }

        // set the number of adjacent bombs for each tile
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (_tiles[row, col] == null)
                {
                    int adjacentBombs = GetAdjacentBombs(row, col);
                    _tiles[row, col] = new Tile(false, adjacentBombs);
                }
            }
        }
    }

    private int GetAdjacentBombs(int row, int col)
    {
        // check adjacent tiles for bombs
        int adjacentBombs = 0;
        for (int i = row - 1; i <= row + 1; i++)
        {
            for (int j = col - 1; j <= col + 1; j++)
            {
                if (i >= 0 && i < _tiles.GetLength(0) &&
                    j >= 0 && j < _tiles.GetLength(1) &&
                    _tiles[i, j] != null &&
                    _tiles[i, j].IsBomb)
                {
                    adjacentBombs++;
                }
            }
        }
        return adjacentBombs;
    }

    public int Rows => _tiles.GetLength(0);

    public int Columns => _tiles.GetLength(1);

    public Tile GetTile(int row, int col)
    {
        return _tiles[row, col];
    }
}
