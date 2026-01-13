using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.Calendars;

namespace Evaluation.Services.MappingProfiles;

public class DepartmentHolidayProfile : Profile
{
    public DepartmentHolidayProfile()
    {
        CreateMap<DepartmentHoliday, DepartmentHolidayDto>();

    }
}
