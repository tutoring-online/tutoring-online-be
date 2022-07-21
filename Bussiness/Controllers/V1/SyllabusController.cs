using System.Net.Mime;
using Anotar.NLog;
using Azure.Storage.Blobs;
using DataAccess.Entities.Subject;
using DataAccess.Entities.Syllabus;
using DataAccess.Models;
using DataAccess.Models.Category;
using DataAccess.Models.Subject;
using DataAccess.Models.Syllabus;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using tutoring_online_be.Services;
using tutoring_online_be.Utils;

namespace tutoring_online_be.Controllers.V1;


[ApiController]
[Route("/api/v1/syllabuses")]
public class SyllabusController : Controller
{
    private readonly ISyllabusService syllabusService;
    private readonly ISubjectService subjectService;
    private readonly IDistributedCache cache;

    private static readonly string BlobConnectionString =
        "DefaultEndpointsProtocol=https;AccountName=cs110032000821a5f14;AccountKey=4Z26K/ZrXugmA1OUcRf5bO4K4BSSBpaHN2iLLMovh5sizlp4e5g+UEhcjZaUP8AAE3IFmKzG7Swj+AStrwp2aQ==;EndpointSuffix=core.windows.net";

    private static readonly string BlobContainerName = "file";

    public SyllabusController(
        ISyllabusService syllabusService,
        ISubjectService subjectService,
        IDistributedCache cache
        )
    {
        this.syllabusService = syllabusService;
        this.subjectService = subjectService;
        this.cache = cache;
    }

    [HttpGet]
    public IActionResult GetSyllabuses([FromQuery] PageRequestModel model, [FromQuery] SearchSyllabusRequest request)
    {
        if (AppUtils.HaveQueryString(model) || AppUtils.HaveQueryString(request))
        {
            var orderByParams = AppUtils.SortFieldParsing(model.Sort, typeof(Syllabus));
            Page<SearchSyllabusResponse> responseData = syllabusService.GetSyllabuses(model, orderByParams, request);

            if (responseData.Data is not null || responseData.Data.Count > 0)
            {
                List<SearchSyllabusResponse> syllabusDtos = responseData.Data;
                HashSet<string> subjectIds = syllabusDtos.Select(t => t.SubjectId).NotEmpty().ToHashSet();
                
                if (subjectIds.Count > 0)
                {
                    Dictionary<string, SubjectDto> subjectDtos = subjectService.GetSubjects(subjectIds);
                    
                    foreach (var item in syllabusDtos.Where(item => subjectDtos.ContainsKey(item.SubjectId)))
                    {
                        item.Subject = subjectDtos[item.SubjectId];
                    }
                }
            }

            return Ok(responseData);
        }
        
        var sysllabusCache = cache.GetString("syllabuses");

        if (string.IsNullOrEmpty(sysllabusCache))
        {
            IEnumerable<SyllabusDto> syllabuses = syllabusService.GetSyllabuses();
            var serializer = JsonConvert.SerializeObject(syllabuses);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(new TimeSpan(0, 3, 0));
            cache.SetString("syllabuses", serializer, options);
            
            return Ok(JsonConvert.DeserializeObject<IEnumerable<SyllabusDto>>(cache.GetString("syllabuses")));
        }
            
        return Ok(JsonConvert.DeserializeObject<IEnumerable<SyllabusDto>>(sysllabusCache));
    }
    

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<SyllabusDto> GetSyllabus(string id)
    {
        var sysllabusCache = cache.GetString("syllabuses");
        
        if (string.IsNullOrEmpty(sysllabusCache))
        {
            IEnumerable<SyllabusDto> syllabuses = syllabusService.GetSyllabuses();
            var serializer = JsonConvert.SerializeObject(syllabuses);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(new TimeSpan(0, 3, 0));
            cache.SetString("categories", serializer, options);
        }

        IEnumerable<SyllabusDto> list =
            JsonConvert.DeserializeObject<IEnumerable<SyllabusDto>>(cache.GetString("syllabuses"));
        
        return list.Where(t => t.Id.Equals(id));
    }
    
    [HttpPost]
    public IActionResult CreateSyllabuses([FromBody]IEnumerable<CreateSyllabusDto> syllabusDto)
    {
        IEnumerable<Syllabus> syllabuses = syllabusDto.Select(syllabusDto => syllabusDto.AsEntity());

        syllabusService.CreateSyllabuses(syllabuses);
        return Created("", "");
    }
    
    [HttpPatch]
    [Route("{id}")]
    public void UpdateSyllabus(string id, [FromBody]UpdateSyllabusDto updateSyllabusDto)
    {
        var syllabuses = syllabusService.GetSyllabusById(id);
        if (syllabuses.Any())
        {
            if (updateSyllabusDto.Image is not null)
            {
                try
                {
                    LogTo.Info("\n Do upload image to Azure Blob Storage");

                    var bytes = Convert.FromBase64String(updateSyllabusDto.Image);
                    var imageName = DateTimeOffset.Now.ToUnixTimeSeconds() + ".jpg";

                    var container = new BlobContainerClient(BlobConnectionString, BlobContainerName);
                    var blob = container.GetBlobClient(imageName);
                    
                    using (var stream = new MemoryStream(bytes))
                    {
                        blob.Upload(stream, true);
                    }

                    var blobUrl = blob.Uri.AbsoluteUri;

                    updateSyllabusDto.Image = blobUrl;
                    
                    LogTo.Info("\n Complete upload image to Azure Blob Storage : " + blobUrl);
                }
                catch (Exception e)
                {
                    LogTo.Info("Upload file error : " + e);
                    updateSyllabusDto.Image = null;
                }
            }

            syllabusService.UpdateSyllabus(updateSyllabusDto.AsEntity(), id);
        }
    }

}
