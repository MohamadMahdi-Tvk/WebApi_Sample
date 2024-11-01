using Microsoft.AspNetCore.Mvc;
using WebApi_Sample.Models.Dto;
using WebApi_Sample.Models.Services;



namespace WebApi_Sample.Controllers;

[Route("api/[controller]")]
[ApiVersion("1")]
[ApiController]
public class ToDoController : ControllerBase
{
    private readonly TodoRepository todoRepository;
    public ToDoController(TodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }


    [HttpGet]
    public IActionResult Get()
    {
        var todoList = todoRepository.GetAll().Select(p => new ToDoItemDto
        {
            Id = p.Id,
            Text = p.Text,
            InsertTime = p.InsertTime,
            Links = new List<Links>()
            {
                new Links
                {
                    Href = Url.Action(nameof(Get),"ToDo",new {p.Id},Request.Scheme),
                    Rel = "Self",
                    Method = "Get"
                },
                new Links
                {
                    Href = Url.Action(nameof(Delete),"ToDo",new {p.Id},Request.Scheme),
                    Rel = "Delete",
                    Method = "Delete"
                },
                new Links
                {
                    Href = Url.Action(nameof(Put),"ToDo",Request.Scheme),
                    Rel = "Update",
                    Method = "Put"
                }
            }
        }).ToList();
        return Ok(todoList);

    }


    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var todo = todoRepository.Get(id);

        return Ok(new ToDoItemDto
        {
            Id = todo.Id,
            InsertTime = todo.InsertTime,
            Text = todo.Text
        });
    }


    [HttpPost]
    public IActionResult Post([FromBody] ItemDto item)
    {
        var result = todoRepository.Add(new AddToDoDto()
        {
            Todo = new TodoDto()
            {
                Text = item.Text,
            }
        });

        string url = Url.Action(nameof(Get), "ToDo", new { Id = result.Todo.Id }, Request.Scheme);

        return Created(url, true);
    }


    [HttpPut]
    public IActionResult Put([FromBody] EditToDoDto editToDo)
    {
        var result = todoRepository.Edit(editToDo);
        return Ok(result);
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        todoRepository.Delete(id);
        return Ok();
    }
}
