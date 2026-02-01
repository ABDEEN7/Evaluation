using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/{depRouting}/[action]")]

public class AcademicYearController(MasterBL masterBl) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetVcationDate()
    {
        //var vcationDate = await masterBL.GetApiService<>
        var vacationDates = await GetVacationDatesAsync();

        // Return in the structure your JS expects
        return Ok(new
        {
            result = vacationDates.Select(x => new
            {
                date = x.Date.ToString("yyyy-MM-dd")
            })
        });

    }
    public class VacationDateDto
    {
        public DateTime Date { get; set; }
    }
    [HttpGet]
    public async Task<List<VacationDateDto>> GetVacationDatesAsync()
    {
        return new List<VacationDateDto>
    {
        new VacationDateDto { Date = new DateTime(2025, 1, 1) },
        new VacationDateDto { Date = new DateTime(2025, 2, 14) }
    };
    }
    [HttpGet]
    public async Task<IActionResult> GetAcademicYearByDepartment()
    {
        var result = await masterBl.GetAdminService<SrvAcademicYearBL>().GetAcademicYearListByCureentDepartment();
        return Ok(result);
    }
}
