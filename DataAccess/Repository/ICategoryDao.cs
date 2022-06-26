using DataAccess.Entities.Category;

namespace DataAccess.Repository;

public interface ICategoryDao
{
    IEnumerable<Category?> GetCategories();

    IEnumerable<Category?> GetCategoryById(string id);

    void CreateCategories(IEnumerable<Category> categories);
    void UpdateCategories(Category category, string id);
    int DeleteCategory(string id);
    Dictionary<string, Category> GetCategories(HashSet<string> ids);
}