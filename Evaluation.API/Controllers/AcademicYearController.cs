using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.PlanLayer;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

public class AcademicYearController(MasterBL masterBL) : ControllerBase
{
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
    public async Task<List<VacationDateDto>> GetVacationDatesAsync()
    {
        return new List<VacationDateDto>
    {
        new VacationDateDto { Date = new DateTime(2025, 1, 1) },
        new VacationDateDto { Date = new DateTime(2025, 2, 14) }
    };
    }
    public class VacationDateDto
    {
        public DateTime Date { get; set; }
    }

}
