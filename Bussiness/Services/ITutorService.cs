using DataAccess.Models.Tutor;
using FirebaseAdmin.Auth;

namespace tutoring_online_be.Services;

public interface ITutorService
{
    IEnumerable<TutorDto> GetTutors();

    IEnumerable<TutorDto> GetTutorById(string id);
    IEnumerable<TutorDto> GetTutorByFirebaseUid(string uid);
    int CreateTutorByFirebaseToken(FirebaseToken token);

}