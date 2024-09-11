namespace WireApps.Data.Models;

public class GameBoard
{
    public const int GridSize = 10;
    public string[][] Board { get; set; }
    public List<Ship> Ships { get; set; } = new List<Ship>();

    public GameBoard()
    {
        // Initialize the jagged array
        Board = new string[GridSize][];
        for (int i = 0; i < GridSize; i++)
        {
            Board[i] = new string[GridSize]; // Each row is an array of 10 columns
        }
    }
}



