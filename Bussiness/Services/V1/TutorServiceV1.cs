using DataAccess.Models.Tutor;
using DataAccess.Repository;
using DataAccess.Utils;

namespace tutoring_online_be.Services.V1;

public class TutorServiceV1 : ITutorService
{
    private readonly ITutorDao tutorDao;

    public TutorServiceV1(ITutorDao tutorDao)
    {
        this.tutorDao = tutorDao;
    }

    public IEnumerable<TutorDto> GetTutors()
    {
        return tutorDao.GetTutors().Select(tutor => tutor.AsDto());
    }

    public IEnumerable<TutorDto> GetTutorById(string id)
    {
        return tutorDao.GetTutorById(id).Select(tutor => tutor.AsDto());
    }

}
