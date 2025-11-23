using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.FormLayer;
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
        return await _masterBl.GetApiService<FormBL>().GetFormItems(formId);
    }

}