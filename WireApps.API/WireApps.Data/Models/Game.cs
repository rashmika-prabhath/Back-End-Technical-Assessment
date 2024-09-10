namespace WireApps.Data.Models;

public class Game
{
    public const int GridSize = 10;
    public char[,] Grid { get; private set; }
    public List<Ship> Ships { get; private set; }

    public Game()
    {
        // Initialize the grid and ships list
        Grid = new char[GridSize, GridSize];
        Ships = new List<Ship>
        {
            new Ship("Battleship", 5),
            new Ship("Destroyer 1", 4),
            new Ship("Destroyer 2", 4)
        };

        InitializeGrid();   // Ensure grid is initialized with water
        PlaceShips();       // Place ships after initializing the grid
    }

    private void InitializeGrid()
    {
        // Initialize the grid with '~' to represent water
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                Grid[i, j] = '~'; // Set all cells to water
            }
        }
    }

    // Method to handle firing a shot at a specific coordinate
    public (bool hit, string message) FireShot(int row, int col)
    {
        // Ensure coordinates are within the grid
        if (row < 0 || row >= GridSize || col < 0 || col >= GridSize)
        {
            return (false, "Shot is out of bounds.");
        }

        if (Grid[row, col] == 'S')  // It's a hit
        {
            Grid[row, col] = 'X';  // Mark the ship as hit
            foreach (var ship in Ships)
            {
                ship.Hit(row, col);  // Update the ship with the hit
                if (ship.IsSunk)
                {
                    return (true, $"You sunk the {ship.Name}!");
                }
            }
            return (true, "Hit!");
        }
        else if (Grid[row, col] == '~')  // It's a miss
        {
            Grid[row, col] = 'M';  // Mark as a miss
            return (false, "Miss!");
        }
        else
        {
            return (false, "You already shot here.");
        }
    }

    public void PlaceShips()
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

    public bool AreAllShipsSunk()
    {
        // If all ships are sunk (i.e., all coordinates are hit), return true
        return Ships.All(ship => ship.IsSunk);
    }

    private bool CanPlaceShip(Ship ship, int row, int col, int orientation)
    {
        if (orientation == 0 && col + ship.Size > GridSize) return false; // Horizontal boundary check
        if (orientation == 1 && row + ship.Size > GridSize) return false; // Vertical boundary check

        for (int i = 0; i < ship.Size; i++)
        {
            if (orientation == 0 && Grid[row, col + i] != '~') return false; // Check if already occupied
            if (orientation == 1 && Grid[row + i, col] != '~') return false; // Check if already occupied
        }
        return true;
    }
}



