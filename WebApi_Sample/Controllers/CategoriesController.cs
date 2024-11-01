using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_Sample.Models.Services;

namespace WebApi_Sample.Controllers;

//[Route("api/v{version:apiVersion}/[controller]")] //api/v1/r => get api in routing
[ApiVersion("1")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly CategoryRepository _categoryRepository;
    public CategoriesController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    /// <summary>
    /// لیست دسته بندی ها را دریافت کن 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_categoryRepository.GetAll());
    }

    /// <summary>
    /// اطلاعات دسته بندی را دریافت کن
    /// </summary>
    /// <param name="Id">شناسه دسته بندی</param>
    /// <returns></returns>
    [HttpGet("{Id}")]
    public IActionResult Get(int Id)
    {
        return Ok(_categoryRepository.Find(Id));
    }

    [HttpPut]
    public IActionResult Put(CategoryDto categoryDto)
    {
        return Ok(_categoryRepository.Edit(categoryDto));
    }

    [HttpPost]
    public IActionResult Post(string Name)
    {
        var result = _categoryRepository.AddCategory(Name);
        return Created(Url.Action(nameof(Get), "Categories", new { Id = result }, Request.Scheme), true);

    }

    [HttpDelete]
    public IActionResult Delete(int Id)
    {
        return Ok(_categoryRepository.Delete(Id));
    }
}
