using AutoMapper;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Models.Template;
using Evaluation.SharedHelper.Models.Api.ProfileDTO;
using Evaluation.SharedHelper.Models.Api.ServiceDTOs;
using Evaluation.SharedHelper.Models.Api.TemplatesDTO;

namespace Evaluation.Services.MappingProfiles;

public class MailProfile : Profile
{
    public MailProfile()
    {
        CreateMap<EmailProfile, EmailProfileDTO>();
        CreateMap<SMSProfile, SMSProfileDTO>();
        CreateMap<EmailTemplate, EmailTemplateDTO>();
        CreateMap<SMSTemplate, SMSTemplateDTO>();
        CreateMap<ServiceStatusConfiguration, ServiceStatusConfigurationDTO>();
    }
}
