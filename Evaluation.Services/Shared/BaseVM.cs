using Evaluation.DAL.Helper;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using static Evaluation.SharedHelper.Enums.ConstantKeys;


namespace Evaluation.Services.Shared
{
    public class BaseVM
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IServiceProvider serviceProvider;
        private readonly UserInfo UserInfo;
        private readonly MasterBL masterBL;
        private readonly RequestInfo requestInfo;


        public BaseVM(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            serviceProvider = this.httpContextAccessor.HttpContext.RequestServices;
            UiControlItems = new List<UiControlItemDTO>();
            PermissionsList = new List<string>();
            ControlsList = new List<UiControlDTO>();
            masterBL = serviceProvider.GetRequiredService<MasterBL>();
            UserInfo = serviceProvider.GetRequiredService<UserInfo>();
            requestInfo = serviceProvider.GetRequiredService<RequestInfo>();
            PermissionsList = UserInfo.PermissionList;
        }


        public async Task LoadAllAData(string[]? PageNamesList = null, string[]? ControlValidationPermissionBackendNameList = null, string[]? SettingKeyList = null)
        {
            //dotn't use Task.WhenAll this tasks depandent on each other
            await LoadAllUiControls(PageNamesList!);
            await LoadAllControlValidations(ControlValidationPermissionBackendNameList!);
            await LoadASystemSettings(SettingKeyList!);
        }

        private async Task LoadAllUiControls(params string[] PageNamesList)
        {
            if (PageNamesList != null && PageNamesList.Any())
            {
                var ControlsListnew = await masterBL.GetAdminService<SrvBaseBL>().GetUiControls(PageNamesList.ToList(), requestInfo.Lang);
                ControlsList.AddRange(ControlsListnew);
            }
            List<string> AdminComm = new List<string> { AdminPages.AdminCommon };
            var uiControlsCommon = await masterBL.GetAdminService<SrvBaseBL>().GetUiControls(AdminComm, requestInfo.Lang);
            if (uiControlsCommon.Count > 0)
            {
                ControlsList.AddRange(uiControlsCommon);
            }

        }

        private async Task LoadAllControlValidations(params string[] ControlValidationPermissionBackendNameList)
        {

            if (ControlValidationPermissionBackendNameList != null && ControlValidationPermissionBackendNameList.Any())
            {
                foreach (var ControlValidationPermissionBackendName in ControlValidationPermissionBackendNameList)
                {
                    Constraints = await masterBL.GetAdminService<SrvSystemSettingBL>().GetAppConstraints(ControlValidationPermissionBackendName);

                    foreach (var constraint in Constraints.OrderBy(x => x.RowOrder))
                    {
                        var item = new UiControlItemDTO
                        {
                            Constraint = constraint,
                            Control = constraint == null || string.IsNullOrEmpty(constraint.UibackendName)
    ? new UiControlDTO()
    : ControlsList.FirstOrDefault(x => x.BackEndName == constraint.UibackendName) ?? new UiControlDTO(),
                            Lang = requestInfo.Lang,
                            ControlName = constraint == null || string.IsNullOrEmpty(constraint.ControlName) ? "" : constraint.ControlName,
                            UibackendName = constraint == null || string.IsNullOrEmpty(constraint.UibackendName) ? null : constraint.UibackendName,
                            RowOrder = constraint!.RowOrder,
                            ControlType = constraint == null || string.IsNullOrEmpty(constraint.ControlType) ? "" : constraint.ControlType,
                            TabulatorConfig = constraint == null || string.IsNullOrEmpty(constraint.TabulatorConfig) ? null : constraint.TabulatorConfig,
                            ControlJsonConfig = constraint == null || string.IsNullOrEmpty(constraint.ControlJsonConfig) ? null : constraint.ControlJsonConfig,


                        };

                        //if (!string.IsNullOrEmpty(item.Constraint.ControlJsonConfig))
                        //    item.Constraint.ControlJsonConfig = JsonConvert.SerializeObject(item.Constraint.ControlJsonConfig);
                        UiControlItems.Add(item);
                    }
                }
            }
            UiControlItems = UiControlItems.OrderBy(x => x.RowOrder).ToList();
        }

        private async Task LoadASystemSettings(params string[] SettingKeyList)
        {
            if (SettingKeyList != null && SettingKeyList.Any())
            {
                SettingList = await masterBL.GetAdminService<SrvSystemSettingBL>().GetSettings(SettingKeyList.ToList());
            }

        }

        public async Task<List<GeneralListDTO>> GetSettingList(string[] settigKeys)
        {
            try
            {


                var SettingTask = await masterBL.GetAdminService<SrvSystemSettingBL>()
                                                .GetSettings(settigKeys.ToList());
                var Settintaskvalue = SettingTask.Select(x => x.SettingValue).FirstOrDefault();
                List<GeneralListDTO> rslt = new List<GeneralListDTO>();

                if(Settintaskvalue!=null)
                {
                    var jsonArray = JArray.Parse(Settintaskvalue);
                    foreach (var data in jsonArray)
                    {
                        GeneralListDTO rslt1 = new GeneralListDTO();
                        rslt1.Value = (string)data["Value"]!;
                        rslt1.Title = requestInfo.Lang == "ar" ? (string)data["TextAr"]! : (string)data["TextEn"]!;
                        rslt.Add(rslt1);
                    }
                }
                
                return rslt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        public string CurrentLanguage
        {
            get
            {

                return requestInfo.Lang;
            }
        }


        public List<UiControlDTO> ControlsList { get; set; } = new();

        public IEnumerable<ControlValidationDTO> Constraints { get; set; } = Enumerable.Empty<ControlValidationDTO>();

        public List<UiControlItemDTO> UiControlItems { get; set; } = new();

        public List<string> PermissionsList { get; set; } = new();
        public List<SystemSettingDTO> SettingList { get; set; } = new();

        public List<GeneralListDTO> TargetSetting { get; set; } = new();
        public bool? containsOrderNo { get; set; }

    }
}
