using WireApps.Data.Models;

namespace WireApps.Test;

public class GameTests
{
    [Fact]
    public void GameInitializes_GridHasWater()
    {
        // Arrange
        var game = new Game();

        // Act
        var grid = game.Grid;

        // Assert - Ensure all cells are water ('~')
        for (int i = 0; i < Game.GridSize; i++)
        {
            for (int j = 0; j < Game.GridSize; j++)
            {
                Assert.Equal('~', grid[i, j]);
            }
        }
    }

    [Fact]
    public void GamePlacesShips_GridContainsShips()
    {
        // Arrange
        var game = new Game();

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
        game.Ships[0].Coordinates.Clear();
        game.Ships[0].Coordinates.AddRange(new (int, int)[] { (0, 0), (0, 1), (0, 2), (0, 3), (0, 4) });
        game.Grid[0, 0] = 'S'; // Manually place a ship

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
        var result = game.FireShot(5, 5); // Assume this is water

        // Assert
        Assert.False(result.hit);
        Assert.Equal("Miss!", result.message);
    }

    [Fact]
    public void GameAreAllShipsSunk_ReturnsTrueWhenAllSunk()
    {
        // Arrange
        var game = new Game();
        foreach (var ship in game.Ships)
        {
            foreach (var coord in ship.Coordinates.ToList())
            {
                ship.Hit(coord.Row, coord.Col); // Sink all ships
            }
        }

        // Act
        bool allSunk = game.AreAllShipsSunk();

        // Assert
        Assert.True(allSunk);
    }

    [Fact]
    public void GameAreAllShipsSunk_ReturnsFalseWhenNotAllSunk()
    {
        // Arrange
        var game = new Game();
        var ship = game.Ships[0];
        ship.Hit(ship.Coordinates[0].Row, ship.Coordinates[0].Col); // Hit only one part

        // Act
        bool allSunk = game.AreAllShipsSunk();

        // Assert
        Assert.False(allSunk);
    }
}
