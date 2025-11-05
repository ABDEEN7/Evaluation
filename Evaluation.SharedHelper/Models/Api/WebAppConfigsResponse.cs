namespace Evaluation.SharedHelper.Models.Api
{
    public class WebAppConfigsResponse
    {

        public List<UiControlDTO> UiControls { get; set; } = new List<UiControlDTO> { };
        public List<SystemSettingDTO> SystemSettings { get; set; } = new List<SystemSettingDTO> { };
        //public List<PermissionDTO> Permissions { get; set; } = new List<PermissionDTO> { };
    }
}
