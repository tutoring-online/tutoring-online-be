using DataAccess.Entities.Tutor;
using DataAccess.Models.Tutor;
using DataAccess.Repository;
using DataAccess.Utils;
using FirebaseAdmin.Auth;

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

    public IEnumerable<TutorDto> GetTutorByFirebaseUid(string uid)
    {
        return tutorDao.GetTutorByFirebaseUid(uid).Select(tutor => tutor.AsDto());
    }

    public int CreateTutorByFirebaseToken(FirebaseToken token)
    {
        var userRecord = FirebaseAuth.DefaultInstance.GetUserAsync(token.Uid).Result;

        var tutor = new Tutor()
        {
            uid = userRecord.Uid,
            Email = userRecord.Email,
            Name = userRecord.DisplayName,
            Phone = userRecord.PhoneNumber,
            Status = 0,
            AvatarURL = userRecord.PhotoUrl,
            CreatedDate = DateTime.Now
        };

        return tutorDao.CreateTutor(tutor);
    }

    public void UpdateTutor(Tutor asEntity, string id)
    {
        tutorDao.UpdateTutor(asEntity, id);
    }

    public int DeleteTutor(string id)
    {
        return tutorDao.DeleteTutor(id);
    }
}
