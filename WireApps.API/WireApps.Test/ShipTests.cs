using WireApps.Data.Models;

namespace WireApps.Test;

public class ShipTests
{
    [Fact]
    public void ShipIsSunk_AllCoordinatesHit_ReturnsTrue()
    {
        // Arrange
        var ship = new Ship("Battleship", 5);
        ship.Coordinates.AddRange(new (int, int)[] { (0, 0), (0, 1), (0, 2), (0, 3), (0, 4) });

        // Act - Hit all coordinates
        foreach (var coord in ship.Coordinates.ToList())
        {
            ship.Hit(coord.Row, coord.Col);
        }

        // Assert
        Assert.True(ship.IsSunk);
    }

    [Fact]
    public void ShipIsNotSunk_NotAllCoordinatesHit_ReturnsFalse()
    {
        // Arrange
        var ship = new Ship("Battleship", 5);
        ship.Coordinates.AddRange(new (int, int)[] { (0, 0), (0, 1), (0, 2), (0, 3), (0, 4) });

        // Act - Hit only part of the ship
        ship.Hit(0, 0);
        ship.Hit(0, 1);

        // Assert
        Assert.False(ship.IsSunk);
    }

    [Fact]
    public void ShipHit_HitsCorrectCoordinate()
    {
        // Arrange
        var ship = new Ship("Destroyer", 4);
        ship.Coordinates.AddRange(new (int, int)[] { (1, 1), (1, 2), (1, 3), (1, 4) });

        // Act
        ship.Hit(1, 3);

        // Assert - Only the coordinate (1,3) should be marked as hit
        Assert.Contains((-1, -1), ship.Coordinates);
        Assert.Contains((1, 4), ship.Coordinates);
    }
}