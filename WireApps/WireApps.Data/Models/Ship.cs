namespace WireApps.Data.Models;

public class Ship
{
    public string Name { get; set; }
    public int Size { get; set; }
    public List<(int Row, int Col)> Coordinates { get; set; } = new List<(int, int)>();
    public int Hits { get; set; } = 0;
    public bool IsSunk => Hits >= Size;
}
