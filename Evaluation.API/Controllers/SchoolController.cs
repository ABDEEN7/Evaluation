using Evaluation.DAL.Entities.Org;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class SchoolController:ControllerBase
{

    private readonly MasterBL _masterBl;
    public SchoolController(MasterBL masterBl)
    {
        this._masterBl = masterBl;
    }

    [HttpGet]

    public async Task<School> GetSchoolDetails(Guid SchoolID)
    {
        var schooldetails = await _masterBl.GetApiService<SchoolBL>().GetUniversityDetails(SchoolID);
        
        var dummyData = new List<School>() {
            new School() { Id = new Guid("921d891a-e0cb-4fd4-8e53-fb3443ef0199"), NameAr = "مدرسة احمد بن حنبل", NameEn = "Ahmad Bin Hanbal School"},
            new School() { Id = new Guid("ecd11007-2ff6-42cb-bca2-b168de94afbc"), NameAr = "مدرسة عائشة", NameEn = "Aesha School" }
        };

        return dummyData.FirstOrDefault(s => s.Id == SchoolID);
    }
}