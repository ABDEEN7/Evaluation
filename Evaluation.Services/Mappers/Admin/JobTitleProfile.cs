using AutoMapper;
using Evaluation.DAL.Models.Master;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Services.Mappers.Admin;

public class JobTitleProfile : Profile
{
    public JobTitleProfile()
    {
        CreateMap<JobTitle, JobTitleDto>();
    }
}
