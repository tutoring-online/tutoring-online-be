using DataAccess.Models.Tutor;

namespace tutoring_online_be.Services;

public interface ITutorService
{
    IEnumerable<TutorDto> GetTutors();

    IEnumerable<TutorDto> GetTutorById(string id);

}