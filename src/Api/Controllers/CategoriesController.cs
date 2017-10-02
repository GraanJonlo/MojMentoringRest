using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class CategoriesController : Controller
    {
        [HttpPost]
        public IActionResult Post([FromBody] Category.Dto dto)
        {
            var category = UseCases.createCategory(dto);

            if (category.IsOk)
            {
                var result = Category.dto(category.ResultValue);
                return Created("/api/v1/categories/" + result.id, result);
            }
            else
            {
                return BadRequest(category.ErrorValue);
            }
        }
    }
}
