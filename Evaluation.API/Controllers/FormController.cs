using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.Services.BusinessLayer;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class FormController : ControllerBase
{
    private readonly MasterBL _masterBl;
    private readonly IMapper _mapper;
    public FormController(MasterBL masterBl, IMapper mapper)
    {
        _masterBl = masterBl;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<List<FormItemDto>> GetItems(Guid formId)
    {
        //var schooldetails = await _masterBl.GetApiService<SchoolBL>().GetSchoolDetails(SchoolID);

        var dummyData = new List<FormItem>() {
            new FormItem() { Id = new Guid("921d891a-e0cb-4fd4-8e53-fb3443ef0199"), EvalFormId = new Guid("e4530010-a302-41f5-935c-2599674db37a"), NameAr = "بند 1", NameEn = "Item 1" },
            new FormItem() { Id = new Guid("ecd11007-2ff6-42cb-bca2-b168de94afbc"), EvalFormId = new Guid("e4530010-a302-41f5-935c-2599674db37a"), NameAr = "بند 2", NameEn = "Item 2" },
            new FormItem() { Id = new Guid("6669fa51-2403-43e3-8cff-03ee9061722b"), EvalFormId = new Guid("9da26b46-4c06-429a-8b3f-8a50c47f1860"), NameAr = "بند 1", NameEn = "Item 1" },
        };

        var mappedData = _mapper.Map<List<FormItemDto>>(dummyData.Where(s => s.EvalFormId == formId).ToList());

        return mappedData;
    }

}