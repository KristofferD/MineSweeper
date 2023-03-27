public class Game
{
    private readonly Board _board;

    public Game(int rows, int columns, int bombs)
    {
        _board = new Board(rows, columns, bombs);
    }

    public bool IsOver { get; private set; }

    public void RevealTile(int row, int col)
    {
        var tile = _board.GetTile(row, col);

        if (tile.IsRevealed || tile.IsFlagged)
        {
            return;
        }

        tile.IsRevealed = true;

        if (tile.IsBomb)
        {
            IsOver = true;
            return;
        }
        else if (tile.AdjacentBombs == 0)
        {
            foreach (var adjacentTile in _board.GetAdjacentTiles(row, col))
            {
                RevealTile(adjacentTile.Row, adjacentTile.Column);
            }
        }

        if (_board.AllNonBombTilesRevealed())
        {
            IsOver = true;
            return;
        }
    }

    public void FlagTile(int row, int col)
    {
        var tile = _board.GetTile(row, col);

        if (!tile.IsRevealed)
        {
            tile.IsFlagged = !tile.IsFlagged;
        }
    }

    public void Reset()
    {
        IsOver = false;
        _board.Reset();
    }
}
