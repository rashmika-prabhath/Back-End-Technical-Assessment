using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireApps.API.Controllers;
using WireApps.Data.Models;
using WireApps.Data.ViewModels;

namespace WireApps.Test;

public class GameControllerTests
{
    private readonly GameController _controller = new GameController();

    [Fact]
    public void TestStartGameReturnsHiddenBoard()
    {
        // Act
        IActionResult result = _controller.StartGame();

        // Assert
        Assert.IsType<OkObjectResult>(result);

        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);

        // Cast the result to the correct type
        var response = okResult.Value as StartGameResponse;
        Assert.NotNull(response);

        // Validate the message and board
        Assert.Equal("Game started", response.Message);
        Assert.NotNull(response.Board);
        Assert.Equal(10, response.Board.Length);  // Check that the board is 10x10

        foreach (var row in response.Board)
        {
            Assert.Equal(10, row.Length);
            Assert.All(row, cell => Assert.Equal("empty", cell));  // Board should initially show as "empty"
        }
    }

    [Fact]
    public void TestAttackMiss()
    {
        // Arrange
        _controller.StartGame();  // Start the game
        var attackRequest = new AttackRequest { Row = 0, Col = 0 };  // Attack a specific coordinate

        // Act
        IActionResult result = _controller.Attack(attackRequest);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);

        // Cast the result to the correct type
        var response = okResult.Value as AttackResponse;
        Assert.NotNull(response);

        // Validate the message
        Assert.Equal("Miss!", response.Message);
    }

    [Fact]
    public void TestAttackHit()
    {
        // Arrange
        _controller.StartGame();  // Start the game first

        // Manually set a ship at (0, 0) for testing a "Hit"
        _controller.GetType()
            .GetField("_gameBoard", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            .SetValue(null, new GameBoard
            {
                Board = new string[10][]
                {
                        new string[] { "Battleship", null, null, null, null, null, null, null, null, null },
                        new string[10], new string[10], new string[10], new string[10],
                        new string[10], new string[10], new string[10], new string[10], new string[10]
                },
                Ships = new List<Ship>
                {
                        new Ship { Name = "Battleship", Size = 5, Coordinates = new List<(int, int)> { (0, 0) } }
                }
            });

        // Attack where the ship is located (0,0)
        var attackRequest = new AttackRequest { Row = 0, Col = 0 };

        // Act
        IActionResult result = _controller.Attack(attackRequest);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);

        // Check the response message for "Hit"
        var response = okResult.Value as AttackResponse;
        Assert.Equal("Hit!", response.Message);
    }

    [Fact]
    public void TestAttackSinkShip()
    {
        // Arrange
        _controller.StartGame();

        // Manually set a ship at (0, 0) and (0, 1) for testing a "Sunk" scenario
        _controller.GetType()
            .GetField("_gameBoard", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            .SetValue(null, new GameBoard
            {
                Board = new string[10][]
                {
                        new string[] { "Battleship", "Battleship", null, null, null, null, null, null, null, null },
                        new string[10], new string[10], new string[10], new string[10],
                        new string[10], new string[10], new string[10], new string[10], new string[10]
                },
                Ships = new List<Ship>
                {
                        new Ship
                        {
                            Name = "Battleship",
                            Size = 2,
                            Coordinates = new List<(int, int)> { (0, 0), (0, 1) }
                        }
                }
            });

        // Attack part 1 of the ship
        var attackRequest1 = new AttackRequest { Row = 0, Col = 0 };
        IActionResult result1 = _controller.Attack(attackRequest1);

        // Assert "Hit!"
        Assert.IsType<OkObjectResult>(result1);
        var okResult1 = result1 as OkObjectResult;
        var response1 = okResult1.Value as AttackResponse;
        Assert.Equal("Hit!", response1.Message);

        // Attack part 2 of the ship (should sink the ship)
        var attackRequest2 = new AttackRequest { Row = 0, Col = 1 };
        IActionResult result2 = _controller.Attack(attackRequest2);

        // Assert "Sunk!"
        Assert.IsType<OkObjectResult>(result2);
        var okResult2 = result2 as OkObjectResult;
        var response2 = okResult2.Value as AttackResponse;
        Assert.Equal("You sunk a Battleship!", response2.Message);
    }
}
