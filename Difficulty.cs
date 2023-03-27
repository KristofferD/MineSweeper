using System;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public static class DifficultyExtensions
{
    public static int GetRows(this Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return 8;
            case Difficulty.Medium:
                return 16;
            case Difficulty.Hard:
                return 24;
            default:
                throw new ArgumentException($"Invalid difficulty: {difficulty}");
        }
    }

    public static int GetColumns(this Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return 8;
            case Difficulty.Medium:
                return 16;
            case Difficulty.Hard:
                return 24;
            default:
                throw new ArgumentException($"Invalid difficulty: {difficulty}");
        }
    }

    public static int GetBombs(this Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return 10;
            case Difficulty.Medium:
                return 40;
            case Difficulty.Hard:
                return 99;
            default:
                throw new ArgumentException($"Invalid difficulty: {difficulty}");
        }
    }
}
