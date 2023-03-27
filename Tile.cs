public class Tile
{
    public Tile(bool isBomb, int adjacentBombs = 0)
    {
        IsBomb = isBomb;
        AdjacentBombs = adjacentBombs;
        IsRevealed = false;
        IsFlagged = false;
    }

    public bool IsBomb { get; }

    public int AdjacentBombs { get; }

    public bool IsRevealed { get; set; }

    public bool IsFlagged { get; set; }
}
