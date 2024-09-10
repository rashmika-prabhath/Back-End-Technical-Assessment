using WireApps.Data.Models;

namespace WireApps.Test;

public class GameTests
{
    [Fact]
    public void GameInitializes_GridHasWater()
    {
        // Arrange
        var game = new Game();

        // Act - Grid is initialized but ships have not yet been placed
        var grid = game.Grid;

        // Assert - Ensure all cells are water ('~') before placing ships
        for (int i = 0; i < Game.GridSize; i++)
        {
            for (int j = 0; j < Game.GridSize; j++)
            {
                Assert.Equal('~', grid[i, j]);  // Check that the grid is water
            }
        }
    }

    [Fact]
    public void GameInitializes_GridHasShipsAndWater()
    {
        // Arrange
        var game = new Game();
        game.PlaceShips();  // Ensure ships are placed on the grid

        // Act
        var grid = game.Grid;

        // Assert - Check that the grid contains both water and ships
        bool foundShip = false;
        bool foundWater = false;
        for (int i = 0; i < Game.GridSize; i++)
        {
            for (int j = 0; j < Game.GridSize; j++)
            {
                if (grid[i, j] == 'S')
                {
                    foundShip = true;
                }
                else if (grid[i, j] == '~')
                {
                    foundWater = true;
                }
            }
        }

        // Assert that both water and ships are found
        Assert.True(foundShip, "Expected to find ships on the grid.");
        Assert.True(foundWater, "Expected to find water on the grid.");
    }

    [Fact]
    public void GamePlacesShips_GridContainsShips()
    {
        // Arrange
        var game = new Game();
        game.PlaceShips();  // Ensure ships are placed on the grid

        // Act
        var grid = game.Grid;

        // Assert - Check that ships are placed ('S')
        bool foundShip = false;
        for (int i = 0; i < Game.GridSize; i++)
        {
            for (int j = 0; j < Game.GridSize; j++)
            {
                if (grid[i, j] == 'S')
                {
                    foundShip = true;
                    break;
                }
            }
        }

        Assert.True(foundShip);
    }

    [Fact]
    public void GameFireShot_HitsShip()
    {
        // Arrange
        var game = new Game();
        game.Ships[0].Coordinates.Clear();  // Clear existing coordinates
        game.Ships[0].Coordinates.AddRange(new (int, int)[] { (0, 0), (0, 1), (0, 2), (0, 3), (0, 4) });
        game.Grid[0, 0] = 'S';  // Manually place part of the ship

        // Act
        var result = game.FireShot(0, 0);

        // Assert
        Assert.True(result.hit);
        Assert.Equal("Hit!", result.message);
    }

    [Fact]
    public void GameFireShot_MissesWater()
    {
        // Arrange
        var game = new Game();

        // Act
        var result = game.FireShot(5, 5);  // Assume this is water

        // Assert
        Assert.False(result.hit);
        Assert.Equal("Miss!", result.message);
    }

    [Fact]
    public void GameAreAllShipsSunk_ReturnsTrueWhenAllSunk()
    {
        // Arrange
        var game = new Game();

        // Sink all ships manually
        foreach (var ship in game.Ships)
        {
            foreach (var coord in ship.Coordinates.ToList())
            {
                ship.Hit(coord.Row, coord.Col);  // Hit all ship parts
            }
        }

        // Act
        bool allSunk = game.AreAllShipsSunk();

        // Assert
        Assert.True(allSunk);  // Expect true as all ships are sunk
    }

    [Fact]
    public void GameAreAllShipsSunk_ReturnsFalseWhenNotAllSunk()
    {
        // Arrange
        var game = new Game();
        var ship = game.Ships[0];  // Get the first ship
        ship.Hit(ship.Coordinates[0].Row, ship.Coordinates[0].Col);  // Hit only one part of the ship

        // Act
        bool allSunk = game.AreAllShipsSunk();

        // Assert
        Assert.False(allSunk);  // Not all ships are sunk
    }

    [Fact]
    public void GameHitShip_MarksShipAsSunk()
    {
        // Arrange
        var game = new Game();
        var ship = game.Ships[0];
        var coordinates = ship.Coordinates.ToList();  // Copy coordinates to avoid modifying while iterating

        // Act - Hit all coordinates of the ship to sink it
        foreach (var coord in coordinates)
        {
            game.FireShot(coord.Row, coord.Col);
        }

        // Assert
        Assert.True(ship.IsSunk);  // Ensure the ship is marked as sunk
    }

    [Fact]
    public void GamePlaceShips_DoesNotOverlap()
    {
        // Arrange
        var game = new Game();
        game.PlaceShips();

        // Act & Assert - Ensure no ships overlap
        var shipCoordinates = game.Ships.SelectMany(ship => ship.Coordinates).ToList();
        var distinctCoordinates = shipCoordinates.Distinct().ToList();

        Assert.Equal(shipCoordinates.Count, distinctCoordinates.Count);  // No overlapping coordinates
    }
}

