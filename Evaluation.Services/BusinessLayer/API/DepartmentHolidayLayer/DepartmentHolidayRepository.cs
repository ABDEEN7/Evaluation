//using Evaluation.DAL.Models.Calendars;
//using Evaluation.DAL.Repositories;
//using Microsoft.EntityFrameworkCore;

//namespace Evaluation.Services.BusinessLayer.API.DepartmentHolidayLayer;

//public class DepartmentHolidayRepository(
//    UnitOfWork unitOfWork
//    ) : ApiServiceBase
//{
//    public async Task<List<DepartmentHoliday>> GetDepartmentHolidays(Guid academicYearId)
//    {
//        List<DepartmentHoliday> departmentHolidays = await unitOfWork
//            .GetRepository<DepartmentHoliday>()
//            .GetAllActiveNonDeleted(x => x.AcademicYearId == academicYearId)
//            .ToListAsync();
//        return departmentHolidays;
//    }
//    public async Task AddDepartmentHoliday(DepartmentHoliday model)
//    {
//        await unitOfWork.GetRepository<DepartmentHoliday>().InsertAsync(model);
//        await unitOfWork.CommitAsync();
//    }
//}
