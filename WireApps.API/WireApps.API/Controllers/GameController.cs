using Microsoft.AspNetCore.Mvc;
using WireApps.Data.Models;
using WireApps.Data.ViewModels;

namespace WireApps.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static Game _game = new();

        [HttpGet("new")]
        public IActionResult NewGame()
        {
            _game = new Game();
            return Ok(new { message = "New game started!" });
        }

        [HttpPost("shot")]
        public IActionResult FireShot([FromBody] FireShotRequest request)
        {
            var (hit, message) = _game.FireShot(request.Row, request.Col);
            return Ok(new { hit, message, allSunk = _game.AreAllShipsSunk() });
        }
    }
}
