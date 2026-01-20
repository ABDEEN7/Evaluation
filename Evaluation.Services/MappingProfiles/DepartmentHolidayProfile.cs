using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.Calendars;
using Evaluation.SharedHelper.Dtos.HrDto;
using Evaluation.SharedHelper.Models.Api.DepartmentHolidaysDto;

namespace Evaluation.Services.MappingProfiles;

public class DepartmentHolidayProfile : Profile
{
    public DepartmentHolidayProfile()
    {
        CreateMap<DepartmentHoliday, DepartmentHolidayOrgDto>();
        CreateMap<DepartmentHoliday, DepartmentHolidayDto>();
        CreateMap<CreateDepartmentHolidayDto, DepartmentHoliday>();
        CreateMap<UpdateDepartmentHolidayDto, DepartmentHoliday>();
    }
}
