using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Enums
{
   
    public enum UserProfileClaim
    {
        UserId = 0,
        Email = 1,
        CreationDate = 2,
        FullNameEn = 3,
        FullNameAr = 4,
        LastLogin = 5,
        PreferredLang = 6,
        UserType = 7,
        TokenExpirationTime = 8,
        Mobile = 9,
    }

    public enum UserType
    {
        Ministry = 0,
        SchoolManager=1,
    }

    public enum DBResult
    {
        Inserted = 1,
        Updated = 2,
        Deleted = 3,
        Exist = 4,
        NotFound = 5,
        Error = 6,
        SpecialError = 7,
        HaveRecords = 8,
        ExceedRecord = 9,
        CannotBeParent = 10,
        SameRowColumn = 11,
        BackendExist = 12,
        PrefixExist = 13,
        ServiceFreezed = 14,
        ParentExists = 15,
        CoreColumn = 16,
        SameRecord = 17,
        CustomField = 17,
    }

    public enum StorageContainerType
    {
        [Description("evaluation")]
        evaluation = 0,
        [Description("website")]
        website = 1,

    }

	public enum RequestType
	{
		Service = 1,
		Evaluation = 2
	}

    public enum ItemPropertyType
    {
        Select = 1,
        Note
    }
}
