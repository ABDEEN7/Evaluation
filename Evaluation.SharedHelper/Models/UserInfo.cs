

namespace Evaluation.SharedHelper.Models
{
    public class UserInfo
    {
        public Guid? UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string DBName { get; set; }
        public string UserType { get; set; }
        public string QID { get; set; }
        public string Mobile { get; set; }
        public string Role { get; set; }

        public List<string> PermissionList { get; set; }
        public List<Guid> PartyTypes { get; set; }
        public DateTime LastLogin { get; set; }


    }
}