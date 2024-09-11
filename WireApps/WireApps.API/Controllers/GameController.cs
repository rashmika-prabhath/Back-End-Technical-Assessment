using Microsoft.AspNetCore.Mvc;
using WireApps.Data.Models;
using WireApps.Data.ViewModels;

namespace WireApps.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private static GameBoard _gameBoard;

    // POST api/game/start
    [HttpPost("start")]
    public IActionResult StartGame()
    {
        _gameBoard = InitializeGameBoard();
        var response = new StartGameResponse
        {
            Message = "Game started",
            Board = GetHiddenBoard()
        };
        return Ok(response);
    }

    // POST api/game/attack
    [HttpPost("attack")]
    public IActionResult Attack([FromBody] AttackRequest attack)
    {
        var result = ProcessAttack(attack.Row, attack.Col);
        var response = new AttackResponse
        {
            Message = result.Message,
            Status = result.Status
        };
        return Ok(response);
    }

    // Initialize the game board with random ship placement
    private GameBoard InitializeGameBoard()
    {
        var board = new GameBoard();
        PlaceShips(board);
        return board;
    }

    // Place ships on the board
    private void PlaceShips(GameBoard board)
    {
        PlaceShip(board, "Battleship", 5);  // Place 1x Battleship (5 squares)
        PlaceShip(board, "Destroyer", 4);   // Place 2x Destroyers (4 squares each)
        PlaceShip(board, "Destroyer", 4);
    }

    // Logic for placing a ship on the board
    private void PlaceShip(GameBoard board, string name, int size)
    {
        var ship = new Ship { Name = name, Size = size };
        var random = new Random();
        bool placed = false;

        while (!placed)
        {
            var orientation = random.Next(0, 2) == 0 ? "horizontal" : "vertical";
            var row = random.Next(0, GameBoard.GridSize);
            var col = random.Next(0, GameBoard.GridSize);

            if (CanPlaceShip(board, row, col, size, orientation))
            {
                for (int i = 0; i < size; i++)
                {
                    if (orientation == "horizontal")
                    {
                        board.Board[row][col + i] = name;
                        ship.Coordinates.Add((row, col + i));
                    }
                    else
                    {
                        board.Board[row + i][col] = name;
                        ship.Coordinates.Add((row + i, col));
                    }
                }
                board.Ships.Add(ship);
                placed = true;
            }
        }
    }

    // Check if the ship can be placed at the given coordinates
    private bool CanPlaceShip(GameBoard board, int row, int col, int size, string orientation)
    {
        if (orientation == "horizontal" && col + size > GameBoard.GridSize) return false;
        if (orientation == "vertical" && row + size > GameBoard.GridSize) return false;

        for (int i = 0; i < size; i++)
        {
            if (orientation == "horizontal" && !string.IsNullOrEmpty(board.Board[row][col + i])) return false;
            if (orientation == "vertical" && !string.IsNullOrEmpty(board.Board[row + i][col])) return false;
        }
        return true;
    }

    // Process the user's attack and return hit/miss status
    private AttackResponse ProcessAttack(int row, int col)
    {
        if (!string.IsNullOrEmpty(_gameBoard.Board[row][col]))
        {
            var ship = _gameBoard.Ships.FirstOrDefault(s => s.Coordinates.Contains((row, col)));
            ship.Hits++;
            _gameBoard.Board[row][col] = "hit";
            if (ship.IsSunk)
                return new AttackResponse() { Message = $"You sunk a {ship.Name}!", Status = "sunk" };
            return new AttackResponse() { Message = "Hit!", Status = "hit" };
        }
        _gameBoard.Board[row][col] = "miss";
        return new AttackResponse(){ Message = "Miss!", Status = "miss" };
    }

    // Return the game board with hidden ships
    private string[][] GetHiddenBoard()
    {
        var hiddenBoard = new string[GameBoard.GridSize][];
        for (int row = 0; row < GameBoard.GridSize; row++)
        {
            hiddenBoard[row] = new string[GameBoard.GridSize];
            for (int col = 0; col < GameBoard.GridSize; col++)
            {
                hiddenBoard[row][col] = _gameBoard.Board[row][col] == "hit" || _gameBoard.Board[row][col] == "miss"
                    ? _gameBoard.Board[row][col]
                    : "empty";
            }
        }
        return hiddenBoard;
    }
}