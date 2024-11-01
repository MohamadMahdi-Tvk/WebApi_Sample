using Microsoft.AspNetCore.Mvc;

namespace WebApi_Sample.Controllers;

[Route("api/ToDo/{ToDoId}/Categories/{CategoryId}")]
[ApiVersion("1")]
[ApiController]
public class ToDoCategoryController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(int ToDoId, int CategoryId)
    {
        // Find TodoId And CategoryId, Then Set Category for Todo

        return Ok();
    }
}
