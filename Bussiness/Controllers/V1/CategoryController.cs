using DataAccess.Entities.Category;
using DataAccess.Models;
using DataAccess.Models.Category;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using tutoring_online_be.Services;
using tutoring_online_be.Utils;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route("/api/v1/categories")]
public class CategoryController:Controller
{
    private readonly ICategoryService categoryService;
    private readonly IDistributedCache cache;

    public CategoryController(
        ICategoryService categoryService,
        IDistributedCache cache
        )
    {
        this.categoryService = categoryService;
        this.cache = cache;
    }

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<CategoryDto> GetCategory(string id)
    {
        var cacheCategory = cache.GetString("categories");

        if (string.IsNullOrEmpty(cacheCategory))
        {
            IEnumerable<CategoryDto> categories = categoryService.GetCategories();
            var serializer = JsonConvert.SerializeObject(categories);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(new TimeSpan(0, 3, 0));
            cache.SetString("categories", serializer, options);
        }

        IEnumerable<CategoryDto> list = JsonConvert.DeserializeObject<IEnumerable<CategoryDto>>(cache.GetString("categories"));

        return list.Where(t => t.Id.Equals(id));
    }
    [HttpPost]
    public IActionResult CreateCategories([FromBody]IEnumerable<CreateCategoryDto> categoryDto)
    {
        IEnumerable<Category> categories = categoryDto.Select(categoryDto => categoryDto.AsEntity());

        categoryService.CreateCategories(categories);
        return Created("", "");
    }

    [HttpPatch]
    [Route("{id}")]
    public void UpdateCategory(string id, [FromBody]UpdateCategoryDto categoryDto)
    {
        var categories = categoryService.GetCategoryById(id);
        if (categories.Any())
        {
            categoryService.UpdateCategories(categoryDto.AsEntity(), id);
        }

    }

    [HttpDelete]
    [Route("{id}")]
    public void DeleteCategory(string id)
    {
        categoryService.DeleteCategory(id);
    }
    [HttpGet]
    public IActionResult GetCategories([FromQuery] PageRequestModel model, [FromQuery] SearchCategoryDto searchCategoryDto)
    {
        if (AppUtils.HaveQueryString(model) || AppUtils.HaveQueryString(searchCategoryDto))
        {
            var orderByParams = AppUtils.SortFieldParsing(model.Sort, typeof(Category));

            Page<SearchCategoryDto> responseData = categoryService.GetCategories(model, orderByParams, searchCategoryDto);
            
            return Ok(responseData);
        }
        
        var cacheCategory = cache.GetString("categories");

        if (string.IsNullOrEmpty(cacheCategory))
        {
            IEnumerable<CategoryDto> lessonDtos = categoryService.GetCategories();
            var serializer = JsonConvert.SerializeObject(lessonDtos);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(new TimeSpan(0, 3, 0));
            cache.SetString("categories", serializer, options);
            
            return Ok(JsonConvert.DeserializeObject<IEnumerable<CategoryDto>>(cache.GetString("categories")));
        }

        return Ok(JsonConvert.DeserializeObject<IEnumerable<CategoryDto>>(cacheCategory));
    }
}