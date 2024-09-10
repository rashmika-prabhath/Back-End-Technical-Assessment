namespace WireApps.Data.Models;

public class Game
{
    public const int GridSize = 10;
    public char[,] Grid { get; private set; }
    public List<Ship> Ships { get; private set; }

    public Game()
    {
        Grid = new char[GridSize, GridSize];
        Ships = new List<Ship>
    {
        new Ship("Battleship", 5),
        new Ship("Destroyer 1", 4),
        new Ship("Destroyer 2", 4)
    };

        InitializeGrid();
        PlaceShips();
    }

    private void InitializeGrid()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                Grid[i, j] = '~'; // Water
            }
        }
    }

    private void PlaceShips()
    {
        Random random = new();
        foreach (var ship in Ships)
        {
            bool placed = false;
            while (!placed)
            {
                int orientation = random.Next(0, 2); // 0: horizontal, 1: vertical
                int row = random.Next(0, GridSize);
                int col = random.Next(0, GridSize);
                if (CanPlaceShip(ship, row, col, orientation))
                {
                    for (int i = 0; i < ship.Size; i++)
                    {
                        if (orientation == 0) // Horizontal
                        {
                            Grid[row, col + i] = 'S';
                            ship.Coordinates.Add((row, col + i));
                        }
                        else // Vertical
                        {
                            Grid[row + i, col] = 'S';
                            ship.Coordinates.Add((row + i, col));
                        }
                    }
                    placed = true;
                }
            }
        }
    }

    private bool CanPlaceShip(Ship ship, int row, int col, int orientation)
    {
        if (orientation == 0 && col + ship.Size > GridSize) return false; // Horizontal boundary check
        if (orientation == 1 && row + ship.Size > GridSize) return false; // Vertical boundary check

        for (int i = 0; i < ship.Size; i++)
        {
            if (orientation == 0 && Grid[row, col + i] != '~') return false;
            if (orientation == 1 && Grid[row + i, col] != '~') return false;
        }
        return true;
    }

    public (bool hit, string message) FireShot(int row, int col)
    {
        if (Grid[row, col] == 'S') // It's a hit
        {
            Grid[row, col] = 'X'; // Mark as hit
            foreach (var ship in Ships)
            {
                ship.Hit(row, col);
                if (ship.IsSunk)
                {
                    return (true, $"You sunk the {ship.Name}!");
                }
            }
            return (true, "Hit!");
        }
        else
        {
            Grid[row, col] = 'M'; // Mark as miss
            return (false, "Miss!");
        }
    }

    public bool AreAllShipsSunk()
    {
        return Ships.All(s => s.IsSunk);
    }
}
