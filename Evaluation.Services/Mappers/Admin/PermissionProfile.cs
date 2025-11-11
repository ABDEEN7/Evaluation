using AutoMapper;
using Evaluation.DAL.Entities.PermissionEntity;
using Evaluation.SharedHelper.Models.Admin;


namespace Evaluation.Services.Mappers
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<Permission, PermissionDTO>();
            
        }
    }
}
