using DataAccess.Entities.Category;
using DataAccess.Models;
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

    public Dictionary<string, CategoryDto> GetCategories(HashSet<string> ids)
    {
        return categoryDao.GetCategories(ids).ToDictionary(pair => pair.Key, pair => pair.Value.AsDto());
    }
    public Page<SearchCategoryDto> GetCategories(PageRequestModel model, List<Tuple<string, string>> orderByParams,
       SearchCategoryDto searchCategoryDto)
    {
        Page<Category> result = new Page<Category>();
        Page<SearchCategoryDto> resultDto = new Page<SearchCategoryDto>();

        result = categoryDao.GetCategories(model.GetLimit(), model.GetOffSet(), orderByParams, searchCategoryDto, model.IsNotPaging());

        resultDto.Data = result.Data.Select(p => p.AsSearchDto()).ToList();
        resultDto.Pagination = result.Pagination;
        resultDto.Pagination.Page = model.GetPage();
        resultDto.Pagination.Size = model.GetSize();
        if (model.IsNotPaging())
        {
            resultDto.Pagination.TotalPages = 1;
            resultDto.Pagination.Size = resultDto.Pagination.TotalItems;
        }

        if (!result.Data.Any())
            resultDto.Pagination.TotalPages = 0;
        else
            resultDto.Pagination.UpdateTotalPages();

        return resultDto;
    }
}
