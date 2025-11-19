using AutoMapper;
using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Newtonsoft.Json.Linq;


namespace Evaluation.Services.Mappers.Admin
{
    public class PlaceHolderProfile : Profile
    {
        public PlaceHolderProfile()
        {
            CreateMap<PlaceHolder, PlaceHolderDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted ?? false))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.Field,
                opt => opt.MapFrom<FieldTitleResolver>())
                .ForMember(dest => dest.FieldId, opt => opt.MapFrom(src => src.ColumnName ?? src.FieldId.ToString()))
                .ForMember(dest => dest.ChildFieldIds,
                opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.ChildFieldIds)
                        ? Array.Empty<string>()
                        : src.ChildFieldIds.Split(",", StringSplitOptions.RemoveEmptyEntries)));
        }


    }
    public class FieldTitleResolver : IValueResolver<PlaceHolder, PlaceHolderDTO, string?>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public FieldTitleResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        private Dictionary<string, string> ParseSettingToDict(string json)
        {
            var dict = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(json))
            {
                var jsonArray = JArray.Parse(json);
                foreach (var item in jsonArray)
                {
                    var id = (string)item["Id"]!;
                    var title = _requestInfo.Lang == "ar" ? (string)item["TitleAr"]! : (string)item["TitleEn"]!;
                    if (!string.IsNullOrEmpty(id))
                    {
                        dict[id] = title;
                    }
                }
            }
            return dict;
        }

        public string? Resolve(PlaceHolder src, PlaceHolderDTO dest, string? destMember, ResolutionContext context)
        {
            var fields = _uow.GetRepository<Field>()
            .GetAllActiveNonDeleted()
            .ToDictionary(x => x.Id, x => _requestInfo.Lang == "ar" ? x.TitleAr : x.TitleEn);

           

            var EvaluationColumnJson = _uow.GetRepository<SystemSetting>()
            .GetAllActiveNonDeleted(x => x.SettingKey == ConstantKeys.AdminSettings.EvaluationColumn)
            .Select(x => x.SettingValue).FirstOrDefault();
            if (EvaluationColumnJson != null)
            {
                var EvaluationColumnDict = ParseSettingToDict(EvaluationColumnJson);

                var requestColumnJson = _uow.GetRepository<SystemSetting>()
            .GetAllActiveNonDeleted(x => x.SettingKey == ConstantKeys.AdminSettings.RequestColumn)
            .Select(x => x.SettingValue).FirstOrDefault();
                if (requestColumnJson != null)
                {
                    var requestColumnDict = ParseSettingToDict(requestColumnJson);

                    if (src.TypeDisplay == "Request")
                    {
                        if (src.Type == "RequestColumn")
                            return requestColumnDict!.GetValueOrDefault(src.ColumnName);
                        return src.FieldId.HasValue && fields.ContainsKey(src.FieldId.Value)
                            ? fields[src.FieldId.Value] : null;
                    }
                    else
                    {
                        if (src.Type == "EvaluationColumn")
                            return EvaluationColumnDict!.GetValueOrDefault(src.ColumnName);
                        return src.FieldId.HasValue && fields.ContainsKey(src.FieldId.Value)
                            ? fields[src.FieldId.Value] : null;
                    }
                }

            }
            return string.Empty;
        
    }
    }


}
