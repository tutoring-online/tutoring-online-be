using DataAccess.Entities.Category;
using DataAccess.Models.Category;

namespace tutoring_online_be.Services;

public interface ICategoryService
{
    IEnumerable<CategoryDto> GetCategories();

    IEnumerable<CategoryDto> GetCategoryById(string id);

    void CreateCategories(IEnumerable<Category> categories);
    void UpdateCategories(Category category, string id);
    int DeleteCategory(string id);
    Dictionary<string, CategoryDto> GetCategories(HashSet<string> categoryIds);
}