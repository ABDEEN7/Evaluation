using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.Admin;

public class SrvControlValidationBL : AdminBase
{
    public SrvControlValidationBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
    {

    }
    public async Task<List<ControlValidationDTO>> GetControlValidationList(AdminSearchDTO message)
    {



        var list = await uow.GetRepository<ControlValidation>()
            .GetAllNonDeleted()
            .Include(x => x.CreateBy)
            .Include(x => x.Permission)
            .OrderByDescending(x => x.CreateDate)
            .Select(x => new ControlValidationDTO
            {
                PermissionName = x.Permission.BackendName,
                PermissionId = x.PermissionId,
                Id = x.Id,
                ControlName = x.Name,
                ControlType = x.ControlType,
                JsonFiled = x.JsonField,
                UibackendName = x.UibackendName,
                IsRequired = x.IsRequired,
                MinLength = x.MinLength,
                MaxLength = x.MaxLength,
                Dbrequired = x.IsDbrequired,
                DbMaxLength = x.DbMaxLength,
                RowOrder = x.RowOrder,
                ColumnOrder = x.ColumnOrder,
                ShowInGrid = x.ShowInGrid,
                TabulatorConfig = x.TabulatorConfigJson,
                FileCount = x.MaxFileCount,
                FileExtention = x.FileExtention,
                FileSize = x.MaxFileSize,
                ControlJsonConfig = x.ControlJsonConfig,
                ControlAttribute = x.ControlAttribute,
                IsActive = x.IsActive,
                Regex = x.Regex,
                UpdateBy = (x.UpdateBy != null ? (_requestInfo.Lang == "ar" ? x.UpdateBy.NameAr : x.UpdateBy.NameEn) : (_requestInfo.Lang == "ar" ? x.CreateBy!.NameAr : x.CreateBy!.NameEn)),
                UpdateDate = (x.UpdateDate.HasValue ? x.UpdateDate.Value.ToString() : x.CreateDate.ToString()),
            })
            .ToListAsync();


        if (!string.IsNullOrEmpty(message.Title))
        {
            list = list.Where(c =>
(!string.IsNullOrEmpty(c.PermissionName) && c.PermissionName.Contains(message.Title, StringComparison.OrdinalIgnoreCase)) ||
(!string.IsNullOrEmpty(c.ControlName) && c.ControlName.Contains(message.Title, StringComparison.OrdinalIgnoreCase)) ||
(!string.IsNullOrEmpty(c.ControlType) && c.ControlType.Contains(message.Title, StringComparison.OrdinalIgnoreCase)) ||
(!string.IsNullOrEmpty(c.UibackendName) && c.UibackendName.Contains(message.Title, StringComparison.OrdinalIgnoreCase))
).ToList();
        }
        var result = list.Skip(message.PageNum!.Value * message.PageSize!.Value).Take(message.PageSize.Value).ToList();


        return result;

    }



    public async Task<ControlValidationDTO> UpdateControlValidation(ControlValidationDTO message)
    {


        var result = new ControlValidationDTO();




        if (message.Id is not null)
        {
            ControlValidation? Obj = await uow.GetRepository<ControlValidation>()
                                    .GetAllNonDeleted()
                                    .Where(x => x.Id == message.Id)
                                    .FirstOrDefaultAsync();

            if (Obj != null)
            {
                Obj.ControlType = message.ControlType;
                Obj.JsonField = message.JsonFiled;
                Obj.IsRequired = (Obj.IsDbrequired == true ? true : message.IsRequired);
                Obj.MinLength = message.MinLength;
                Obj.MaxLength = (Obj.DbMaxLength < message.MaxLength ? Obj.DbMaxLength : message.MaxLength);
                Obj.Regex = message.Regex;
                Obj.RowOrder = message.RowOrder;
                Obj.ColumnOrder = message.ColumnOrder;
                Obj.ShowInGrid = message.ShowInGrid;
                Obj.TabulatorConfigJson = message.TabulatorConfig;
                Obj.MaxFileCount = message.FileCount;
                Obj.FileExtention = message.FileExtention;
                Obj.MaxFileSize = message.FileSize;
                Obj.ControlJsonConfig = message.ControlJsonConfig;
                Obj.ControlAttribute = message.ControlAttribute;
                Obj.IsActive = message.IsActive;

                uow.GetRepository<ControlValidation>().Update(Obj);
                await uow.CommitAsync();
                message.UpdateBy = userInfo.DBName;
                message.IsRequired = Obj.IsRequired;
                message.MaxLength = Obj.MaxLength;
                message.ResponseStatus = DBResult.Updated;
            }
        }

        return message;

    }


}
