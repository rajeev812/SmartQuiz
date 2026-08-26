using Microsoft.AspNetCore.Mvc;
using SmartQuiz.Domain.Entities;

namespace SmartQuiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Board>> GetBoards()
    {
        return Ok(new List<Board>
        {
            new() { Id = Guid.NewGuid(), BoardName = "CBSE" },
            new() { Id = Guid.NewGuid(), BoardName = "ICSE" }
        });
    }

    [HttpPost]
    public ActionResult<Board> CreateBoard([FromBody] Board board)
    {
        return Ok(board);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<Board> UpdateBoard(Guid id, [FromBody] Board board)
    {
        board.Id = id;
        return Ok(board);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBoard(Guid id)
    {
        return NoContent();
    }
}
