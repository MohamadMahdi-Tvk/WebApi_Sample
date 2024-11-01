using Microsoft.AspNetCore.Mvc;

namespace WebApi_Sample.Controllers.V1;

[ApiVersion("1", Deprecated = true)]
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{

    [HttpGet]
    public virtual IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

 
    [HttpGet("{id}")]
    public virtual string Get(int id)
    {
        return "value";
    }

    [HttpPost]
    public virtual void Post([FromBody] string value)
    {
    }

 
    [HttpPut("{id}")]
    public virtual void Put(int id, [FromBody] string value)
    {
    }

   
    [HttpDelete("{id}")]
    public virtual void Delete(int id)
    {
    }
}
