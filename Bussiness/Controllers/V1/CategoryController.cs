using DataAccess.Entities.Category;
using DataAccess.Models.Category;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Services;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route("/api/v1/categories")]
public class CategoryController
{
    private readonly ICategoryService categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    [HttpGet]
    public IEnumerable<CategoryDto> GetCategories()
    {
        return categoryService.GetCategories();
    }

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<CategoryDto> GetCategory(string id)
    {
        var categories = categoryService.GetCategoryById(id);

        return categories;
    }
    [HttpPost]
    public void CreateCategories(IEnumerable<CreateCategoryDto> categoryDto)
    {
        IEnumerable<Category> categories = categoryDto.Select(categoryDto => categoryDto.AsEntity());

        categoryService.CreateCategories(categories);

    }

    [HttpPatch]
    [Route("{id}")]
    public void UpdateCategory(string id, UpdateCategoryDto categoryDto)
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
}