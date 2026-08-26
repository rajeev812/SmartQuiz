using Microsoft.AspNetCore.Mvc;
using SmartQuiz.Domain.Entities;

namespace SmartQuiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<SchoolClass>> GetClasses()
    {
        return Ok(new List<SchoolClass>
        {
            new() { Id = Guid.NewGuid(), ClassName = "Class 10" },
            new() { Id = Guid.NewGuid(), ClassName = "Class 12" }
        });
    }

    [HttpPost]
    public ActionResult<SchoolClass> CreateClass([FromBody] SchoolClass schoolClass)
    {
        return Ok(schoolClass);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<SchoolClass> UpdateClass(Guid id, [FromBody] SchoolClass schoolClass)
    {
        schoolClass.Id = id;
        return Ok(schoolClass);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteClass(Guid id)
    {
        return NoContent();
    }
}
