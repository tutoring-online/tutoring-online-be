using DataAccess.Entities.Category;
using DataAccess.Models;
using DataAccess.Models.Category;

namespace DataAccess.Repository;

public interface ICategoryDao
{
    IEnumerable<Category?> GetCategories();

    IEnumerable<Category?> GetCategoryById(string id);

    void CreateCategories(IEnumerable<Category> categories);
    void UpdateCategories(Category category, string id);
    int DeleteCategory(string id);
    Dictionary<string, Category> GetCategories(HashSet<string> ids);
    Page<Category?> GetCategories(int? limit, int? offSet, List<Tuple<string, string>> orderByParams,
        SearchCategoryDto searchCategoryDto, bool isNotPaging);
}