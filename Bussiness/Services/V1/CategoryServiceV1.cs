using DataAccess.Entities.Category;
using DataAccess.Models.Category;
using DataAccess.Repository;
using DataAccess.Utils;

namespace tutoring_online_be.Services.V1;

public class CategoryServiceV1:ICategoryService
{
    private readonly ICategoryDao categoryDao;

    public CategoryServiceV1(ICategoryDao categoryDao)
    {
        this.categoryDao = categoryDao;
    }

    public IEnumerable<CategoryDto> GetCategories()
    {
        return categoryDao.GetCategories().Select(category => category.AsDto());
    }

    public IEnumerable<CategoryDto> GetCategoryById(string id)
    {
        return categoryDao.GetCategoryById(id).Select(category => category.AsDto());
    }

    public void CreateCategories(IEnumerable<Category> categories)
    {
        categoryDao.CreateCategories(categories);
    }

    public void UpdateCategories(Category category, string id)
    {
        categoryDao.UpdateCategories(category, id);
    }

    public int DeleteCategory(string id)
    {
        return categoryDao.DeleteCategory(id);
    }
}
