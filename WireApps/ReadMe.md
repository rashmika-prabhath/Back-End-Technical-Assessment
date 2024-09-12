+-------------------+                         +---------------------+
|                   |  /api/game/start        |                     |
|     Client        +-----------------------> |  GameController      |
| (e.g., Frontend)  |  (POST Request)         | (ASP.NET Core API)   |
|                   |                         |                     |
+-------------------+                         +----------+----------+
                                                         |
                                                         | (Start Game Logic)
                                                         v
                                               +---------------------+
                                               |     GameBoard       |
                                               |  (10x10 Grid, Ships)|
                                               +---------------------+
                                               
                                               
+-------------------+                         +---------------------+
|                   |  /api/game/attack       |                     |
|     Client        +-----------------------> |  GameController      |
| (e.g., Frontend)  |  (POST Request)         | (ASP.NET Core API)   |
|                   |                         |                     |
+-------------------+                         +----------+----------+
                                                         |
                                                         | (Attack Logic)
                                                         v
                                               +---------------------+
                                               |    Ships (Hit/Miss)  |
                                               +---------------------+

API Endpoints:

/api/game/start: Initializes a new game, randomly places ships on a 10x10 board, and returns a board with hidden ships and a message indicating the game has started.
/api/game/attack: Processes an attack on the game board. Takes player input (row and column), checks for a hit or miss, updates the board, and responds with the result (hit, miss, or sink).
Core Models:

GameBoard: Represents the game board, containing a 10x10 grid and a list of ships.
Ship: Represents a ship, including its name, size, position (coordinates), and hit status.
AttackRequest: Represents a player's attack request, containing the row and column of the target.
StartGameResponse and AttackResponse: Strongly-typed response classes used to convey game state and action results to the client.
Game Logic:

Start Game: When a game starts, ships (1 battleship and 2 destroyers) are randomly placed on the board. Ships cannot overlap or extend beyond the grid.
Attack: When an attack is processed, the API checks if a ship occupies the attacked cell. If a ship is present, the cell is marked as a hit, and if all parts of the ship are hit, the ship is considered sunk. If no ship is present, the cell is marked as a miss.
Unit Testing:

XUnit test cases validate the core game functionalities, ensuring that:
The game starts correctly with an empty, hidden board.
Attacks on empty cells result in a miss.
Attacks on ships result in a hit, and after multiple hits, ships can be sunk.

Technology Stack
Backend Framework: ASP.NET Core Web API
Programming Language: C#
Serialization Library: System.Text.Json for JSON serialization
Unit Testing: XUnit for test cases
Dependencies: Microsoft.AspNetCore.Mvc, System.Text.Json