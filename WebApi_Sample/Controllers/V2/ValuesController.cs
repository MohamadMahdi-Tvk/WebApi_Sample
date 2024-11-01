using Microsoft.AspNetCore.Mvc;

namespace WebApi_Sample.Controllers.V2;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ValuesController : V1.ValuesController
{
    public override IEnumerable<string> Get()
    {
        return new string[] { "V2 value1", "V2 value2", "V2 value3" };
    }
}
