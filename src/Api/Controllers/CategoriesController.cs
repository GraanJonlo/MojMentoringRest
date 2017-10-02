using Api.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class CategoriesController : Controller
    {
         [HttpPost]
         public IActionResult Post([FromBody] DomainTypes.CreateCategoryCommand dto)
         {
             var category = UseCases.createCategory(dto);

             if (category.IsOk)
             {
                 var result = new CategoryDto
                 {
                     Id = category.ResultValue.id,
                     Name = category.ResultValue.name,
                     Slug = category.ResultValue.slug,
                     Description = category.ResultValue.description,
                     Status = category.ResultValue.status
                 };
                 return Created("/api/v1/categories/" + result.Id, result);
             }
             else
             {
                 return BadRequest(category.ErrorValue);
             }
         }
    }
}
