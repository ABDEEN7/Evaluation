using AutoMapper;
using Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

namespace Evaluation.Services.MappingProfiles;

public class NSISProfile : Profile
{
    public NSISProfile()
    {
        CreateMap<SchoolDto, NSISSchool>()
           .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(d => d.NameAr, opt => opt.MapFrom(src => src.Name))
           .ForMember(d => d.NameEn, opt => opt.MapFrom(src => src.MetaData.EnglishName))
           .ForMember(d => d.Status, opt => opt.MapFrom(src => src.Status))
           .ForMember(d => d.DateLastModified, opt => opt.MapFrom(src => src.DateLastModified))
           .ForMember(d => d.Teachers, opt => opt.MapFrom(src => src.Teachers))
           .ForMember(d => d.Staff, opt => opt.MapFrom(src => src.Staff))
           .ForMember(d => d.Classes, opt => opt.MapFrom(src => src.Classes))
           .ReverseMap();

        CreateMap<StaffDto, NSISStaff>()
   .ForMember(d => d.NameAr, opt => opt.MapFrom(src => src.GivenName))
   .ForMember(d => d.NameEn, opt => opt.MapFrom(src => src.MetaData.EnglishName))
   .ForMember(d => d.QId, opt => opt.MapFrom(src => src.Identifier))
   .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
   .ForMember(d => d.Phones, opt => opt.MapFrom(src => src.Phone))
   .ForMember(d => d.Sms, opt => opt.MapFrom(src => src.Sms))
   .ForMember(d => d.Role, opt => opt.MapFrom(src => src.Role))
   .ForMember(d => d.Address, opt => opt.MapFrom(src => src.MetaData.Address))
   .ForMember(d => d.Username, opt => opt.MapFrom(src => src.Username))
   .ReverseMap();

        CreateMap<Teacher, NSISTeacher>()
   .ForMember(d => d.NameAr, opt => opt.MapFrom(src => src.GivenName))
   .ForMember(d => d.NameEn, opt => opt.MapFrom(src => src.MetaData.EnglishName))
   .ForMember(d => d.QId, opt => opt.MapFrom(src => src.Identifier))
   .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
   .ForMember(d => d.Username, opt => opt.MapFrom(src => src.Username))
   .ForMember(d => d.Phones, opt => opt.MapFrom(src => src.Phone))
   .ForMember(d => d.Sms, opt => opt.MapFrom(src => src.Sms))
   .ForMember(d => d.Role, opt => opt.MapFrom(src => src.Role))
   .ForMember(d => d.Address, opt => opt.MapFrom(src => src.MetaData.Address))
   .ReverseMap();

        CreateMap<Class, NSISClass>()
   .ForMember(d => d.NameAr, opt => opt.MapFrom(src => src.Title))
   .ForMember(d => d.NameEn, opt => opt.MapFrom(src => src.MetaData.EnglishTitle))
   .ForMember(d => d.ClassCode, opt => opt.MapFrom(src => src.ClassCode))
   .ForMember(d => d.Grade, opt => opt.MapFrom(src => src.MetaData.Grade))
   .ReverseMap();

    }
}
