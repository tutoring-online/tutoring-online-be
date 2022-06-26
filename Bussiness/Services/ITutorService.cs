using DataAccess.Entities.Tutor;
using DataAccess.Models.Tutor;
using FirebaseAdmin.Auth;

namespace tutoring_online_be.Services;

public interface ITutorService
{
    IEnumerable<TutorDto> GetTutors();

    IEnumerable<TutorDto> GetTutorById(string id);
    IEnumerable<TutorDto> GetTutorByFirebaseUid(string uid);
    int CreateTutorByFirebaseToken(FirebaseToken token);

    void UpdateTutor(Tutor asEntity, string id);
    int DeleteTutor(string id);
    Dictionary<string, TutorDto> GetTutors(HashSet<string> tutorIds);
}