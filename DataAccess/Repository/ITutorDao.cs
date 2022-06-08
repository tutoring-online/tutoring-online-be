using DataAccess.Entities.Tutor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository;

public interface ITutorDao
{
    IEnumerable<Tutor?> GetTutors();

    IEnumerable<Tutor?> GetTutorById(string id);

}
