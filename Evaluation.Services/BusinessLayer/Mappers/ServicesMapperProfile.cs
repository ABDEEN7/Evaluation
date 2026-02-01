using AutoMapper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.SharedHelper.Models.Admin;
using Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.StatusDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.BusinessLayer.Mappers
{
	public class ServicesMapperProfile : Profile
	{
		public ServicesMapperProfile()
		{
			CreateMap<DAL.Models.ActionEntities.ServiceAction, SharedHelper.Models.Api.ActionEntitiesDTOs.ActionDTO>().ReverseMap();
			CreateMap<ServiceStatus, StatusDTO>().ReverseMap();
			//CreateMap<Service, ServiceDTO>().ReverseMap();
			CreateMap<ActionField, ActionFieldDTO>().ReverseMap();
			CreateMap<ActionType, SharedHelper.Models.Api.ActionEntitiesDTOs.ActionTypeDTO>().ReverseMap();
			CreateMap<ActionPartyType, ActionPartyTypeDTO>().ReverseMap();
			CreateMap<AcademicYear, AcademicYearDTO>().ReverseMap();
		}
	}
}
