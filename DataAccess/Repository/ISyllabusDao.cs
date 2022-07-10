using DataAccess.Entities.Syllabus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Models.Syllabus;

namespace DataAccess.Repository;

public interface ISyllabusDao
{
    IEnumerable<Syllabus?> GetSyllabuses();

    IEnumerable<Syllabus?> GetSyllabusById(string id);

    void CreateSyllabuses(IEnumerable<Syllabus> syllabuses);
    void UpdateSyllabus(Syllabus asEntity, string id);
    Dictionary<string, Syllabus> GetSyllabuses(HashSet<string> ids);
    Page<Syllabus> GetSyllabuses(int? getLimit, int? getOffSet, List<Tuple<string, string>> orderByParams, SearchSyllabusRequest request, bool isNotPaging);
}
